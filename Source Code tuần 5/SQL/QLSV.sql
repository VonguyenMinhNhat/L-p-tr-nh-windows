CREATE DATABASE QuanLySinhVien;
GO
USE QuanLySinhVien;
GO

	CREATE TABLE faculty (
		FacultyID INT PRIMARY KEY,        -- FacultyID as the primary key
		FacultyName VARCHAR(100)          -- FacultyName as a string with a max length of 100
	);

	-- Creating the student table
	CREATE TABLE student (
		StudentID INT PRIMARY KEY,        -- StudentID as the primary key
		FullName VARCHAR(100),            -- FullName as a string with a max length of 100
		AverageScore DECIMAL(5, 2),       -- AverageScore as a decimal with up to 2 decimal places
		FacultyID INT,                    -- Foreign key referencing FacultyID from faculty table
		FOREIGN KEY (FacultyID) REFERENCES faculty(FacultyID)
	);