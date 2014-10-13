module comments

open FSharp.Data

[<Literal>]
let private getCommentsByIssueIdQuery = """
SELECT * FROM dbo.Comments
WHERE IssueId = @IssueId"""

type GetCommentsByIssueIdQuery = SqlCommandProvider<getCommentsByIssueIdQuery, "name=IssueTracker">

let getCommentsByIssueId issueId =
    let cmd = new GetCommentsByIssueIdQuery()    
    cmd.AsyncExecute(IssueId = issueId) 
    |> Async.RunSynchronously
    |> List.ofSeq