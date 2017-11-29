CREATE TABLE [dbo].[Balances](
	[ID] [int] NOT NULL,
	[Balance] [int] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Balances]  WITH CHECK ADD  CONSTRAINT [FK_Balances_Clients] FOREIGN KEY([ID])
REFERENCES [dbo].[Clients] ([ID])
GO

ALTER TABLE [dbo].[Balances] CHECK CONSTRAINT [FK_Balances_Clients]
GO
