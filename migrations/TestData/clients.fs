module clients

open common

let client name isAdmin = sprintf "INSERT INTO dbo.Clients VALUES ('%s', %i)" name (boolToBit isAdmin)

let add connection database =
    let connection = createConnectionString connection database
    [
        client "Main Client" true
        client "Client 2" false
        client "Client 3" false
        client "Client 4" false
        client "Client 5" false
    ]
    |> List.iter (fun sql -> migration.executeNonQuery connection sql)