CREATE TABLE [dbo].[Transaction_Log](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[User] [int] NOT NULL,
	[Amount] [int] NOT NULL,
	[Transaction] [int] NOT NULL,
 CONSTRAINT [PK_Transaction_Log] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Transaction_Log]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Log_Clients] FOREIGN KEY([User])
REFERENCES [dbo].[Clients] ([ID])
GO

ALTER TABLE [dbo].[Transaction_Log] CHECK CONSTRAINT [FK_Transaction_Log_Clients]
GO

ALTER TABLE [dbo].[Transaction_Log]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Log_Transactions] FOREIGN KEY([Transaction])
REFERENCES [dbo].[Transactions] ([ID])
GO

ALTER TABLE [dbo].[Transaction_Log] CHECK CONSTRAINT [FK_Transaction_Log_Transactions]
GO