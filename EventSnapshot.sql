USE [AuctionEvents]
GO

/****** Object:  Table [dbo].[EventSnapshot]    Script Date: 11/3/2018 5:46:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID(N'[dbo].[EventSnapshot]', N'U') IS NULL
BEGIN
	CREATE TABLE [dbo].[EventSnapshot]
	(
		[AggregateID] [UNIQUEIDENTIFIER] NOT NULL,
		[SerializedData] [NVARCHAR](MAX) NOT NULL,
		[Version] [INT] NOT NULL,
		CONSTRAINT FK_EventSourceEventSnapshot FOREIGN KEY (AggregateID) REFERENCES [EventSource](AggregateID)
	) ON [PRIMARY];
END;
GO




