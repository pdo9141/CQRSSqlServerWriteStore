﻿IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'EventValueType' AND ss.name = N'dbo')
CREATE TYPE [dbo].[EventValueType] AS TABLE (
	[Data] [NVARCHAR](MAX) NOT NULL
)
GO