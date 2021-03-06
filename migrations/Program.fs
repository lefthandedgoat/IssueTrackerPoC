﻿open migration

let connection = """Data Source=.\SQLExpress;Initial Catalog=%s;Integrated Security=True"""

[
    _1_CreateClientsTable.m
    _2_CreateUsersTable.m
    _3_CreateIssueTemplateTable.m
    _4_CreateIssuesTable.m
    _5_CreateCommentsTable.m
]
|> run connection "IssueTracker"

clients.add connection "IssueTracker"
users.add connection "IssueTracker"
issuetemplates.add connection "IssueTracker"
issues.add connection "IssueTracker"
comments.add connection "IssueTracker"