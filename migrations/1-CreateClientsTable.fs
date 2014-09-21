module _1_CreateClientsTable

open migration

let m = {
    id = 1
    description = "Add clients table"
    up = """    
CREATE TABLE [dbo].[Clients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[IsAdmin] [varchar](255) NOT NULL	
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[Clients] ADD  CONSTRAINT [DF_Clients_IsAdmin]  DEFAULT ((0)) FOR [IsAdmin]
    """
    down = ""
}