module migration

//core 'library'
type migration = { id : int; description : string; up : string; down : string }

let mutable drop = true

let executeNonQuery connectionString sql =    
    use connection = new System.Data.SqlClient.SqlConnection(connectionString)
    use command = new System.Data.SqlClient.SqlCommand()
    command.Connection <- connection
    command.CommandText <- sql
    connection.Open()
    command.ExecuteNonQuery() |> ignore

let private executeCount connectionString sql =
    use connection = new System.Data.SqlClient.SqlConnection(connectionString)
    use command = new System.Data.SqlClient.SqlCommand()
    command.Connection <- connection
    command.CommandText <- sql
    connection.Open()
    let reader = command.ExecuteReader()
    reader.Read() |> ignore
    reader.[0] |> System.Convert.ToInt32
    
let private create (connectionString : string) database =
    let connectionString = Printf.StringFormat<string->string>(connectionString)
    let master = sprintf connectionString "master"
    //drop if needed
    if drop then
        sprintf "
        IF DB_ID('%s') IS NOT NULL
	        BEGIN
	        ALTER DATABASE %s SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	        DROP DATABASE %s;					
        END
        " database database database
        |> executeNonQuery master
        
    //add
    sprintf "
    IF DB_ID('%s') IS NULL
    BEGIN
        CREATE DATABASE %s
    END
    " database database
    |> executeNonQuery master

    //back to multi user mode
    if drop then
        sprintf "ALTER DATABASE %s SET MULTI_USER WITH ROLLBACK IMMEDIATE" database
        |> executeNonQuery master

    //add migrations table
    let db = sprintf connectionString database
    "
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='MigrationInfo')
        CREATE TABLE [dbo].[MigrationInfo](
	        [Id] [int] NOT NULL,
	        [Description] [nvarchar](max) NULL,
	        [AppliedOn] [datetime] NOT NULL DEFAULT (getdate())	        
        ) ON [PRIMARY]    
    "
    |> executeNonQuery db
    
let private migrationAlreadyRun id description connectionString =
    1 <= executeCount connectionString (sprintf "SELECT COUNT(*) FROM MigrationInfo WHERE Id = %i AND Description = '%s'" id description)

let private logMigration id description connectionString =
    sprintf "INSERT INTO [dbo].[MigrationInfo] ([Id] ,[Description]) VALUES (%i ,'%s')" id description
    |> executeNonQuery connectionString

let run connectionString database migrations =
    let sw = System.Diagnostics.Stopwatch.StartNew()
    create connectionString database

    let connectionString = Printf.StringFormat<string->string>(connectionString)
    let connectionString = sprintf connectionString database

    migrations
    |> List.sortBy (fun migration -> migration.id)
    |> List.iter (fun migration ->
        if not <| migrationAlreadyRun migration.id migration.description connectionString then        
            printfn "Starting: #%i -> %s" migration.id migration.description
            try 
                printfn "Executing: %s" migration.up
                executeNonQuery connectionString migration.up
                logMigration migration.id migration.description connectionString
            with ex -> 
                printfn "ERROR: %s
                Stack: %s
                " ex.Message ex.StackTrace
                printfn "Executing: %s" migration.down
                executeNonQuery connectionString migration.down
                failwith "ABORTING MIGRATION"
            printfn "\r\n=========================================="
        else printfn "Already Run: #%i -> %s" migration.id migration.description)

    sw.Stop()
    printfn "Migration finished in %f seconds" sw.Elapsed.TotalSeconds