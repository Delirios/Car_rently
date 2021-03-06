USE [master]
GO
/****** Object:  Database [car_rently]    Script Date: 24.04.2020 21:00:32 ******/
CREATE DATABASE [car_rently]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'car_rently', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\car_rently.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'car_rently_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\car_rently_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [car_rently] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [car_rently].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [car_rently] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [car_rently] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [car_rently] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [car_rently] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [car_rently] SET ARITHABORT OFF 
GO
ALTER DATABASE [car_rently] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [car_rently] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [car_rently] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [car_rently] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [car_rently] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [car_rently] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [car_rently] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [car_rently] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [car_rently] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [car_rently] SET  DISABLE_BROKER 
GO
ALTER DATABASE [car_rently] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [car_rently] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [car_rently] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [car_rently] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [car_rently] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [car_rently] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [car_rently] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [car_rently] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [car_rently] SET  MULTI_USER 
GO
ALTER DATABASE [car_rently] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [car_rently] SET DB_CHAINING OFF 
GO
ALTER DATABASE [car_rently] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [car_rently] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [car_rently] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [car_rently] SET QUERY_STORE = OFF
GO
USE [car_rently]
GO
/****** Object:  Table [dbo].[cars]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cars](
	[Id_car] [int] IDENTITY(1,1) NOT NULL,
	[Id_type] [int] NULL,
	[car_model] [varchar](30) NULL,
	[year] [int] NULL,
	[cost] [int] NULL,
	[price] [int] NULL,
	[picture] [varbinary](max) NULL,
	[Id_brand] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_car] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[type_of_car]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[type_of_car](
	[Id_type] [int] IDENTITY(1,1) NOT NULL,
	[type_name] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rent]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rent](
	[Id_rent] [int] IDENTITY(1,1) NOT NULL,
	[Id_client] [int] NULL,
	[Id_car] [int] NULL,
	[Id_discount] [int] NULL,
	[lease_date] [date] NULL,
	[return_date] [date] NULL,
	[rental_days] [int] NULL,
	[total_amount] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_rent] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rent_penalty]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rent_penalty](
	[Id_rent_penalty] [int] IDENTITY(1,1) NOT NULL,
	[Id_penalty] [int] NULL,
	[Id_rent] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_rent_penalty] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cars_brand]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cars_brand](
	[Id_brand] [int] IDENTITY(1,1) NOT NULL,
	[brand_name] [varchar](30) NULL,
 CONSTRAINT [PK_tbl_name_ID] PRIMARY KEY CLUSTERED 
(
	[Id_brand] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[unavailable_cars_view]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[unavailable_cars_view] AS
SELECT cars.Id_car, type_name,brand_name,car_model,year,cost,price,picture from cars join cars_brand  on cars.Id_brand= cars_brand.Id_brand join type_of_car on cars.Id_type = type_of_car.Id_type left outer join rent on cars.Id_car = rent.Id_car left outer join rent_penalty on rent.Id_rent = rent_penalty.Id_rent where rent_penalty.Id_rent is null and rent.Id_rent is not null;
GO
/****** Object:  View [dbo].[available_cars_view]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[available_cars_view] AS
SELECT cars.Id_car, type_of_car.type_name,cars_brand.brand_name,cars.car_model,cars.year,cars.cost,cars.price,cars.picture from cars join cars_brand  on cars.Id_brand= cars_brand.Id_brand join type_of_car on cars.Id_type = type_of_car.Id_type left outer join unavailable_cars_view on cars.Id_car = unavailable_cars_view.Id_car where unavailable_cars_view.Id_car is null;
GO
/****** Object:  Table [dbo].[client]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[client](
	[Id_client] [int] IDENTITY(1,1) NOT NULL,
	[First_name] [varchar](50) NULL,
	[Last_name] [varchar](50) NULL,
	[Patronymic] [varchar](50) NULL,
	[E_mail] [varchar](50) NULL,
	[Phone] [varchar](20) NULL,
	[Password] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_client] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[discounts]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[discounts](
	[Id_discount] [int] IDENTITY(1,1) NOT NULL,
	[discount_name] [varchar](30) NULL,
	[discount_percent] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_discount] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[penalties]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[penalties](
	[Id_penalty] [int] IDENTITY(1,1) NOT NULL,
	[penalty_name] [varchar](30) NULL,
	[amount_penalty] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_penalty] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[cars_brand] ON 

INSERT [dbo].[cars_brand] ([Id_brand], [brand_name]) VALUES (14, N'FORD')
INSERT [dbo].[cars_brand] ([Id_brand], [brand_name]) VALUES (15, N'LEXUS ')
INSERT [dbo].[cars_brand] ([Id_brand], [brand_name]) VALUES (16, N'KIA')
INSERT [dbo].[cars_brand] ([Id_brand], [brand_name]) VALUES (17, N'DAEWOO')
INSERT [dbo].[cars_brand] ([Id_brand], [brand_name]) VALUES (18, N'BMW')
INSERT [dbo].[cars_brand] ([Id_brand], [brand_name]) VALUES (19, N'ACURA')
INSERT [dbo].[cars_brand] ([Id_brand], [brand_name]) VALUES (20, N'OPEL')
SET IDENTITY_INSERT [dbo].[cars_brand] OFF
SET IDENTITY_INSERT [dbo].[discounts] ON 

INSERT [dbo].[discounts] ([Id_discount], [discount_name], [discount_percent]) VALUES (4, N'Постійний клієнт', 15)
SET IDENTITY_INSERT [dbo].[discounts] OFF
SET IDENTITY_INSERT [dbo].[penalties] ON 

INSERT [dbo].[penalties] ([Id_penalty], [penalty_name], [amount_penalty]) VALUES (5, N'Невчасно', 20)
INSERT [dbo].[penalties] ([Id_penalty], [penalty_name], [amount_penalty]) VALUES (6, N'Неохайне авто', 30)
SET IDENTITY_INSERT [dbo].[penalties] OFF
SET IDENTITY_INSERT [dbo].[type_of_car] ON 

INSERT [dbo].[type_of_car] ([Id_type], [type_name]) VALUES (1, N'Седан')
INSERT [dbo].[type_of_car] ([Id_type], [type_name]) VALUES (2, N'Універсал')
INSERT [dbo].[type_of_car] ([Id_type], [type_name]) VALUES (3, N'Ліфтбек')
INSERT [dbo].[type_of_car] ([Id_type], [type_name]) VALUES (4, N'Хетчбек')
INSERT [dbo].[type_of_car] ([Id_type], [type_name]) VALUES (5, N'Кросовер')
INSERT [dbo].[type_of_car] ([Id_type], [type_name]) VALUES (6, N'Пікап')
INSERT [dbo].[type_of_car] ([Id_type], [type_name]) VALUES (7, N'Мінівен')
SET IDENTITY_INSERT [dbo].[type_of_car] OFF
ALTER TABLE [dbo].[cars]  WITH CHECK ADD  CONSTRAINT [fk_Id_brand] FOREIGN KEY([Id_brand])
REFERENCES [dbo].[cars_brand] ([Id_brand])
GO
ALTER TABLE [dbo].[cars] CHECK CONSTRAINT [fk_Id_brand]
GO
ALTER TABLE [dbo].[cars]  WITH CHECK ADD  CONSTRAINT [fk_Id_type] FOREIGN KEY([Id_type])
REFERENCES [dbo].[type_of_car] ([Id_type])
GO
ALTER TABLE [dbo].[cars] CHECK CONSTRAINT [fk_Id_type]
GO
ALTER TABLE [dbo].[rent]  WITH CHECK ADD  CONSTRAINT [FK_Id_car] FOREIGN KEY([Id_car])
REFERENCES [dbo].[cars] ([Id_car])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[rent] CHECK CONSTRAINT [FK_Id_car]
GO
ALTER TABLE [dbo].[rent]  WITH CHECK ADD  CONSTRAINT [FK_Id_client] FOREIGN KEY([Id_client])
REFERENCES [dbo].[client] ([Id_client])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[rent] CHECK CONSTRAINT [FK_Id_client]
GO
ALTER TABLE [dbo].[rent]  WITH CHECK ADD  CONSTRAINT [FK_Id_discount] FOREIGN KEY([Id_discount])
REFERENCES [dbo].[discounts] ([Id_discount])
GO
ALTER TABLE [dbo].[rent] CHECK CONSTRAINT [FK_Id_discount]
GO
ALTER TABLE [dbo].[rent_penalty]  WITH CHECK ADD  CONSTRAINT [FK_Id_penalty] FOREIGN KEY([Id_penalty])
REFERENCES [dbo].[penalties] ([Id_penalty])
GO
ALTER TABLE [dbo].[rent_penalty] CHECK CONSTRAINT [FK_Id_penalty]
GO
ALTER TABLE [dbo].[rent_penalty]  WITH CHECK ADD  CONSTRAINT [FK_Id_rent] FOREIGN KEY([Id_rent])
REFERENCES [dbo].[rent] ([Id_rent])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[rent_penalty] CHECK CONSTRAINT [FK_Id_rent]
GO
/****** Object:  StoredProcedure [dbo].[close_order]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[close_order] 
@E_mail varchar(50)
AS
BEGIN
SELECT rent.Id_rent, brand_name,picture,car_model,lease_date,return_date,total_amount FROM rent JOIN client ON rent.Id_client = client.Id_client JOIN cars ON rent.Id_car = cars.Id_car lEFT OUTER JOIN rent_penalty ON rent.Id_rent = rent_penalty.Id_rent JOIN cars_brand ON cars.Id_brand = cars_brand.Id_brand WHERE E_mail = @E_mail AND rent_penalty.Id_rent IS NULL
END;
GO
/****** Object:  StoredProcedure [dbo].[current_cars]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[current_cars] as

BEGIN
SELECT Id_car, type_name,brand_name,car_model, year, cost,price, picture FROM cars JOIN type_of_car ON cars.Id_type = type_of_car.Id_type JOIN cars_brand  ON cars.Id_brand = cars_brand.Id_brand
end
GO
/****** Object:  StoredProcedure [dbo].[current_orders]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[current_orders] as

BEGIN
SELECT rent.Id_rent, E_mail, brand_name, car_model,lease_date, return_date, rental_days, total_amount FROM cars JOIN rent ON cars.Id_car = rent.Id_car JOIN client  ON rent.Id_client = client.Id_client JOIN cars_brand  ON cars.Id_brand = cars_brand.Id_brand lEFT OUTER JOIN rent_penalty ON rent.Id_rent = rent_penalty.Id_rent WHERE rent_penalty.Id_rent IS NULL
END
GO
/****** Object:  StoredProcedure [dbo].[rent_history]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[rent_history] as

BEGIN
SELECT DISTINCT rent.Id_rent, E_mail, brand_name, car_model,lease_date, return_date, rental_days, total_amount FROM cars JOIN rent ON cars.Id_car = rent.Id_car JOIN client  ON rent.Id_client = client.Id_client JOIN cars_brand  ON cars.Id_brand = cars_brand.Id_brand lEFT OUTER JOIN rent_penalty ON rent.Id_rent = rent_penalty.Id_rent WHERE rent_penalty.Id_rent IS NOT NULL
END
GO
/****** Object:  StoredProcedure [dbo].[show_order]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[show_order] 
@Id_rent int
AS
BEGIN
select Id_rent,Last_name,First_name,  Patronymic,brand_name,car_model,price,total_amount, lease_date,return_date,rental_days from rent join client on rent.Id_client = client.Id_client join cars on cars.Id_car = rent.Id_car join cars_brand on cars.Id_brand = cars_brand.Id_brand where @Id_rent = Id_rent
END;
GO
/****** Object:  StoredProcedure [dbo].[show_order_to_customer]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[show_order_to_customer]
@Id_client int
as
begin
  SELECT TOP 1 brand_name, car_model,lease_date, return_date, total_amount,picture FROM rent JOIN cars ON rent.Id_car = cars.Id_car JOIN cars_brand  ON cars.Id_brand = cars_brand.Id_brand WHERE Id_client = @Id_client ORDER BY Id_rent DESc
END;
GO
/****** Object:  StoredProcedure [dbo].[update_cars]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[update_cars]
@Id_car int,
@cost int,
@price int
as
begin
  update cars set cost=@cost, price=@price where Id_car=@Id_car;
end
GO
/****** Object:  StoredProcedure [dbo].[update_client]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[update_client]
@Id_client int,
@First_name varchar(50),
@Last_name varchar(50),
@Patronymic varchar(50),
@E_mail varchar(50),
@Phone int
as
begin
  update client set First_name=@First_name, Last_name=@Last_name, Patronymic=@Patronymic,E_mail=@E_mail, Phone=@Phone where Id_client=@Id_client;
end
GO
/****** Object:  StoredProcedure [dbo].[update_discounts]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[update_discounts]
@Id_discount int,
@discount_name varchar(30),
@discount_percent int
as
begin
  update discounts set discount_name=@discount_name, discount_percent=@discount_percent where Id_discount=@Id_discount;
end
GO
/****** Object:  StoredProcedure [dbo].[update_order]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[update_order] 
@Id_rent int,
@return_date date,
@rental_days int,
@total_amount int

as
begin
  update rent set return_date=@return_date, rental_days=@rental_days,total_amount=@total_amount where Id_rent=@Id_rent;
END;
GO
/****** Object:  StoredProcedure [dbo].[update_penalties]    Script Date: 24.04.2020 21:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[update_penalties]
@Id_penalty int,
@penalty_name varchar(30) ,
@amount_penalty int
as
begin
  update penalties  set penalty_name=@penalty_name, amount_penalty=@amount_penalty where Id_penalty=@Id_penalty;
end
GO
USE [master]
GO
ALTER DATABASE [car_rently] SET  READ_WRITE 
GO
