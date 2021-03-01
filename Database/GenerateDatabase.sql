CREATE DATABASE SharpsenStream
USE SharpsenStream

CREATE TABLE Users (
    UserId int IDENTITY(1,1) PRIMARY KEY,
    Username varchar(256),
    UserPassword varchar(256),
    Email varchar(256),
    AvatarFilePath varchar(256),
	Color int
);

CREATE TABLE Perms (
    UserId int,
    Streamer bit,
    Banned bit,
);

CREATE TABLE Stream (
    StreamId int IDENTITY(1,1) PRIMARY KEY,
	OwnerId int,
	StreamName varchar(256),
	Live bit,
	Title varchar(256),
	Description varchar(256),
	ChatId int,
	Token varchar(256),
	StartTime datetime
);

CREATE TABLE UserTokens (
    TokenId int IDENTITY(1,1) PRIMARY KEY,
	OwnerId int,
	Token varchar(256),
	Expiration datetime
);

CREATE TABLE StreamsHistory (
    StreamId int,
	OwnerId int,
	StreamName varchar(256),
	Title varchar(256),
	Description varchar(256),
	Token varchar(256),
	StartTime datetime,
	EndTime datetime
);