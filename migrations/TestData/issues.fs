module issues

open common

let issue issueTemplateId clientId desc userId = sprintf "
INSERT INTO [dbo].[Issues] 
        ([IssueTemplateId], [ClientId], [Description], [CreatedByUserId])
VALUES  (%i,                %i,         '%s',          %i)" issueTemplateId clientId desc userId

let add connection database =
    let connection = createConnectionString connection database
    [
        issue 1 1 "Example SOW" 1
        issue 2 1 "Example PROD" 1
        issue 3 1 "Example QA" 1
        issue 4 1 "Example ONBOARD" 1
        issue 5 1 "Example BOUNTY" 1
    ]
    |> List.iter (fun sql -> migration.executeNonQuery connection sql)