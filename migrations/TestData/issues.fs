module issues

open common

let number = 200

let issue issueTemplateId clientId desc userId = sprintf "
INSERT INTO [dbo].[Issues] 
        ([IssueTemplateId], [ClientId], [Description], [CreatedByUserId])
VALUES  (%i,                %i,         '%s',          %i);" issueTemplateId clientId desc userId

let add connection database =
    let sw = System.Diagnostics.Stopwatch.StartNew()
    let connection = createConnectionString connection database
    
    let sqls =
        [| 1 .. clients.number |]
        |> Array.map (fun clientId ->
            [| 1 .. number |]
            |> Array.map (fun _ ->
                [|
                    issue 1 clientId (sprintf "Client %i Example SOW" clientId) clientId
                    issue 2 clientId (sprintf "Client %i Example PROD" clientId) clientId
                    issue 3 clientId (sprintf "Client %i Example QA" clientId) clientId
                    issue 4 clientId (sprintf "Client %i Example ONBOARD" clientId) clientId
                    issue 5 clientId (sprintf "Client %i Example BOUNTY" clientId) clientId
                |]
            )
            |> Array.concat
        )
        |> Array.concat
                
    sqls
    |> migration.batch 5000
    |> List.iter (fun sql -> migration.executeNonQuery connection sql)
    sw.Stop()
    printfn "issues added inin %f seconds" sw.Elapsed.TotalSeconds