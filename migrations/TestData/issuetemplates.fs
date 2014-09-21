module issuetemplates

open common

let template name desc clientId = sprintf "INSERT INTO dbo.IssueTemplates VALUES ('%s', '%s', %i)" name desc clientId

let add connection database =
    let connection = createConnectionString connection database
    [
        template "Statement of Work" "SOW" 1
        template "Production Issue" "PROD" 1
        template "QA Issue" "QA" 1
        template "Onboarding Task" "ONBOARD" 1
        template "Bounty" "BOUNTY" 1
    ]
    |> List.iter (fun sql -> migration.executeNonQuery connection sql)