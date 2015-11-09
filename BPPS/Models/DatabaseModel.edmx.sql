
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/09/2015 13:52:07
-- Generated from EDMX file: C:\Users\Mayki\Documents\Visual Studio 2015\Projects\BPPS1\BPPS\Models\DatabaseModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-BPPS-20151013082622];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__AspNetUse__RoleI__74794A92]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK__AspNetUse__RoleI__74794A92];
GO
IF OBJECT_ID(N'[dbo].[FK__AspNetUse__UserI__0697FACD]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK__AspNetUse__UserI__0697FACD];
GO
IF OBJECT_ID(N'[dbo].[FK__feedback___feedb__02FC7413]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[feedback_questions] DROP CONSTRAINT [FK__feedback___feedb__02FC7413];
GO
IF OBJECT_ID(N'[dbo].[FK__feedback___quest__02084FDA]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[feedback_questions] DROP CONSTRAINT [FK__feedback___quest__02084FDA];
GO
IF OBJECT_ID(N'[dbo].[FK__feedbacks__Id__14270015]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[feedbacks] DROP CONSTRAINT [FK__feedbacks__Id__14270015];
GO
IF OBJECT_ID(N'[dbo].[FK__feedbacks__proje__01142BA1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[feedbacks] DROP CONSTRAINT [FK__feedbacks__proje__01142BA1];
GO
IF OBJECT_ID(N'[dbo].[FK__Users_projec__Id__4222D4EF]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users_projects] DROP CONSTRAINT [FK__Users_projec__Id__4222D4EF];
GO
IF OBJECT_ID(N'[dbo].[FK_are situated]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[departments] DROP CONSTRAINT [FK_are situated];
GO
IF OBJECT_ID(N'[dbo].[FK_are_realized_in]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_are_realized_in];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_is subsegment of]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[departments] DROP CONSTRAINT [FK_is subsegment of];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AspNetRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserClaims];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserLogins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserLogins];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[departments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[departments];
GO
IF OBJECT_ID(N'[dbo].[feedback_questions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[feedback_questions];
GO
IF OBJECT_ID(N'[dbo].[feedbacks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[feedbacks];
GO
IF OBJECT_ID(N'[dbo].[locations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[locations];
GO
IF OBJECT_ID(N'[dbo].[Projects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Projects];
GO
IF OBJECT_ID(N'[dbo].[questions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[questions];
GO
IF OBJECT_ID(N'[dbo].[Table]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Table];
GO
IF OBJECT_ID(N'[dbo].[Users_projects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users_projects];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'C__MigrationHistory'
CREATE TABLE [dbo].[C__MigrationHistory] (
    [MigrationId] nvarchar(150)  NOT NULL,
    [ContextKey] nvarchar(300)  NOT NULL,
    [Model] varbinary(max)  NOT NULL,
    [ProductVersion] nvarchar(32)  NOT NULL
);
GO

-- Creating table 'AspNetRoles'
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'AspNetUserClaims'
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(50)  NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetUserLogins'
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] nvarchar(128)  NOT NULL,
    [ProviderKey] nvarchar(128)  NOT NULL,
    [UserId] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(50)  NOT NULL,
    [Email] nvarchar(256)  NULL,
    [EmailConfirmed] bit  NOT NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberConfirmed] bit  NOT NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [LockoutEndDateUtc] datetime  NULL,
    [LockoutEnabled] bit  NOT NULL,
    [AccessFailedCount] int  NOT NULL,
    [UserName] nvarchar(256)  NULL,
    [FirstName] nvarchar(50)  NULL,
    [LastName] nvarchar(50)  NULL
);
GO

-- Creating table 'departments'
CREATE TABLE [dbo].[departments] (
    [department_id] int  NOT NULL,
    [location_id] int  NULL,
    [name] varchar(100)  NULL,
    [dep_department_id] int  NULL
);
GO

-- Creating table 'feedback_questions'
CREATE TABLE [dbo].[feedback_questions] (
    [question_id] int  NOT NULL,
    [feedback_id] int  NOT NULL,
    [result] int  NULL,
    [comment] nvarchar(50)  NULL
);
GO

-- Creating table 'feedbacks'
CREATE TABLE [dbo].[feedbacks] (
    [feedback_id] int  NOT NULL,
    [initiated] datetime  NULL,
    [received] datetime  NULL,
    [project_id] int  NULL,
    [Id] nvarchar(50)  NULL
);
GO

-- Creating table 'locations'
CREATE TABLE [dbo].[locations] (
    [location_id] int  NOT NULL,
    [town] varchar(100)  NULL,
    [country] varchar(100)  NULL
);
GO

-- Creating table 'Projects'
CREATE TABLE [dbo].[Projects] (
    [project_id] int  NOT NULL,
    [acronym] nvarchar(50)  NULL,
    [name] nvarchar(100)  NULL,
    [description] nvarchar(100)  NULL,
    [start_date] datetime  NULL,
    [end_date] datetime  NULL,
    [status] nvarchar(50)  NULL,
    [department_id] int  NULL
);
GO

-- Creating table 'questions'
CREATE TABLE [dbo].[questions] (
    [question_id] int  NOT NULL,
    [question] nvarchar(max)  NULL
);
GO

-- Creating table 'Table'
CREATE TABLE [dbo].[Table] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'Users_projects'
CREATE TABLE [dbo].[Users_projects] (
    [Id] nvarchar(50)  NOT NULL,
    [project_id] int  NOT NULL,
    [project_role] nvarchar(50)  NULL
);
GO

-- Creating table 'userRoles'
CREATE TABLE [dbo].[userRoles] (
    [UserId] nvarchar(50)  NOT NULL,
    [RoleId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUserRoles'
CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] nvarchar(50)  NOT NULL,
    [RoleId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'userRoles1Set'
CREATE TABLE [dbo].[userRoles1Set] (
    [UserId] nvarchar(50)  NOT NULL,
    [RoleId] nvarchar(128)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [MigrationId], [ContextKey] in table 'C__MigrationHistory'
ALTER TABLE [dbo].[C__MigrationHistory]
ADD CONSTRAINT [PK_C__MigrationHistory]
    PRIMARY KEY CLUSTERED ([MigrationId], [ContextKey] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetRoles'
ALTER TABLE [dbo].[AspNetRoles]
ADD CONSTRAINT [PK_AspNetRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [PK_AspNetUserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [LoginProvider], [ProviderKey], [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [PK_AspNetUserLogins]
    PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey], [UserId] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [department_id] in table 'departments'
ALTER TABLE [dbo].[departments]
ADD CONSTRAINT [PK_departments]
    PRIMARY KEY CLUSTERED ([department_id] ASC);
GO

-- Creating primary key on [question_id], [feedback_id] in table 'feedback_questions'
ALTER TABLE [dbo].[feedback_questions]
ADD CONSTRAINT [PK_feedback_questions]
    PRIMARY KEY CLUSTERED ([question_id], [feedback_id] ASC);
GO

-- Creating primary key on [feedback_id] in table 'feedbacks'
ALTER TABLE [dbo].[feedbacks]
ADD CONSTRAINT [PK_feedbacks]
    PRIMARY KEY CLUSTERED ([feedback_id] ASC);
GO

-- Creating primary key on [location_id] in table 'locations'
ALTER TABLE [dbo].[locations]
ADD CONSTRAINT [PK_locations]
    PRIMARY KEY CLUSTERED ([location_id] ASC);
GO

-- Creating primary key on [project_id] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [PK_Projects]
    PRIMARY KEY CLUSTERED ([project_id] ASC);
GO

-- Creating primary key on [question_id] in table 'questions'
ALTER TABLE [dbo].[questions]
ADD CONSTRAINT [PK_questions]
    PRIMARY KEY CLUSTERED ([question_id] ASC);
GO

-- Creating primary key on [Id] in table 'Table'
ALTER TABLE [dbo].[Table]
ADD CONSTRAINT [PK_Table]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id], [project_id] in table 'Users_projects'
ALTER TABLE [dbo].[Users_projects]
ADD CONSTRAINT [PK_Users_projects]
    PRIMARY KEY CLUSTERED ([Id], [project_id] ASC);
GO

-- Creating primary key on [UserId], [RoleId] in table 'userRoles'
ALTER TABLE [dbo].[userRoles]
ADD CONSTRAINT [PK_userRoles]
    PRIMARY KEY CLUSTERED ([UserId], [RoleId] ASC);
GO

-- Creating primary key on [UserId], [RoleId] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [PK_AspNetUserRoles]
    PRIMARY KEY CLUSTERED ([UserId], [RoleId] ASC);
GO

-- Creating primary key on [UserId], [RoleId] in table 'userRoles1Set'
ALTER TABLE [dbo].[userRoles1Set]
ADD CONSTRAINT [PK_userRoles1Set]
    PRIMARY KEY CLUSTERED ([UserId], [RoleId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserClaims]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserLogins]
    ([UserId]);
GO

-- Creating foreign key on [Id] in table 'Users_projects'
ALTER TABLE [dbo].[Users_projects]
ADD CONSTRAINT [FK__Users_projec__Id__4222D4EF]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [location_id] in table 'departments'
ALTER TABLE [dbo].[departments]
ADD CONSTRAINT [FK_are_situated]
    FOREIGN KEY ([location_id])
    REFERENCES [dbo].[locations]
        ([location_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_are_situated'
CREATE INDEX [IX_FK_are_situated]
ON [dbo].[departments]
    ([location_id]);
GO

-- Creating foreign key on [dep_department_id] in table 'departments'
ALTER TABLE [dbo].[departments]
ADD CONSTRAINT [FK_is_subsegment_of]
    FOREIGN KEY ([dep_department_id])
    REFERENCES [dbo].[departments]
        ([department_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_is_subsegment_of'
CREATE INDEX [IX_FK_is_subsegment_of]
ON [dbo].[departments]
    ([dep_department_id]);
GO

-- Creating foreign key on [department_id] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_are_realized_in]
    FOREIGN KEY ([department_id])
    REFERENCES [dbo].[departments]
        ([department_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_are_realized_in'
CREATE INDEX [IX_FK_are_realized_in]
ON [dbo].[Projects]
    ([department_id]);
GO

-- Creating foreign key on [project_id] in table 'feedbacks'
ALTER TABLE [dbo].[feedbacks]
ADD CONSTRAINT [FK__feedbacks__proje__01142BA1]
    FOREIGN KEY ([project_id])
    REFERENCES [dbo].[Projects]
        ([project_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__feedbacks__proje__01142BA1'
CREATE INDEX [IX_FK__feedbacks__proje__01142BA1]
ON [dbo].[feedbacks]
    ([project_id]);
GO

-- Creating foreign key on [feedback_id] in table 'feedback_questions'
ALTER TABLE [dbo].[feedback_questions]
ADD CONSTRAINT [FK__feedback___feedb__02FC7413]
    FOREIGN KEY ([feedback_id])
    REFERENCES [dbo].[feedbacks]
        ([feedback_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__feedback___feedb__02FC7413'
CREATE INDEX [IX_FK__feedback___feedb__02FC7413]
ON [dbo].[feedback_questions]
    ([feedback_id]);
GO

-- Creating foreign key on [question_id] in table 'feedback_questions'
ALTER TABLE [dbo].[feedback_questions]
ADD CONSTRAINT [FK__feedback___quest__02084FDA]
    FOREIGN KEY ([question_id])
    REFERENCES [dbo].[questions]
        ([question_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'feedbacks'
ALTER TABLE [dbo].[feedbacks]
ADD CONSTRAINT [FK__feedbacks__Id__14270015]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__feedbacks__Id__14270015'
CREATE INDEX [IX_FK__feedbacks__Id__14270015]
ON [dbo].[feedbacks]
    ([Id]);
GO

-- Creating foreign key on [RoleId] in table 'userRoles'
ALTER TABLE [dbo].[userRoles]
ADD CONSTRAINT [FK__userRoles__RoleI__3E1D39E1]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__userRoles__RoleI__3E1D39E1'
CREATE INDEX [IX_FK__userRoles__RoleI__3E1D39E1]
ON [dbo].[userRoles]
    ([RoleId]);
GO

-- Creating foreign key on [UserId] in table 'userRoles'
ALTER TABLE [dbo].[userRoles]
ADD CONSTRAINT [FK__userRoles__UserI__3F115E1A]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserId] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK__AspNetUse__UserI__29221CFB]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RoleId] in table 'userRoles1Set'
ALTER TABLE [dbo].[userRoles1Set]
ADD CONSTRAINT [FK__userRoles__RoleI__4D5F7D71]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__userRoles__RoleI__4D5F7D71'
CREATE INDEX [IX_FK__userRoles__RoleI__4D5F7D71]
ON [dbo].[userRoles1Set]
    ([RoleId]);
GO

-- Creating foreign key on [UserId] in table 'userRoles1Set'
ALTER TABLE [dbo].[userRoles1Set]
ADD CONSTRAINT [FK__userRoles__UserI__607251E5]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RoleId] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK__AspNetUse__RoleI__74794A92]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__AspNetUse__RoleI__74794A92'
CREATE INDEX [IX_FK__AspNetUse__RoleI__74794A92]
ON [dbo].[AspNetUserRoles]
    ([RoleId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------