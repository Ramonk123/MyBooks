# Use the official .NET SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all projects in the solution
COPY MyBooks/MyBooks.csproj MyBooks/
COPY Data/Data.csproj Data/
COPY PopularityService/PopularityService.csproj PopularityService/
COPY RatingService/RatingService.csproj RatingService/
COPY Seeder/Seeder.csproj Seeder/
# Restore dependencies for each project
RUN dotnet restore MyBooks/MyBooks.csproj
RUN dotnet restore Data/Data.csproj
RUN dotnet restore PopularityService/PopularityService.csproj
RUN dotnet restore RatingService/RatingService.csproj
RUN dotnet restore Seeder/Seeder.csproj

# Copy all source files into the container
COPY . .

# Build all projects
RUN dotnet build MyBooks/MyBooks.csproj -c Release -o /app/build
RUN dotnet build Data/Data.csproj -c Release -o /app/build
RUN dotnet build PopularityService/PopularityService.csproj -c Release -o /app/build
RUN dotnet build RatingService/RatingService.csproj -c Release -o /app/build
RUN dotnet build Seeder/Seeder.csproj -c Release -o /app/build

# Publish all projects
RUN dotnet publish MyBooks/MyBooks.csproj -c Release -o /app/publish
RUN dotnet publish Data/Data.csproj -c Release -o /app/publish
RUN dotnet publish PopularityService/PopularityService.csproj -c Release -o /app/publish
RUN dotnet publish RatingService/RatingService.csproj -c Release -o /app/publish
RUN dotnet publish Seeder/Seeder.csproj -c Release -o /app/publish

# Use the runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Copy all published projects to the container
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MyBooks.dll"]
