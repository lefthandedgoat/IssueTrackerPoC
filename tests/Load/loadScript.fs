module loadScript

open System.Net
open System.Diagnostics
open System
open Microsoft.FSharp.Control.WebExtensions
open CustomWebClient

let asyncFetch name (url:string) (webClient : System.Net.WebClient) =    
    async {
        //printfn "fetching %s" name
        let uri = new System.Uri(url)        
        let stopwatch = Stopwatch()
        stopwatch.Start()
        let! html = webClient.AsyncDownloadString(uri)
        stopwatch.Stop()
        return stopwatch.Elapsed.Milliseconds
    }

let fetch name (url:string) (webClient : System.Net.WebClient) =       
    //printfn "fetching %s" name
    let uri = new System.Uri(url)        
    let stopwatch = Stopwatch()
    stopwatch.Start()
    let html = webClient.DownloadString(uri)
    stopwatch.Stop()
    stopwatch.Elapsed.Milliseconds

let random = Random()

let done' () = 
    //printfn "done!"
    0
let asyncHome webClient = asyncFetch "home" "http://localhost:48214/" webClient |> Async.RunSynchronously
let home webClient = fetch "home" "http://localhost:48214/" webClient

type work =   
    | Die   
    | Goto of (unit -> int)
    | Sleep of int * int
    | Done
    
type report =
    | Die
    | Stats of int
    | Instructions of AsyncReplyChannel<int>    

type worker = { id : int; worker : MailboxProcessor<work> }

type manage =
    | Die
    | Enlist of worker
    | Go
    | Done of int    

type expectations = { minUsers : int; totalUsers : int; avgResponseTimeInMS : int }

let expectations = { minUsers = 5; totalUsers = 500; avgResponseTimeInMS = 300 }
let avgLastXItems = 20
let numberOfTimesToDoSameBoringThing = 10

let login () =
    let client = new CustomWebClient()        
    //todo simply turned off auth for now
    client

let doWork (w : MailboxProcessor<work>) =
    w.Post(Sleep(50,900))
    use client = login()
    [1 .. numberOfTimesToDoSameBoringThing] 
    |> List.iter (fun _ -> 
        w.Post(Goto(fun _ -> home client))        
        w.Post(Sleep(50,100))
        )
    w.Post(Goto(done'))
    w.Post(work.Done)

let accountant (expectations : expectations) =
    MailboxProcessor.Start(fun inbox ->
        let rec loop (results : float list) (currentUsers : int) =   
            async { let! msg = inbox.Receive()   
                    match msg with   
                    | report.Die -> return ()
                    | Instructions(reply) -> 
                        //printfn "cu = %i" currentUsers
                        let count = results |> List.length                        
                        let avg = 
                            if  count >= avgLastXItems then results |> Seq.take avgLastXItems |> Seq.average
                            else results |> List.average
                        if currentUsers < expectations.minUsers then
                            reply.Reply(2)
                            printfn "Adding %i! cause minuser too low" 1
                            return! loop results (currentUsers + 1)
                        else if currentUsers >= expectations.totalUsers then
                            reply.Reply(0)
                            printfn "Not adding cause current = %i!" currentUsers
                            return! loop results (currentUsers - 1)                            
                        else if avg > (float expectations.avgResponseTimeInMS) then
                            reply.Reply(0)
                            printfn "Not adding cause avg = %f and current = %i" avg currentUsers
                            return! loop results (currentUsers - 1)                            
                        else                            
                            reply.Reply(2)
                            printfn "Adding 1 cause the system can take more load!!!!!"
                            return! loop results (currentUsers + 1)
                    | Stats(ms) -> 
                        
                        printfn "done in: %i" ms
                        return! loop ([float ms] @ results) currentUsers }   
        loop [] 0)
        
let manager (accountant : MailboxProcessor<report>) =
    MailboxProcessor.Start(fun inbox ->
        let rec loop (workers : worker list) (busyWorkers : worker list) =   
            async { let! msg = inbox.Receive()   
                    match msg with   
                    | manage.Die -> return ()
                    | manage.Go ->
                        let worker = List.head workers
                        doWork worker.worker
                        return! loop (List.tail workers) ([worker] @ busyWorkers)
                    | manage.Enlist(worker) -> 
                        let workers = [worker] @ workers
                        return! loop workers busyWorkers
                    | manage.Done(id) -> 
                        let! numberOfNeededWorkers = accountant.PostAndAsyncReply(Instructions) 
                        //printfn "adding %i workers" numberOfNeededWorkers                        
                        let worker = busyWorkers |> List.find (fun w -> w.id = id)
                        let busyWorkers = busyWorkers |> List.filter (fun w -> w.id <> id) //remove that guy
                        let workers = [worker] @ workers
                        let aboutToBeBusy = workers |> Seq.take numberOfNeededWorkers |> List.ofSeq                                                
                        
                        aboutToBeBusy |> List.iter (fun w -> doWork w.worker)
                        let busyWorkers = aboutToBeBusy @ busyWorkers
                        let workers = 
                            workers
                            |> Seq.skip numberOfNeededWorkers
                            |> List.ofSeq
                        //printfn "working: %i" workers.Length
                        printfn "busy: %i" busyWorkers.Length

                        return! loop workers busyWorkers }
        loop [] [])

let worker (id : int) (accountant : MailboxProcessor<report>) (manager : MailboxProcessor<manage>) =   
    MailboxProcessor.Start(fun inbox ->
        let rec loop ms =   
            async { let! msg = inbox.Receive()   
                    match msg with   
                    | work.Die -> 
                        return ()
                    | Sleep(from, too) ->
                        let milliseconds = random.Next(from, too)
                        //if milliseconds > 10 then printfn "sleeping for %i ms" milliseconds
                        do! Async.Sleep(milliseconds)
                        return! loop ms
                    | Goto(func) ->
                        return! loop <| func () + ms 
                    | work.Done ->
                        accountant.Post(Stats(ms))
                        manager.Post(Done(id))
                        return! loop 0 }
        loop 0)  
        
let run () =
    let accnt = accountant <| expectations
    let mgr = manager accnt

    let workers =
        [ 1 .. 5000 ]
        |> List.map (fun id -> { id = id; worker = worker id accnt mgr })
    
    workers |> List.iter (fun w -> mgr.Post(Enlist(w)))  

    mgr.Post(Go)

    System.Console.ReadKey() |> ignore

    workers |> List.iter (fun w -> w.worker.Post(work.Die))
    accnt.Post(report.Die)
    mgr.Post(manage.Die)

    System.Console.ReadKey() |> ignore