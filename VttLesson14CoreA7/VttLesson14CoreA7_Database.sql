-- =============================================================
-- BÀI THỰC HÀNH 07: TÌM HIỂU VỀ LAYOUT
-- SCRIPT TẠO DATABASE VÀ CÁC BẢNG THEO YÊU CẦU LAB 07
-- Sinh viên: VTT (K24CNT2 - ASP.NET Core MVC)
-- =============================================================

CREATE DATABASE [VttLesson14CoreA7Db];
GO

USE [VttLesson14CoreA7Db];
GO

-- 1. BẢNG CATEGORY
IF OBJECT_ID('dbo.Category', 'U') IS NOT NULL
    DROP TABLE dbo.Category;
GO

CREATE TABLE dbo.Category (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Status TINYINT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_Category_Name UNIQUE (Name)
);
GO

-- 2. BẢNG PRODUCT
IF OBJECT_ID('dbo.Product', 'U') IS NOT NULL
    DROP TABLE dbo.Product;
GO

CREATE TABLE dbo.Product (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Price FLOAT NOT NULL DEFAULT 0,
    CategoryId INT NOT NULL,
    Image VARCHAR(100) NULL,
    CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryId) REFERENCES dbo.Category(Id)
);
GO

-- 3. BẢNG BANNER / BLOG
IF OBJECT_ID('dbo.BannerBlog', 'U') IS NOT NULL
    DROP TABLE dbo.BannerBlog;
GO

CREATE TABLE dbo.BannerBlog (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(350) NOT NULL,
    Image VARCHAR(100) NULL,
    Type NVARCHAR(50) NOT NULL DEFAULT 'Banner',
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    IsActive BIT NOT NULL DEFAULT 1
);
GO

-- CHÈN DỮ LIỆU MẪU BAN ĐẦU
INSERT INTO dbo.Category (Name, Status, CreatedDate) VALUES
(N'Điện thoại & Smartphone', 1, GETDATE()),
(N'Laptop & Máy tính xách tay', 1, GETDATE()),
(N'Đồng hồ thông minh', 1, GETDATE()),
(N'Phụ kiện cao cấp', 1, GETDATE());

INSERT INTO dbo.Product (Name, Price, CategoryId, Image) VALUES
(N'iPhone 16 Pro Max 256GB', 34990000, 1, 'iphone16.jpg'),
(N'Samsung Galaxy S24 Ultra', 28990000, 1, 's24ultra.jpg'),
(N'MacBook Air M3 13 inch', 28490000, 2, 'macbookm3.jpg'),
(N'ASUS Zenbook 14 OLED', 26990000, 2, 'zenbook.jpg'),
(N'Apple Watch Series 10', 11490000, 3, 'applewatch10.jpg'),
(N'Tai nghe Sony WH-1000XM5', 6990000, 4, 'sonyxm5.jpg');

INSERT INTO dbo.BannerBlog (Title, Description, Image, Type, CreatedDate, IsActive) VALUES
(N'Đại tiệc công nghệ Chào năm học mới - Giảm tới 40%', N'Cơ hội sở hữu laptop, smartphone và phụ kiện chính hãng với ưu đãi tốt nhất.', 'banner1.jpg', 'Banner', GETDATE(), 1),
(N'Top 5 ultrabook mỏng nhẹ cho sinh viên CNTT', N'Đánh giá chi tiết 5 mẫu laptop văn phòng và lập trình với thời lượng pin ấn tượng.', 'blog1.jpg', 'Blog', GETDATE(), 1);
GO
