module client

open FSharp.Data

[<Literal>]
let private getClientByIdQuery = """SELECT * FROM dbo.Clients WHERE Id = @ClientId"""

type GetClientByIdQuery = SqlCommandProvider<getClientByIdQuery, "name=IssueTracker">

let getClientById clientId =
    let cmd = new GetClientByIdQuery()    
    cmd.AsyncExecute(ClientId = clientId) 
    |> Async.RunSynchronously
    |> Seq.head