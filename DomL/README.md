bug
Olha o GIFT, tah salvando  "From; To; Description" (o To devia ser o nome da pessoa)

Melhora
output de categorias: (WriteYearRecapFiles):
	as categorias que nao tem 2 datas (inicio/fim) podem só output 1 data ao inves de '----------	2021-10-08'
	output do ano deve ser só com datas e nome do filme/jogo/livro
	gift should merge the "From/To" field with the Person (instead of "From	(tab) Felipe" -> "From Felipe" (with space)
	travel should merge the "From/To" fields (instead of "Sao Paulo (tab) Brasilia" -> "Sao Paulo -> Brasilia"
	Doom coloca mais um tab entre data e description
Fun estatisticas
	soma de valores dos purchases
	quantos jogos por console
	quantos animes, séries de tv, etc
	Soma de gasto por loja




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
