CREATE TABLE [dbo].[tickets] (
    [Id]          INT  NOT NULL,
    [User]        TEXT NULL,
    [Description] TEXT NULL,
    [Project]     TEXT NULL,
    [Status]      TEXT NULL,
    [Severity]    TEXT NULL,
    [Date Logged] DATE NULL,
    [Deadline]    DATE NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

