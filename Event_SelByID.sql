﻿IF EXISTS (SELECT * FROM sys.objects WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[Event_SelByID]') AND type IN (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Event_SelByID]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Event_SelByID]
(
	@ID BIGINT
)
AS
BEGIN
	SELECT
		[Data]
	FROM [Event] WITH (NOLOCK)
	WHERE SequenceID = @ID
END
GO