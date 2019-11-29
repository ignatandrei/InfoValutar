CREATE TABLE [dbo].[ECB](
	[ExchangeFrom] [nvarchar](50) NOT NULL,
	[ExchangeTo] [nvarchar](50) NOT NULL,
	[Date] [date] NOT NULL,
	[ExchangeValue] [decimal](18, 6) NOT NULL,
 CONSTRAINT [PK_ECB] PRIMARY KEY CLUSTERED 
(
	[ExchangeFrom] ASC,
	[ExchangeTo] ASC,
	[Date] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]