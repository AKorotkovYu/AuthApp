CREATE TABLE [dbo].[Users] (
    [Id]                       INT            IDENTITY (1, 1) NOT NULL,
    [Nickname]                 NVARCHAR (MAX) NULL,
    [Email]                    NVARCHAR (MAX) NULL,
    [Password]                 NVARCHAR (MAX) NULL,
    [Role]                     NVARCHAR (MAX) NULL,
    [MessageActivity]          INT            NOT NULL,
    [DateOfRegistration]       DATETIME2 (7)  NOT NULL,
    [LastMessageTime]          DATETIME2 (7)  NOT NULL,
    [AverageMessageCountInDay] FLOAT          NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

