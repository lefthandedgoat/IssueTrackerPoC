module _3_CreateIssueTemplateTable


open migration

let m = {
    id = 3
    description = "Add issue templates table"
    up = """    
CREATE TABLE [dbo].[IssueTemplates](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
    [ShortDescription] [varchar](255) NOT NULL,
	[ClientId] [int] NOT NULL,
 CONSTRAINT [PK_IssueTemplates] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[IssueTemplates]  WITH CHECK ADD  CONSTRAINT [FK_Clients_IssueTemplates] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])

ALTER TABLE [dbo].[IssueTemplates] CHECK CONSTRAINT [FK_Clients_IssueTemplates]
    """
    down = ""
}