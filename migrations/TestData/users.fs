module users

open common
open BCrypt.Net

let user userName password clientId= 
    let salt = BCrypt.GenerateSalt()
    let password = BCrypt.HashPassword(password, salt)
    sprintf "
        INSERT INTO dbo.Users 
        ([UserName], [Salt], [Password], [FailedAttempts], [LastFailedAttempt], [LastSuccess], [IsLocked], [ClientId])
        VALUES
        ('%s',       '%s',   '%s',       0,                null,                null,          0,          %i)"
        userName salt password clientId

let add connection database =
    let connection = createConnectionString connection database
    [
        user "Admin" "test1234" 1
        user "Admin2" "test1234" 2
        user "Admin3" "test1234" 3
        user "Admin4" "test1234" 4
        user "Admin5" "test1234" 5
    ]
    |> List.iter (fun sql -> migration.executeNonQuery connection sql)