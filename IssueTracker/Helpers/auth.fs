module auth

open FSharp.Data
open BCrypt.Net

[<Literal>]
let private getUserQuery = """
SELECT Salt, Password FROM dbo.Users WHERE UserName = @UserName"""

type GetUserQuery = SqlCommandProvider<getUserQuery, "name=IssueTracker">

let getUser userName =
    let cmd = new GetUserQuery()    
    let result =
        cmd.AsyncExecute(UserName = userName) 
        |> Async.RunSynchronously
        |> List.ofSeq
    match result with
    | user :: _ -> Some user
    | _ -> None

let passwordCorrect (password : string) (hashedPasword : string) (salt : string) =    
    let hash = BCrypt.HashPassword(password, salt)
    hashedPasword = hash