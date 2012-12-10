
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 12/10/2012 21:52:30
-- Generated from EDMX file: C:\Users\Toke Jensen\Documents\Visual Studio 2010\Projects\SliceOfPie\bdsa\SliceOfPie_BDSA\Persistence\Database.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-SliceOfPie_OnlineGUI-20121126153317];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Changes'
CREATE TABLE [dbo].[Changes] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [User_email] nvarchar(max)  NOT NULL,
    [timestamp] bigint  NULL,
    [change1] nvarchar(max)  NULL,
    [File_id] bigint  NOT NULL
);
GO

-- Creating table 'Files'
CREATE TABLE [dbo].[Files] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [serverpath] nvarchar(max)  NOT NULL,
    [deleted] tinyint  NULL,
    [Project_id] int  NOT NULL,
    [version] decimal(18,0)  NULL
);
GO

-- Creating table 'FileInstances'
CREATE TABLE [dbo].[FileInstances] (
    [id] bigint  NOT NULL,
    [User_email] nvarchar(max)  NOT NULL,
    [path] nvarchar(max)  NOT NULL,
    [deleted] tinyint  NULL,
    [File_id] bigint  NOT NULL
);
GO

-- Creating table 'FileMetaDatas'
CREATE TABLE [dbo].[FileMetaDatas] (
    [id] int  NOT NULL,
    [value] nvarchar(max)  NULL,
    [MetaDataType_Type] nvarchar(max)  NOT NULL,
    [File_id] bigint  NOT NULL
);
GO

-- Creating table 'MetaDataTypes'
CREATE TABLE [dbo].[MetaDataTypes] (
    [Type] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Projects'
CREATE TABLE [dbo].[Projects] (
    [id] int  NOT NULL,
    [title] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [email] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ProjectHasUser'
CREATE TABLE [dbo].[ProjectHasUser] (
    [Projects_id] int  NOT NULL,
    [Users_email] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id], [User_email], [File_id] in table 'Changes'
ALTER TABLE [dbo].[Changes]
ADD CONSTRAINT [PK_Changes]
    PRIMARY KEY CLUSTERED ([id], [User_email], [File_id] ASC);
GO

-- Creating primary key on [id] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [PK_Files]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id], [User_email], [File_id] in table 'FileInstances'
ALTER TABLE [dbo].[FileInstances]
ADD CONSTRAINT [PK_FileInstances]
    PRIMARY KEY CLUSTERED ([id], [User_email], [File_id] ASC);
GO

-- Creating primary key on [id], [MetaDataType_Type] in table 'FileMetaDatas'
ALTER TABLE [dbo].[FileMetaDatas]
ADD CONSTRAINT [PK_FileMetaDatas]
    PRIMARY KEY CLUSTERED ([id], [MetaDataType_Type] ASC);
GO

-- Creating primary key on [Type] in table 'MetaDataTypes'
ALTER TABLE [dbo].[MetaDataTypes]
ADD CONSTRAINT [PK_MetaDataTypes]
    PRIMARY KEY CLUSTERED ([Type] ASC);
GO

-- Creating primary key on [id] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [PK_Projects]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [email] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([email] ASC);
GO

-- Creating primary key on [Projects_id], [Users_email] in table 'ProjectHasUser'
ALTER TABLE [dbo].[ProjectHasUser]
ADD CONSTRAINT [PK_ProjectHasUser]
    PRIMARY KEY NONCLUSTERED ([Projects_id], [Users_email] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [File_id] in table 'Changes'
ALTER TABLE [dbo].[Changes]
ADD CONSTRAINT [fk_Change_File1]
    FOREIGN KEY ([File_id])
    REFERENCES [dbo].[Files]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'fk_Change_File1'
CREATE INDEX [IX_fk_Change_File1]
ON [dbo].[Changes]
    ([File_id]);
GO

-- Creating foreign key on [User_email] in table 'Changes'
ALTER TABLE [dbo].[Changes]
ADD CONSTRAINT [fk_Change_User1]
    FOREIGN KEY ([User_email])
    REFERENCES [dbo].[Users]
        ([email])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'fk_Change_User1'
CREATE INDEX [IX_fk_Change_User1]
ON [dbo].[Changes]
    ([User_email]);
GO

-- Creating foreign key on [Project_id] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [fk_File_Project1]
    FOREIGN KEY ([Project_id])
    REFERENCES [dbo].[Projects]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'fk_File_Project1'
CREATE INDEX [IX_fk_File_Project1]
ON [dbo].[Files]
    ([Project_id]);
GO

-- Creating foreign key on [File_id] in table 'FileInstances'
ALTER TABLE [dbo].[FileInstances]
ADD CONSTRAINT [fk_FileInstance_File1]
    FOREIGN KEY ([File_id])
    REFERENCES [dbo].[Files]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'fk_FileInstance_File1'
CREATE INDEX [IX_fk_FileInstance_File1]
ON [dbo].[FileInstances]
    ([File_id]);
GO

-- Creating foreign key on [File_id] in table 'FileMetaDatas'
ALTER TABLE [dbo].[FileMetaDatas]
ADD CONSTRAINT [fk_FileMetaData_File1]
    FOREIGN KEY ([File_id])
    REFERENCES [dbo].[Files]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'fk_FileMetaData_File1'
CREATE INDEX [IX_fk_FileMetaData_File1]
ON [dbo].[FileMetaDatas]
    ([File_id]);
GO

-- Creating foreign key on [User_email] in table 'FileInstances'
ALTER TABLE [dbo].[FileInstances]
ADD CONSTRAINT [fk_FileInstance_User1]
    FOREIGN KEY ([User_email])
    REFERENCES [dbo].[Users]
        ([email])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'fk_FileInstance_User1'
CREATE INDEX [IX_fk_FileInstance_User1]
ON [dbo].[FileInstances]
    ([User_email]);
GO

-- Creating foreign key on [MetaDataType_Type] in table 'FileMetaDatas'
ALTER TABLE [dbo].[FileMetaDatas]
ADD CONSTRAINT [fk_FileMetaData_MetaDataType1]
    FOREIGN KEY ([MetaDataType_Type])
    REFERENCES [dbo].[MetaDataTypes]
        ([Type])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'fk_FileMetaData_MetaDataType1'
CREATE INDEX [IX_fk_FileMetaData_MetaDataType1]
ON [dbo].[FileMetaDatas]
    ([MetaDataType_Type]);
GO

-- Creating foreign key on [Projects_id] in table 'ProjectHasUser'
ALTER TABLE [dbo].[ProjectHasUser]
ADD CONSTRAINT [FK_ProjectHasUser_Project]
    FOREIGN KEY ([Projects_id])
    REFERENCES [dbo].[Projects]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Users_email] in table 'ProjectHasUser'
ALTER TABLE [dbo].[ProjectHasUser]
ADD CONSTRAINT [FK_ProjectHasUser_User]
    FOREIGN KEY ([Users_email])
    REFERENCES [dbo].[Users]
        ([email])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectHasUser_User'
CREATE INDEX [IX_FK_ProjectHasUser_User]
ON [dbo].[ProjectHasUser]
    ([Users_email]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------