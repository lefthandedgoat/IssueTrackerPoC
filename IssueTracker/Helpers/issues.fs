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

[<Literal>]
let private getIssueByIdQuery = """
SELECT * FROM dbo.Issues 
WHERE ClientId = @ClientId
AND Id = @Id"""

type GetIssueByIdQuery = SqlCommandProvider<getIssueByIdQuery, "name=IssueTracker">

let getIssueById id clientId =
    let cmd = new GetIssueByIdQuery()    
    cmd.AsyncExecute(Id = id, ClientId = clientId) 
    |> Async.RunSynchronously
    |> Seq.head