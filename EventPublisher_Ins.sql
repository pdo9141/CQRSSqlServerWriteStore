IF EXISTS (SELECT * FROM sys.objects WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[EventPublisher_Ins]') AND type IN (N'P', N'PC'))
	DROP PROCEDURE [dbo].[EventPublisher_Ins]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EventPublisher_Ins]
(
	@SequenceID [BIGINT]
)
AS
BEGIN
	
	INSERT INTO [EventPublisher] ([SequenceID], [Timestamp])
	VALUES (@SequenceID, GETUTCDATE())

END
GO