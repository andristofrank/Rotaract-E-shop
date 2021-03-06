USE [DB_A5778A_RotaractDB]
GO
/****** Object:  Table [dbo].[DistrictNoes]    Script Date: 02/06/2020 10.13.12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DistrictNoes](
	[DistrictNumber] [int] NOT NULL,
 CONSTRAINT [PK_Districtnoes] PRIMARY KEY CLUSTERED 
(
	[DistrictNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DonationProgram_Audit]    Script Date: 02/06/2020 10.13.13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonationProgram_Audit](
	[Donationprogram_AuditId] [int] IDENTITY(1,1) NOT NULL,
	[DonationProgramId] [int] NOT NULL,
	[DonationProgramName] [varchar](50) NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[ImageRef] [varchar](max) NOT NULL,
	[StartDate] [datetime2](0) NOT NULL,
	[EndDate] [datetime2](0) NOT NULL,
	[Total] [int] NOT NULL,
	[Updated_at] [datetime2](0) NOT NULL,
	[Operation] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Donationprograms_audit] PRIMARY KEY CLUSTERED 
(
	[Donationprogram_AuditId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DonationPrograms]    Script Date: 02/06/2020 10.13.13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonationPrograms](
	[DonationProgramId] [int] IDENTITY(1,1) NOT NULL,
	[DonationProgramName] [varchar](50) NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[ImageRef] [varchar](max) NOT NULL,
	[StartDate] [datetime2](0) NOT NULL,
	[EndDate] [datetime2](0) NOT NULL,
	[Total] [int] NOT NULL,
 CONSTRAINT [PK_Donationprograms] PRIMARY KEY CLUSTERED 
(
	[DonationProgramId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Donations]    Script Date: 02/06/2020 10.13.13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Donations](
	[DonationId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[DonationProgram_AuditId] [int] NOT NULL,
	[Amount] [int] NOT NULL,
 CONSTRAINT [PK_Donations] PRIMARY KEY CLUSTERED 
(
	[DonationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItems]    Script Date: 02/06/2020 10.13.13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItems](
	[OrderItemId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[Product_AuditId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_Orderitems] PRIMARY KEY CLUSTERED 
(
	[OrderItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 02/06/2020 10.13.13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Status] [varchar](50) NOT NULL,
	[DateStamp] [datetime2](0) NOT NULL,
	[TotalPrice] [float] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 02/06/2020 10.13.13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[ImageRef] [varchar](max) NOT NULL,
	[Price] [float] NOT NULL,
	[Inventory] [int] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products_audit]    Script Date: 02/06/2020 10.13.13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products_audit](
	[Product_AuditId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[ImageRef] [varchar](max) NOT NULL,
	[Price] [float] NOT NULL,
	[Inventory] [int] NOT NULL,
	[Updated_at] [datetime2](0) NOT NULL,
	[Operation] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Products_audit] PRIMARY KEY CLUSTERED 
(
	[Product_AuditId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShippingDetails]    Script Date: 02/06/2020 10.13.13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShippingDetails](
	[ShippingDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[Country] [varchar](50) NOT NULL,
	[City] [varchar](50) NOT NULL,
	[AddressLine] [varchar](150) NOT NULL,
	[CountyOrRegion] [varchar](150) NOT NULL,
	[PostalCode] [int] NOT NULL,
	[FirstName] [varchar](150) NOT NULL,
	[LastName] [varchar](150) NOT NULL,
	[PhoneNumber] [int] NOT NULL,
 CONSTRAINT [PK_Shippingdetails] PRIMARY KEY CLUSTERED 
(
	[ShippingDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 02/06/2020 10.13.13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[DistrictNumber] [int] NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[Role] [varchar](20) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Donations]  WITH CHECK ADD  CONSTRAINT [FK_Donations_Donationprograms_audit] FOREIGN KEY([DonationProgram_AuditId])
REFERENCES [dbo].[DonationProgram_Audit] ([Donationprogram_AuditId])
GO
ALTER TABLE [dbo].[Donations] CHECK CONSTRAINT [FK_Donations_Donationprograms_audit]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItems_contains] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_OrderItems_contains]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItems_has] FOREIGN KEY([Product_AuditId])
REFERENCES [dbo].[Products_audit] ([Product_AuditId])
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_OrderItems_has]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_has] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_has]
GO
ALTER TABLE [dbo].[ShippingDetails]  WITH CHECK ADD  CONSTRAINT [FK_ShippingDetails_Has] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
GO
ALTER TABLE [dbo].[ShippingDetails] CHECK CONSTRAINT [FK_ShippingDetails_Has]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_has] FOREIGN KEY([DistrictNumber])
REFERENCES [dbo].[DistrictNoes] ([DistrictNumber])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_has]
GO
/****** Object:  Trigger [dbo].[DonationProgram_AuditTrigger]    Script Date: 02/06/2020 10.13.13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[DonationProgram_AuditTrigger]
ON [dbo].[DonationPrograms]
FOR INSERT, UPDATE , DELETE
AS 
IF EXISTS ( SELECT 0 FROM Deleted )
        BEGIN
            IF EXISTS ( SELECT 0 FROM Inserted )
                BEGIN
                    INSERT  INTO dbo.DonationProgram_Audit
                            ( 
							  DonationProgramId ,
                              DonationProgramName ,
                              Description ,
                              ImageRef ,
							  Total,
							  StartDate,
							  EndDate,
                              Updated_at ,
                              Operation
                            )
                            SELECT  D.DonationProgramId ,
									D.DonationProgramName ,
                                    D.Description ,
                                    D.ImageRef ,
                                    D.Total ,
									StartDate,
									EndDate,
                                    GETDATE() ,
                                    'UPDATED'
                            FROM    inserted D
                END
            ELSE
                BEGIN
                    INSERT  INTO dbo.DonationProgram_Audit
                            ( 
							  DonationProgramId ,
                              DonationProgramName ,
                              Description ,
                              ImageRef ,
							  Total,
							  StartDate,
							  EndDate,
                              Updated_at ,
                              Operation
                            )
                            SELECT   D.DonationProgramId ,
									D.DonationProgramName ,
                                    D.Description ,
                                    D.ImageRef ,
                                    D.Total ,
									StartDate,
									EndDate,
                                    GETDATE() ,
                                    'DELETED'
                            FROM    Deleted D
                END  
        END
    ELSE
        BEGIN
            INSERT  INTO dbo.DonationProgram_Audit
                    (		  
						      DonationProgramId ,
                              DonationProgramName ,
                              Description ,
                              ImageRef ,
							  Total,
							  StartDate,
							  EndDate,
                              Updated_at ,
                              Operation
                    )
                    SELECT  I.DonationProgramId ,
							I.DonationProgramName ,
                            I.Description ,
                            I.ImageRef ,
                            I.Total ,
							I.StartDate,
							I.EndDate,
                            GETDATE() ,
                            'INSERTED'
                    FROM    Inserted I
        END
GO
ALTER TABLE [dbo].[DonationPrograms] ENABLE TRIGGER [DonationProgram_AuditTrigger]
GO
/****** Object:  Trigger [dbo].[Products_AuditTrigger]    Script Date: 02/06/2020 10.13.14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- End Of trigger 1

--Trigger 2 
CREATE TRIGGER [dbo].[Products_AuditTrigger]
ON [dbo].[Products]
FOR INSERT, UPDATE , DELETE
AS 
IF EXISTS ( SELECT 0 FROM Deleted )
        BEGIN
            IF EXISTS ( SELECT 0 FROM Inserted )
                BEGIN
                    INSERT  INTO dbo.Products_audit
                            ( 
							  ProductId ,
                              Name ,
                              Description ,
                              ImageRef ,
                              Price ,
							  Inventory,
                              Updated_at,
                              Operation
                            )
                            SELECT  D.Productid ,
									D.Name ,
                                    D.Description ,
                                    D.ImageRef ,
                                    D.Price ,
									Inventory,
                                    GETDATE() ,
                                    'UPDATED'
                            FROM    inserted D
                END
            ELSE
                BEGIN
                    INSERT  INTO dbo.Products_audit
                            ( ProductId ,
                              Name ,
                              Description ,
                              ImageRef ,
                              Price ,
							  Inventory,
                              Updated_at ,
                              Operation
                            )
                            SELECT  D.Productid ,
									D.Name ,
                                    D.Description ,
                                    D.ImageRef ,
                                    D.Price ,
									Inventory,
                                    GETDATE() ,
                                    'DELETED'
                            FROM    Deleted D
                END  
        END
    ELSE
        BEGIN
            INSERT  INTO dbo.Products_audit
                    (		  
						      ProductId ,
                              Name ,
                              Description ,
                              ImageRef ,
                              Price ,
							  Inventory,
                              Updated_at ,
                              Operation
                    )
                    SELECT  I.Productid ,
							I.Name ,
                            I.Description ,
                            I.ImageRef ,
                            I.Price ,
							I.Inventory,
                            GETDATE() ,
                            'INSERTED'
                    FROM    Inserted I
        END
GO
ALTER TABLE [dbo].[Products] ENABLE TRIGGER [Products_AuditTrigger]
GO
