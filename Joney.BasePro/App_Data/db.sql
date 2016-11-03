----use master
--if exists(select * from sys.sysdatabases where name='JoneyDB')
--begin
--use master
----备份
----exec sp_addumpdevice 'disk','JoneyDB','D:\Demo\dbBackup\JoneyDB.dat'
----backup database pubs [JoneyDB] 
--drop database [JoneyDB]
--end
--go
--create database [JoneyDB]
--on
--(name=N'JoneyDB',filename=N'D:\Demo\JoneyDB.mdf',size=3mb,maxsize=unlimited,filegrowth=1)
--log on
--(name=N'JoneyDB',filename=N'D:\Demo\JoneyDB_log.ldf',size=3mb,maxsize=unlimited,filegrowth=1)
--go

use JoneyDB
go
if exists(select * from sys.sysobjects where name='UserInfo')
begin
drop table [dbo].[UserInfo]
end

use JoneyDB
go
begin
create table [dbo].[UserInfo]
(
[UID] int identity(1,1) not null,
[UserCode] nvarchar(50) primary key,
[Password] nvarchar(50),
[UserName] nvarchar(50),
[Sex] char(1),
[Birthday] datetime,
[Phone] nvarchar(20),
[Tel] nvarchar(20),
[Email] nvarchar(100),
[QQ] nvarchar(50),
[Wechat] nvarchar(50),
[Address] nvarchar(200),
[PostZip] nvarchar(20),
[Activity] bit,
[UserType] char,
[RegistDate] datetime,
[Remark] nvarchar(200)
)
end
begin
create table [dbo].[Roles]
(
[RID] int identity,
[RoleName] nvarchar(50),
[RoleDescription] nvarchar(100)
)
end
begin
create table [dbo].[Permission]
(
[PID] int identity,
[Description] nvarchar(50)
)
end

begin
create table [dbo].[Menus]
(
[MID] int identity,
[MenuName] varchar(50),
[ParentMID] int,
[OrderID] int,
[Url] nvarchar(200),
[PermissionID] int,
[ImgUrl] nvarchar(10),
[Activity] bit
)
end

begin
create table [dbo].[UserRoles]
(
[UID] int not null,
[RID] int not null,
constraint [PK_UserRoles] primary key clustered
(
[UID] asc,
[RID] asc
)
with(IGNORE_DUP_KEY=OFF) 
ON [PRIMARY]
)
on [PRIMARY]
end

BEGIN
CREATE TABLE [dbo].[RolePermissions]
(
[RID] int not null,
[PID] int not null
constraint [PK_RolePermissions] primary key clustered
(
[RID] asc,
[PID] asc
)
with(ignore_dup_key=off)
on [PRIMARY]
)
on [PRIMARY]
END

--
insert into [dbo].UserInfo values('A001','123456','Admin',1,GETDATE(),'0755-1234567','17876991530','junjieyu@msn.com','382232865','Joney125','深圳市高新区朗山路同方信息港',1,1,'超级管理员')
insert into [dbo].UserInfo values('A002','123456','Joney',1,GETDATE(),'0755-1234567','18697982406','junjieyu@outlook.com','382232865','Joney125','深圳市高新区朗山路同方信息港',1,1,'系统管理员')
--
insert into [dbo].[Roles] values('系统管理员','系统维护管理日程操作用户')
--
insert into [dbo].[Permission] values('匿名用户')
--
insert into [dbo].[Menus] values('主页',0,1,'Index',1,'')
--
insert into [dbo].[UserRoles] values(1,1)
--
insert into [dbo].[RolePermissions] values(1,1)