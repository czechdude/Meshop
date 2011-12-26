CREATE TABLE [dbo].[BasicCategory](
	[CategoryID] [int] IDENTITY(9,1) NOT NULL,
	[Name] [nvarchar](4000) COLLATE Czech_CI_AS NOT NULL,
	[Parent_CategoryID] [int] NULL
)

GO
ALTER TABLE [dbo].[BasicCategory] ADD  CONSTRAINT [PK__BasicCategory__0000000000000044] PRIMARY KEY 
(
	[CategoryID]
)
GO
CREATE TABLE [dbo].[BasicCategoryBasicProduct](
	[BasicCategory_CategoryID] [int] NOT NULL,
	[BasicProduct_ProductID] [int] NOT NULL
)

GO
ALTER TABLE [dbo].[BasicCategoryBasicProduct] ADD  CONSTRAINT [PK__BasicCategoryBasicProduct__00000000000000A8] PRIMARY KEY 
(
	[BasicCategory_CategoryID],
	[BasicProduct_ProductID]
)
GO
CREATE TABLE [dbo].[BasicProduct](
	[ProductID] [int] IDENTITY(4,1) NOT NULL,
	[Name] [nvarchar](4000) COLLATE Czech_CI_AS NOT NULL,
	[Price] [numeric](18, 2) NOT NULL
)

GO
ALTER TABLE [dbo].[BasicProduct] ADD  CONSTRAINT [PK__BasicProduct__0000000000000038] PRIMARY KEY 
(
	[ProductID]
)
GO
CREATE TABLE [dbo].[CartItem](
	[RecordID] [int] IDENTITY(1,1) NOT NULL,
	[CartID] [nvarchar](4000) COLLATE Czech_CI_AS NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL
)

GO
ALTER TABLE [dbo].[CartItem] ADD  CONSTRAINT [PK__CartItem__0000000000000054] PRIMARY KEY 
(
	[RecordID]
)
GO
CREATE TABLE [dbo].[Customer](
	[Username] [nvarchar](4000) COLLATE Czech_CI_AS NOT NULL,
	[FirstName] [nvarchar](4000) COLLATE Czech_CI_AS NOT NULL,
	[LastName] [nvarchar](4000) COLLATE Czech_CI_AS NOT NULL,
	[Email] [nvarchar](4000) COLLATE Czech_CI_AS NOT NULL,
	[Password] [nvarchar](4000) COLLATE Czech_CI_AS NOT NULL,
	[Role] [nvarchar](4000) COLLATE Czech_CI_AS NULL,
	[Enabled] [bit] NOT NULL
)

GO
ALTER TABLE [dbo].[Customer] ADD  CONSTRAINT [PK__Customer__000000000000002C] PRIMARY KEY 
(
	[Username]
)
GO
CREATE TABLE [dbo].[EdmMetadata](
	[Id] [int] IDENTITY(2,1) NOT NULL,
	[ModelHash] [nvarchar](4000) COLLATE Czech_CI_AS NULL
)

GO
ALTER TABLE [dbo].[EdmMetadata] ADD  CONSTRAINT [PK__EdmMetadata__000000000000009E] PRIMARY KEY 
(
	[Id]
)
GO
CREATE TABLE [dbo].[Order](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](4000) COLLATE Czech_CI_AS NULL,
	[FirstName] [nvarchar](160) COLLATE Czech_CI_AS NOT NULL,
	[LastName] [nvarchar](160) COLLATE Czech_CI_AS NOT NULL,
	[Address] [nvarchar](70) COLLATE Czech_CI_AS NOT NULL,
	[City] [nvarchar](40) COLLATE Czech_CI_AS NOT NULL,
	[PostalCode] [nvarchar](10) COLLATE Czech_CI_AS NOT NULL,
	[Country] [nvarchar](40) COLLATE Czech_CI_AS NOT NULL,
	[Phone] [nvarchar](24) COLLATE Czech_CI_AS NOT NULL,
	[Email] [nvarchar](4000) COLLATE Czech_CI_AS NOT NULL,
	[Total] [numeric](18, 2) NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[Shipping] [nvarchar](4000) COLLATE Czech_CI_AS NULL,
	[Payment] [nvarchar](4000) COLLATE Czech_CI_AS NULL
)

GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [PK__Order__0000000000000076] PRIMARY KEY 
(
	[OrderID]
)
GO
CREATE TABLE [dbo].[OrderDetail](
	[OrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [numeric](18, 2) NOT NULL
)

GO
ALTER TABLE [dbo].[OrderDetail] ADD  CONSTRAINT [PK__OrderDetail__0000000000000086] PRIMARY KEY 
(
	[OrderDetailID]
)
GO
CREATE TABLE [dbo].[Resource](
	[resourceKey] [nvarchar](4000) COLLATE Czech_CI_AS NOT NULL,
	[cultureCode] [nvarchar](4000) COLLATE Czech_CI_AS NOT NULL,
	[resourceType] [nvarchar](4000) COLLATE Czech_CI_AS NULL,
	[resourceValue] [nvarchar](4000) COLLATE Czech_CI_AS NULL
)

GO
ALTER TABLE [dbo].[Resource] ADD  CONSTRAINT [PK__Resource__000000000000000A] PRIMARY KEY 
(
	[resourceKey],
	[cultureCode]
)
GO
CREATE TABLE [dbo].[Review](
	[ReviewID] [int] IDENTITY(2,1) NOT NULL,
	[Text] [nvarchar](4000) COLLATE Czech_CI_AS NULL,
	[ProductID] [int] NOT NULL,
	[CustomerUserName] [nvarchar](4000) COLLATE Czech_CI_AS NULL
)

GO
ALTER TABLE [dbo].[Review] ADD  CONSTRAINT [PK__Review__0000000000000094] PRIMARY KEY 
(
	[ReviewID]
)
GO
CREATE TABLE [dbo].[Setting](
	[Key] [nvarchar](4000) COLLATE Czech_CI_AS NOT NULL,
	[Value] [nvarchar](4000) COLLATE Czech_CI_AS NOT NULL,
	[Type] [nvarchar](4000) COLLATE Czech_CI_AS NOT NULL,
	[Description] [nvarchar](4000) COLLATE Czech_CI_AS NULL
)

GO
ALTER TABLE [dbo].[Setting] ADD  CONSTRAINT [PK__Setting__0000000000000018] PRIMARY KEY 
(
	[Key]
)
GO
ALTER TABLE [dbo].[BasicCategory]  ADD  CONSTRAINT [BasicCategory_Parent] FOREIGN KEY([Parent_CategoryID])
REFERENCES [BasicCategory] ([CategoryID])
GO
ALTER TABLE [dbo].[BasicCategoryBasicProduct]  ADD  CONSTRAINT [BasicCategory_Products_Source] FOREIGN KEY([BasicCategory_CategoryID])
REFERENCES [BasicCategory] ([CategoryID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BasicCategoryBasicProduct]  ADD  CONSTRAINT [BasicCategory_Products_Target] FOREIGN KEY([BasicProduct_ProductID])
REFERENCES [BasicProduct] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CartItem]  ADD  CONSTRAINT [CartItem_Product] FOREIGN KEY([ProductID])
REFERENCES [BasicProduct] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetail]  ADD  CONSTRAINT [OrderDetail_Order] FOREIGN KEY([OrderID])
REFERENCES [Order] ([OrderID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetail]  ADD  CONSTRAINT [OrderDetail_Product] FOREIGN KEY([ProductID])
REFERENCES [BasicProduct] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Review]  ADD  CONSTRAINT [Review_Customer] FOREIGN KEY([CustomerUserName])
REFERENCES [Customer] ([Username])
GO
ALTER TABLE [dbo].[Review]  ADD  CONSTRAINT [Review_Product] FOREIGN KEY([ProductID])
REFERENCES [BasicProduct] ([ProductID])
ON DELETE CASCADE
GO
