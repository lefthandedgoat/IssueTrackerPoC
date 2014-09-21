module _2_CreateUsersTable

open migration

let m = {
    id = 2
    description = "Add users table"
    up = """    
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](255) NOT NULL,
	[Salt] [varchar](255) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[FailedAttempts] [int] NOT NULL,
	[LastFailedAttempt] [datetime] NULL,
	[LastSuccess] [datetime] NULL,
	[IsLocked] [bit] NOT NULL,
    [ClientId] [int] NOT NULL
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_FailedAttempts]  DEFAULT ((0)) FOR [FailedAttempts]

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsLocked]  DEFAULT ((0)) FOR [IsLocked]

ALTER TABLE dbo.Users ADD CONSTRAINT
	FK_Users_Clients FOREIGN KEY
	(
	ClientId
	) REFERENCES dbo.Clients
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
    """
    down = ""
}