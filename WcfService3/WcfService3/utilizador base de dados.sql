USE [master]
GO

/****** Object:  Login [IIS APPPOOL\.NET v4.5 Classic]    Script Date: 23/05/2017 20:03:14 ******/
CREATE LOGIN [IIS APPPOOL\.NET v4.5 Classic] FROM WINDOWS WITH DEFAULT_DATABASE=[snipetts], DEFAULT_LANGUAGE=[us_english]
GO

ALTER SERVER ROLE [sysadmin] ADD MEMBER [IIS APPPOOL\.NET v4.5 Classic]
GO


