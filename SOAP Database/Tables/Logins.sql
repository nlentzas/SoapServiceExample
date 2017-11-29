CREATE TABLE [dbo].[Logins](
	[ID] [int] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](200) NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Logins]  WITH CHECK ADD  CONSTRAINT [FK_Logins_Clients] FOREIGN KEY([ID])
REFERENCES [dbo].[Clients] ([ID])
GO

ALTER TABLE [dbo].[Logins] CHECK CONSTRAINT [FK_Logins_Clients]
GO
