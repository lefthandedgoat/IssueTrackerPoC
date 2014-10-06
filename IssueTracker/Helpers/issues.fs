module issues

open FSharp.Data

[<Literal>]
let private getIssuesByClientQuery = """SELECT TOP 100 * FROM dbo.Issues WHERE ClientId = @ClientId"""

type GetIssuesByClientQuery = SqlCommandProvider<getIssuesByClientQuery, "name=IssueTracker">

let getIssuesByClientId clientId =
    let cmd = new GetIssuesByClientQuery()    
    cmd.AsyncExecute(ClientId = clientId) 
    |> Async.RunSynchronously
    |> List.ofSeq    