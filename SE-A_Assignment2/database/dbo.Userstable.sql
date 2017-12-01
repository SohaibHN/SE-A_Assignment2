CREATE TABLE [dbo].[users] (
    [Id]          INT  NOT NULL,
    [Username]        TEXT NOT NULL,
    [First Name] TEXT NULL,
    [Surname]     TEXT NULL,
    [Email]      TEXT NULL,
    [Deadline]    DATE NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

