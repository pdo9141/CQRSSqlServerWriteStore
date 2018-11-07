USE [AuctionEvents]
GO

/****** Object:  Table [dbo].[EventPublisher]    Script Date: 11/3/2018 5:46:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID(N'[dbo].[EventPublisher]', N'U') IS NULL
BEGIN
	CREATE TABLE [dbo].[EventPublisher]
	(
		[SequenceID] [BIGINT] NOT NULL,
		[Timestamp] [DATETIME] NOT NULL,
		CONSTRAINT [PK_EventEventPublisher]
			PRIMARY KEY CLUSTERED ([SequenceID] ASC)
			WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY];
END;
GO




