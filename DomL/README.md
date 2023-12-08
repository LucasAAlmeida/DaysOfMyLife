Download SourceTree,
clona DomL project do GitHub
Download and install Visual Studio Community
Download and install SQL Server Express
Download and install SQL Server Management Studio (SSMS)

IN SQL SERVER MANAGEMENT STUDIO (SSMS)
Connect to:
Server Type		Database Engine
Server Name		".\SQLEXPRESS"
Authentication	Windows authentication

Create Database with name DomLv01 (or whatever is in the project's "App.config" file

==========================================

IN VISUAL STUDIO COMMUNITY
Open the .sln file in Visua Studio to load up the project
(in the Solution Explorere, Right click the project (not the solution) and change the type to 'Console Application' if you must)

View -> Other Windows -> Package Manager Console

"Update-Database" -> this will apply the initial migration in the project, the one that creates all tables in the database

Entity Framework Commands to remember

Add_Migration
Add-Migration [-Name] <String> [-OutputDir <String>] [-Context <String>] [-Project <String>] [-StartupProject <String>] [-Environment <String>] [<CommonParameters>]

Remove-Migration
Remove-Migration [-Force] [-Context <String>] [-Environment <String>] [-Project <String>] [-StartupProject <String>] [<CommonParameters>]

Update-Database
Update-Database [[-Migration] <String>] [-Context <String>] [-Environment <String>] [-Project <String>] [-StartupProject <String>] [<CommonParameters>]
