IF EXISTS (SELECT * FROM sys.objects WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[EventPublisher_Sel]') AND type IN (N'P', N'PC'))
	DROP PROCEDURE [dbo].[EventPublisher_Sel]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EventPublisher_Sel]
AS
BEGIN

	SELECT
		TOP 1 SequenceID
	FROM [EventPublisher] WITH (NOLOCK)
	ORDER BY [SequenceID] DESC
END
GO