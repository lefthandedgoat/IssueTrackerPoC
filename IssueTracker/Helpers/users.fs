module user

open FSharp.Data

[<Literal>]
let private getUserByNameQuery = """SELECT Id, UserName, ClientId FROM dbo.Users WHERE UserName = @UserName"""

type GetUserByNameQuery = SqlCommandProvider<getUserByNameQuery, "name=IssueTracker">

let getUserByName userName =
    let cmd = new GetUserByNameQuery()    
    cmd.AsyncExecute(UserName = userName) 
    |> Async.RunSynchronously
    |> Seq.head