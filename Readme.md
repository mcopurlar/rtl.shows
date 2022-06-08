# Rtl.Shows.Scraper

The solution contains two projects. One of them is a background service for scraping TvMaze api and the other one is web api to serve shows endpoint. SQLite is used for for database.

## Requirements

- Asp.Net 6 SDK

## Running application

### 1. SQLite database is created by following commands

`cd <root>\src\Rtl.Shows.Scraper`

`mkdir c:\Database`
`dotnet ef migrations add init`
`dotnet ef database update`

### 2. Run Scraper
`cd <root>\src\Rtl.Shows.Scraper`
`dotnet run` 

### 3. Run WebApi
`cd <root>\src\Rtl.Shows.WebApi`
`dotnet run` 


Scraping will take some time. Meantime you can run following command to see first pages are imported.

`curl --location --request GET 'http://localhost:5225/shows?pageNumber=0&pageSize=250'`