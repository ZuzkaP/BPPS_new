DROP TABLE [dbo].[questions];
CREATE TABLE [dbo].[questions] (
    [question_id] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [question]    NVARCHAR (MAX) NOT NULL,
);