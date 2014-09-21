open migration

let connection = """Data Source=.\SQLExpress;Initial Catalog=%s;Integrated Security=True"""

[
    _1_CreateClientsTable.m
    _2_CreateUsersTable.m
]
|> run connection "IssueTracker"
