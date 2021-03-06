USE [DEV_DB_CodeCommon]
GO
/****** Object:  Table [dbo].[AppInfo]    Script Date: 2018/7/10 17:30:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AppInfo](
	[AppID] [int] NOT NULL,
	[AppName] [nvarchar](100) NOT NULL,
	[CreateID] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[IsDelete] [int] NOT NULL,
	[Domain] [nvarchar](300) NOT NULL,
 CONSTRAINT [PK_AppInfo] PRIMARY KEY CLUSTERED 
(
	[AppID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AppMessageService]    Script Date: 2018/7/10 17:30:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppMessageService](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MessageName] [nvarchar](50) NULL,
	[UserName] [nvarchar](50) NULL,
	[MobNo] [nvarchar](50) NULL,
	[AppKey] [nvarchar](50) NULL,
	[AppSercret] [nvarchar](50) NULL,
	[Remark] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[IsStop] [bit] NULL,
 CONSTRAINT [PK_AppMessageService] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppServiceToken]    Script Date: 2018/7/10 17:30:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppServiceToken](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AppKey] [nvarchar](50) NULL,
	[AppSercret] [nvarchar](50) NULL,
	[Title] [nvarchar](50) NULL,
	[Username] [nvarchar](50) NULL,
	[MobNo] [nvarchar](50) NULL,
	[Remark] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[IsStop] [bit] NULL,
 CONSTRAINT [PK_AppServiceToken] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[AppInfo] ([AppID], [AppName], [CreateID], [CreateTime], [IsDelete], [Domain]) VALUES (130000, N'权限管理平台', N'1', CAST(0x0000A4F000000000 AS DateTime), 0, N'carabaooauthdemo.esurl.cn')
INSERT [dbo].[AppInfo] ([AppID], [AppName], [CreateID], [CreateTime], [IsDelete], [Domain]) VALUES (130111, N'订单管理平台', N'1', CAST(0x0000A57500000000 AS DateTime), 0, N'cdemo.esurl.cn/Account/Login')
INSERT [dbo].[AppInfo] ([AppID], [AppName], [CreateID], [CreateTime], [IsDelete], [Domain]) VALUES (130213, N'串码管理平台', N'1', CAST(0x0000A83800000000 AS DateTime), 0, N'carabaocodedemo.esurl.cn')
SET IDENTITY_INSERT [dbo].[AppServiceToken] ON 

INSERT [dbo].[AppServiceToken] ([Id], [AppKey], [AppSercret], [Title], [Username], [MobNo], [Remark], [CreateTime], [IsStop]) VALUES (2, N'1234', N'5678', N'查询码接口', N'test', N'15021559432', N'一个月', CAST(0x0000A83900E8E216 AS DateTime), 0)
INSERT [dbo].[AppServiceToken] ([Id], [AppKey], [AppSercret], [Title], [Username], [MobNo], [Remark], [CreateTime], [IsStop]) VALUES (3, N'1111', N'2222', N'更新码', N'admin', N'123456', N'更新', CAST(0x0000A83C0110E394 AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[AppServiceToken] OFF
ALTER TABLE [dbo].[AppMessageService] ADD  CONSTRAINT [DF_AppMessageService_IsActive]  DEFAULT ((0)) FOR [IsStop]
GO
ALTER TABLE [dbo].[AppServiceToken] ADD  CONSTRAINT [DF_AppServiceToken_IsActive]  DEFAULT ((0)) FOR [IsStop]
GO
