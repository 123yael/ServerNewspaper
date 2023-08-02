
--CREATE DATABASE NewspapersSystem

--USE NewspapersSystem

-- עיתונים שיצאו לאור
CREATE TABLE NewspapersPublished
(
	newspaper_id INT PRIMARY KEY IDENTITY(1, 1),
	publication_date DATE NOT NULL,
	pdfFile VARCHAR(50) NOT NULL,
	img VARCHAR(50) NOT NULL
)

-- קטגורית פרסומות
CREATE TABLE AdvertisementCategories
(
	category_id INT PRIMARY KEY IDENTITY(1, 1),
	category_name VARCHAR(50) NOT NULL
)

-- לקוחות
CREATE TABLE Customers
(
	cust_id INT PRIMARY KEY IDENTITY(1, 1),
	cust_firstName VARCHAR(50),
	cust_lastName VARCHAR(50),
	cust_email VARCHAR(50) NOT NULL,
	cust_phone VARCHAR(10) NOT NULL,
	cust_password VARCHAR(10) NOT NULL
)

-- הזמנות
CREATE TABLE Orders
(
	order_id INT PRIMARY KEY IDENTITY(1, 1),
	cust_id INT REFERENCES Customers NOT NULL,
	order_finalPrice DECIMAL NOT NULL,
	order_date DATE NOT NULL
)

-- גדלי פרסומות
CREATE TABLE AdSizes
(
	size_id INT PRIMARY KEY IDENTITY(1, 1),
	size_name VARCHAR(50) NOT NULL,
	size_height DECIMAL NOT NULL,
	size_width DECIMAL NOT NULL,
	size_price DECIMAL NOT NULL,
	size_img VARCHAR(50) NOT NULL,
)

-- מיקומי פרסומת
CREATE TABLE AdPlacements
(
	place_id INT PRIMARY KEY IDENTITY(1, 1),
	place_name VARCHAR(50) NOT NULL,
	place_price DECIMAL NOT NULL
)

-- תתי קטגוריות מודעות מילים
CREATE TABLE WordAdSubCategories
(
	wordCategory_id INT PRIMARY KEY IDENTITY(1, 1),
	wordCategory_name VARCHAR(50) NOT NULL,
)

-- פרטי הזמנה
CREATE TABLE OrderDetails
(
	details_id INT PRIMARY KEY IDENTITY(1, 1),
	order_id INT REFERENCES Orders,
	category_id INT REFERENCES AdvertisementCategories,
	wordCategory_id INT REFERENCES WordAdSubCategories,
	ad_content VARCHAR(255),
	ad_file VARCHAR(255),
	size_id INT REFERENCES AdSizes,
	place_id INT REFERENCES AdPlacements NOT NULL,
	ad_duration INT DEFAULT(1),
)

-- תאריכים לפרטי הזמנה
CREATE TABLE DatesForOrderDetails
(
	date_id INT PRIMARY KEY IDENTITY(1, 1),
	details_id INT REFERENCES OrderDetails,
	[date] DATE NOT NULL,
	date_status BIT DEFAULT(1),
)

-- עמודים בעיתון
CREATE TABLE PagesInNewspaper
(
	page_id INT PRIMARY KEY IDENTITY(1, 1),
	page_number INT NOT NULL,
	newspaper_id INT REFERENCES NewspapersPublished
)

-- שיבוץ מודעות בעמוד
CREATE TABLE PlacingAdsInPage
(
	placeInPage_id INT PRIMARY KEY IDENTITY(1, 1),
	page_id INT REFERENCES PagesInNewspaper,
	details_id INT REFERENCES OrderDetails
)

------------------------------------------------------

INSERT INTO NewspapersPublished
VALUES('2023-02-07', 'finalNewspaper2.pdf', 'newspaperimg.png')
GO

INSERT INTO NewspapersPublished
VALUES('2023-03-07', 'finalNewspaper2.pdf', 'newspaperimg.png')
GO

INSERT INTO NewspapersPublished
VALUES('2023-02-15', 'finalNewspaper2.pdf', 'newspaperimg.png')
GO

------------------------------------------------------

INSERT INTO AdvertisementCategories
VALUES('מזון ומשקאות')
GO

INSERT INTO AdvertisementCategories
VALUES('תיקונים ואינסטלציה')
GO

INSERT INTO AdvertisementCategories
VALUES('אולמות')
GO

INSERT INTO AdvertisementCategories
VALUES('ביגוד והנעלה')
GO

INSERT INTO AdvertisementCategories
VALUES('מוסדות צייבוריים')
GO

------------------------------------------------------

INSERT INTO Customers
VALUES('Yael', 'Burya', 'yaelshli762@gmail.com', '0533133762', '1111')
GO

INSERT INTO Customers
VALUES('Yael', 'Malkin', 'malkin.yaeli@gmail.com', '0583220353', '2222')
GO

INSERT INTO Customers
VALUES('Chani', 'Malkin', 'malkin.chany@gmail.com', '0543488561', '3333')
GO

------------------------------------------------------

INSERT INTO AdSizes
VALUES('Whole page', 8, 4, 1000, 'locationPictures/WholePage.png')
GO

INSERT INTO AdSizes
VALUES('Half page balanced', 4, 4, 500, 'locationPictures/HalfPageBalanced.png')
GO

INSERT INTO AdSizes
VALUES('Half a vertical page', 8, 2, 500, 'locationPictures/HalfAVerticalPage.png')
GO

INSERT INTO AdSizes
VALUES('Quarter page', 4, 2, 250, 'locationPictures/QuarterPage.png')
GO

INSERT INTO AdSizes
VALUES('Eighth of the page', 2, 2, 150, 'locationPictures/EighthOfThePage.png')
GO

INSERT INTO AdSizes
VALUES('One out of sixteen', 2, 1, 100, 'locationPictures/OneOutOfSixteen.png')
GO

------------------------------------------------------

INSERT INTO AdPlacements
VALUES('Cover Page', 100)
GO

INSERT INTO AdPlacements
VALUES('Back page', 200)
GO

INSERT INTO AdPlacements
VALUES('Normal location', 0)
GO

------------------------------------------------------

INSERT INTO WordAdSubCategories
VALUES('sale')
GO

INSERT INTO WordAdSubCategories
VALUES('renting')
GO

INSERT INTO WordAdSubCategories
VALUES('wanted')
GO

INSERT INTO WordAdSubCategories
VALUES('general')
GO
