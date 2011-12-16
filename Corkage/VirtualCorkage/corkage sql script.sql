/****** Object:  ForeignKey [FK_Story_Sprint]    Script Date: 11/15/2010 21:39:39 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Story_Sprint]') AND parent_object_id = OBJECT_ID(N'[dbo].[Story]'))
ALTER TABLE [dbo].[Story] DROP CONSTRAINT [FK_Story_Sprint]
GO
/****** Object:  ForeignKey [FK_Task_Category]    Script Date: 11/15/2010 21:39:39 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Task_Category]') AND parent_object_id = OBJECT_ID(N'[dbo].[Task]'))
ALTER TABLE [dbo].[Task] DROP CONSTRAINT [FK_Task_Category]
GO
/****** Object:  ForeignKey [FK_Task_Story]    Script Date: 11/15/2010 21:39:39 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Task_Story]') AND parent_object_id = OBJECT_ID(N'[dbo].[Task]'))
ALTER TABLE [dbo].[Task] DROP CONSTRAINT [FK_Task_Story]
GO
/****** Object:  Table [dbo].[Task]    Script Date: 11/15/2010 21:39:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND type in (N'U'))
DROP TABLE [dbo].[Task]
GO
/****** Object:  Table [dbo].[Story]    Script Date: 11/15/2010 21:39:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Story]') AND type in (N'U'))
DROP TABLE [dbo].[Story]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 11/15/2010 21:39:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Category]') AND type in (N'U'))
DROP TABLE [dbo].[Category]
GO
/****** Object:  Table [dbo].[Sprint]    Script Date: 11/15/2010 21:39:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sprint]') AND type in (N'U'))
DROP TABLE [dbo].[Sprint]
GO
/****** Object:  Table [dbo].[Sprint]    Script Date: 11/15/2010 21:39:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sprint]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Sprint](
	[SprintId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_Sprint] PRIMARY KEY CLUSTERED 
(
	[SprintId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Sprint]') AND name = N'IX_Sprint')
CREATE NONCLUSTERED INDEX [IX_Sprint] ON [dbo].[Sprint] 
(
	[SprintId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Sprint]') AND name = N'IX_Sprint_1')
CREATE NONCLUSTERED INDEX [IX_Sprint_1] ON [dbo].[Sprint] 
(
	[SprintId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
SET IDENTITY_INSERT [dbo].[Sprint] ON
INSERT [dbo].[Sprint] ([SprintId], [Description]) VALUES (1, N'First sprint')
SET IDENTITY_INSERT [dbo].[Sprint] OFF
/****** Object:  Table [dbo].[Category]    Script Date: 11/15/2010 21:39:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Category]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Category](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
SET IDENTITY_INSERT [dbo].[Category] ON
INSERT [dbo].[Category] ([CategoryId], [Description]) VALUES (1, N'Todo')
INSERT [dbo].[Category] ([CategoryId], [Description]) VALUES (2, N'In Progress')
INSERT [dbo].[Category] ([CategoryId], [Description]) VALUES (3, N'For Test')
INSERT [dbo].[Category] ([CategoryId], [Description]) VALUES (4, N'Complete')
SET IDENTITY_INSERT [dbo].[Category] OFF
/****** Object:  Table [dbo].[Story]    Script Date: 11/15/2010 21:39:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Story]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Story](
	[StoryId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SprintId] [int] NOT NULL,
 CONSTRAINT [PK_Story] PRIMARY KEY CLUSTERED 
(
	[StoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
SET IDENTITY_INSERT [dbo].[Story] ON
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (1, N'As a user, I want to zoom and pan the corkboard', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (2, N'As a user, I want to add a story to the corkboard', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (3, N'As a user, I want to add a task to the corkboard', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (4, N'As a user, I want to modify the text on a story on the corkboard', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (5, N'As a user, I want to modify the text on a task on the corkbaord', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (6, N'As a user, I want to rearrange the tasks for a particular story to put them in order', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (7, N'As a user, I want to change the category of a task for a particular story (todo->in progress) via UI dragging', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (8, N'As a user, I want to remove a task from a story (and from the corkboard)', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (9, N'As a user, I want to rearrange the story order (including tasks) on the corkboard (up & down)', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (10, N'As a user, I want to remove a story (and its associated tasks) from the corkboard', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (11, N'As a user, I want to pre-populate the stories and tasks on the corkboard from an external data source (XML)', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (12, N'As a user, I want to have RIA services hooked up with the corkboard', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (13, N'As a user, I want to view the corkboard with multiple other users making changes on the fly', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (14, N'As a user, I want I want to move a task from one story to another story', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (15, N'As a user, I want to add new task categories to place my tasks in', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (16, N'As a user, I want to be able to move categories around on the corkboard (left-right) UI dragging', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (17, N'As a user, I want to the corkboard to resize for stories & tasks (wrt minimum size) ', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (18, N'As a user, I can resize column widths & row heights on my coarkboard, accordingly (automated solution first)', 1)
INSERT [dbo].[Story] ([StoryId], [Description], [SprintId]) VALUES (19, N'As a user, I want ot reset my corkboard layout (auto arrange)', 1)
SET IDENTITY_INSERT [dbo].[Story] OFF
/****** Object:  Table [dbo].[Task]    Script Date: 11/15/2010 21:39:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Task](
	[TaskId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[StoryId] [int] NOT NULL,
	[Description] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[TaskId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
SET IDENTITY_INSERT [dbo].[Task] ON
INSERT [dbo].[Task] ([TaskId], [CategoryId], [StoryId], [Description]) VALUES (1, 1, 1, N'Sort out problem with capture mouse on corkboard (make corkboard a control?)')
INSERT [dbo].[Task] ([TaskId], [CategoryId], [StoryId], [Description]) VALUES (2, 1, 1, N'Allow user to adjust easing functions & timings via xaml (dep. properties)')
INSERT [dbo].[Task] ([TaskId], [CategoryId], [StoryId], [Description]) VALUES (3, 1, 1, N'Try and get the full size corkboard shown (so that its not clipped but always the same size)')
INSERT [dbo].[Task] ([TaskId], [CategoryId], [StoryId], [Description]) VALUES (4, 1, 2, N'Add story to the task board (kinda already done)')
INSERT [dbo].[Task] ([TaskId], [CategoryId], [StoryId], [Description]) VALUES (5, 1, 2, N'Put an image on the background of the story (5x3 card?)')
INSERT [dbo].[Task] ([TaskId], [CategoryId], [StoryId], [Description]) VALUES (6, 1, 2, N'Stop user from adding too many stories? Could put a limit on it for now (set max# stories on corkboard?)')
INSERT [dbo].[Task] ([TaskId], [CategoryId], [StoryId], [Description]) VALUES (7, 1, 2, N'Allow user to adjust text on story card (don''t bother with story points)')
INSERT [dbo].[Task] ([TaskId], [CategoryId], [StoryId], [Description]) VALUES (8, 1, 2, N'Stop user from typing in too much text')
INSERT [dbo].[Task] ([TaskId], [CategoryId], [StoryId], [Description]) VALUES (9, 1, 3, N'Prevent user from adding too many tasks (cat1 error) (eventually this should be a resize)')
INSERT [dbo].[Task] ([TaskId], [CategoryId], [StoryId], [Description]) VALUES (10, 1, 3, N'Put an image on the background of a task (piece of paper?)')
INSERT [dbo].[Task] ([TaskId], [CategoryId], [StoryId], [Description]) VALUES (11, 1, 3, N'Prevent user from adding too much text to task')
INSERT [dbo].[Task] ([TaskId], [CategoryId], [StoryId], [Description]) VALUES (12, 1, 3, N'Allow user to move tasks around')
SET IDENTITY_INSERT [dbo].[Task] OFF
/****** Object:  ForeignKey [FK_Story_Sprint]    Script Date: 11/15/2010 21:39:39 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Story_Sprint]') AND parent_object_id = OBJECT_ID(N'[dbo].[Story]'))
ALTER TABLE [dbo].[Story]  WITH CHECK ADD  CONSTRAINT [FK_Story_Sprint] FOREIGN KEY([SprintId])
REFERENCES [dbo].[Sprint] ([SprintId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Story_Sprint]') AND parent_object_id = OBJECT_ID(N'[dbo].[Story]'))
ALTER TABLE [dbo].[Story] CHECK CONSTRAINT [FK_Story_Sprint]
GO
/****** Object:  ForeignKey [FK_Task_Category]    Script Date: 11/15/2010 21:39:39 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Task_Category]') AND parent_object_id = OBJECT_ID(N'[dbo].[Task]'))
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([CategoryId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Task_Category]') AND parent_object_id = OBJECT_ID(N'[dbo].[Task]'))
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_Category]
GO
/****** Object:  ForeignKey [FK_Task_Story]    Script Date: 11/15/2010 21:39:39 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Task_Story]') AND parent_object_id = OBJECT_ID(N'[dbo].[Task]'))
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Story] FOREIGN KEY([StoryId])
REFERENCES [dbo].[Story] ([StoryId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Task_Story]') AND parent_object_id = OBJECT_ID(N'[dbo].[Task]'))
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_Story]
GO
