# Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copy user secrets to the container
COPY appsettings.json ./

# Set environment variables from user secrets
ENV IssueAudience=apiWithAuthBackend
ENV ConnectionString="Server=host.docker.internal,1433;Database=solarwatch;User Id=sa;Password=yourStrong(!)Password;MultipleActiveResultSets=true;TrustServerCertificate=True;"
ENV ServiceApiKey="1593b000aeea6d27a1247cb005f6103b"
ENV IssueSign=!SomethingSecret!

# Copy published output
COPY --from=build-env /app/out .

# Set up ASP.NET Core URL
ENV ASPNETCORE_URLS=http://+:80

# Run the application
ENTRYPOINT ["dotnet", "SolarWatchMvp.dll"]
