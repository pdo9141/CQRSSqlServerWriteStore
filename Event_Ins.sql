IF EXISTS (SELECT * FROM sys.objects WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[Event_Ins]') AND type IN (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Event_Ins]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Event_Ins]
(
	@AggregateID [UNIQUEIDENTIFIER],
	@Name [NVARCHAR](300),
	@Type [NVARCHAR](300),
	@Version [INT],
	@Batch [dbo].[EventValueType] READONLY
)
AS
BEGIN
	BEGIN TRANSACTION;

	BEGIN TRY
		DECLARE @EventSourceVersion INT, @Now DATETIME = GETUTCDATE();

		SELECT @EventSourceVersion = [Version]
		FROM [EventSource] WITH (NOLOCK)
		WHERE AggregateID = @AggregateID;

		IF @EventSourceVersion IS NULL
		BEGIN
			INSERT INTO [EventSource] ([AggregateID], [Type], [Version])
			VALUES (@AggregateID, @Type, 0);
		END;
		ELSE
		BEGIN
			IF @EventSourceVersion != @Version
			BEGIN
				RAISERROR('Concurrency Exception', 16, 1);
			END;
		END;

		SET @Version = @Version + 1;

		INSERT INTO [Event] ([AggregateID], [Name], [Data], [Version], [Timestamp])
		SELECT @AggregateID, @Name, [Data], @Version], @Now
		FROM @Batch;

		UPDATE [EventSource]
		SET [Version] = @Version
		WHERE AggregateID = @AggregateID
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT @ErrorMessage = ERROR_MESSAGE(),
			   @ErrorSeverity = ERROR_SEVERITY(),
			   @ErrorState = ERROR_STATE();

		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH;
	


END
GO