USE [APTIFY]
GO

/****** Object:  Table [dbo].[ACSStagingContacts]    Script Date: 9/21/2021 10:45:20 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACSStagingContacts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Email] [nvarchar](255) NULL,
	[Credentials] [nvarchar](50) NULL,
	[Title] [nvarchar](100) NULL,
	[AddressLine1] [nvarchar](100) NULL,
	[AddressLine2] [nvarchar](100) NULL,
	[AddressLine3] [nvarchar](100) NULL,
	[AddressLine4] [nvarchar](100) NULL,
	[City] [nvarchar](50) NULL,
	[StateId] [int] NULL,
	[ZipCode] [nvarchar](25) NULL,
	[CountryId] [int] NULL,
	[PhoneCountryCode] [nvarchar](5) NULL,
	[PhoneAreaCode] [nvarchar](5) NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[PhoneExtension] [nvarchar](10) NULL,
	[FaxCountryCode] [nvarchar](5) NULL,
	[FaxAreaCode] [nvarchar](5) NULL,
	[FaxNumber] [nvarchar](20) NULL,
	[UseFacilityAddress] [bit] NOT NULL,
	[BillingContact] [bit] NOT NULL,
	[Status] [nvarchar](50) NULL,
	[DateCreated] [datetime] NULL,
	[DateProcessed] [datetime] NULL,
	[FunctionId] [int] NOT NULL,
	[StagingContactAppId] [int] NOT NULL,
	[PersonId] [int] NULL,
	[CompanyId] [int] NULL,
	[PrimaryContact] [bit] NOT NULL,
	[FTEPercent] [int] NULL,
	[EntityID] [int] NULL,
	[RecordID] [int] NULL,
	[StagingContactsEntityId] [int] NULL,
 CONSTRAINT [pkey_ACSStagingContacts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ACSStagingContacts] ADD  DEFAULT ((0)) FOR [UseFacilityAddress]
GO

ALTER TABLE [dbo].[ACSStagingContacts] ADD  DEFAULT ((0)) FOR [BillingContact]
GO

ALTER TABLE [dbo].[ACSStagingContacts] ADD  DEFAULT ((0)) FOR [PrimaryContact]
GO

ALTER TABLE [dbo].[ACSStagingContacts] ADD  DEFAULT ((-1)) FOR [RecordID]
GO

ALTER TABLE [dbo].[ACSStagingContacts]  WITH CHECK ADD  CONSTRAINT [fkey_ACSStagingContacts_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([ID])
GO

ALTER TABLE [dbo].[ACSStagingContacts] CHECK CONSTRAINT [fkey_ACSStagingContacts_CompanyId]
GO

ALTER TABLE [dbo].[ACSStagingContacts]  WITH CHECK ADD  CONSTRAINT [fkey_ACSStagingContacts_CountryId] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([ID])
GO

ALTER TABLE [dbo].[ACSStagingContacts] CHECK CONSTRAINT [fkey_ACSStagingContacts_CountryId]
GO

ALTER TABLE [dbo].[ACSStagingContacts]  WITH CHECK ADD  CONSTRAINT [fkey_ACSStagingContacts_EntityID] FOREIGN KEY([EntityID])
REFERENCES [dbo].[Entity] ([ID])
GO

ALTER TABLE [dbo].[ACSStagingContacts] CHECK CONSTRAINT [fkey_ACSStagingContacts_EntityID]
GO

ALTER TABLE [dbo].[ACSStagingContacts]  WITH CHECK ADD  CONSTRAINT [fkey_ACSStagingContacts_PersonId] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Person] ([ID])
GO

ALTER TABLE [dbo].[ACSStagingContacts] CHECK CONSTRAINT [fkey_ACSStagingContacts_PersonId]
GO

ALTER TABLE [dbo].[ACSStagingContacts]  WITH CHECK ADD  CONSTRAINT [fkey_ACSStagingContacts_StagingContactsEntityId] FOREIGN KEY([StagingContactsEntityId])
REFERENCES [dbo].[ACSStagingContactsEntities] ([ID])
GO

ALTER TABLE [dbo].[ACSStagingContacts] CHECK CONSTRAINT [fkey_ACSStagingContacts_StagingContactsEntityId]
GO

ALTER TABLE [dbo].[ACSStagingContacts]  WITH CHECK ADD  CONSTRAINT [fkey_ACSStagingContacts_StateId] FOREIGN KEY([StateId])
REFERENCES [dbo].[StateProvince] ([ID])
GO

ALTER TABLE [dbo].[ACSStagingContacts] CHECK CONSTRAINT [fkey_ACSStagingContacts_StateId]
GO

