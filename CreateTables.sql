/****** Object:  Table [dbo].[LabelSpooler]    Script Date: 03/03/2020 17:02:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LabelSpooler](
	[jobID] [bigint] IDENTITY(1,1) NOT NULL,
	[printerName] [varchar](64) NOT NULL,
	[printData] [text] NOT NULL,
	[jobStatus] [tinyint] NOT NULL,
	[jobCreated] [datetime] NOT NULL,
	[jobRetries] [int] NOT NULL,
	[jobTag] [varchar](128) NULL,
	[jobLastSent] [datetime] NULL,
 CONSTRAINT [PK_LabelSpooler] PRIMARY KEY CLUSTERED 
(
	[jobID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[LabelSpooler] ADD  CONSTRAINT [DF_LabelSpooler_jobStatus]  DEFAULT ((0)) FOR [jobStatus]
GO

ALTER TABLE [dbo].[LabelSpooler] ADD  CONSTRAINT [DF_LabelSpooler_jobCreated]  DEFAULT (getdate()) FOR [jobCreated]
GO

ALTER TABLE [dbo].[LabelSpooler] ADD  CONSTRAINT [DF_LabelSpooler_jobRetries]  DEFAULT ((0)) FOR [jobRetries]
GO

ALTER TABLE [dbo].[LabelSpooler] ADD  CONSTRAINT [DF_LabelSpooler_lastRetry]  DEFAULT (NULL) FOR [jobLastSent]
GO