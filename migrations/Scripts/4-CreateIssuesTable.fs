module _4_CreateIssuesTable

open migration

let m = {
    id = 4
    description = "Add issue table"
    up = """    

CREATE TABLE [dbo].[Issues](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IssueTemplateId] [int] NOT NULL,
	[ClientId] [int] NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreatedByUserId] [int] NOT NULL,
 CONSTRAINT [PK_Issues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

SET ANSI_PADDING OFF

ALTER TABLE [dbo].[Issues] ADD  CONSTRAINT [DF_Issues_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]

ALTER TABLE [dbo].[Issues]  WITH CHECK ADD  CONSTRAINT [FK_Issues_Clients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])

ALTER TABLE [dbo].[Issues] CHECK CONSTRAINT [FK_Issues_Clients]

ALTER TABLE [dbo].[Issues]  WITH CHECK ADD  CONSTRAINT [FK_Issues_IssueTemplates] FOREIGN KEY([IssueTemplateId])
REFERENCES [dbo].[IssueTemplates] ([Id])

ALTER TABLE [dbo].[Issues] CHECK CONSTRAINT [FK_Issues_IssueTemplates]

ALTER TABLE [dbo].[Issues]  WITH CHECK ADD  CONSTRAINT [FK_Issues_Users] FOREIGN KEY([CreatedByUserId])
REFERENCES [dbo].[Users] ([Id])

ALTER TABLE [dbo].[Issues] CHECK CONSTRAINT [FK_Issues_Users]
    """
    down = ""
}