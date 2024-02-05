create database DBPRUEBATECNICABACKEND
use DBPRUEBATECNICABACKEND

CREATE DATABASE DBPRUEBATECNICABACKEND;
GO

USE DBPRUEBATECNICABACKEND;
GO

CREATE TABLE AUTO (
    ID int IDENTITY(1,1) PRIMARY KEY,
    Patente varchar(7) UNIQUE
);

CREATE TABLE ESTANCIA (
    ID int IDENTITY(1,1) PRIMARY KEY,
    HsEntrada datetime,
    HsSalida datetime,
    Costo float,
    IdAuto varchar(7),
    CONSTRAINT FK_IdAuto FOREIGN KEY (IdAuto) REFERENCES AUTO(Patente)
);

CREATE TABLE TIPO (
    IDTipo int IDENTITY(1,1) PRIMARY KEY,
    Descripcion varchar(20)
);

CREATE TABLE USUARIOS (
    ID int IDENTITY(1,1) PRIMARY KEY,
    Acumulado float,
    IDTipo int,
    IdAuto varchar(7),
    CONSTRAINT FK_IDTipo FOREIGN KEY (IDTipo) REFERENCES TIPO(IDTipo),
    CONSTRAINT FK_AutoID FOREIGN KEY (IdAuto) REFERENCES AUTO(Patente)
);

