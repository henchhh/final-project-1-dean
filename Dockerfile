#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 48719
EXPOSE 27017
EXPOSE 5000
EXPOSE 5001
EXPOSE 44319

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY . .

#ENV ASPNETCORE_ENVIRONMENT=Development

#COPY ["ProjectApp/ProjectApp.csproj", "ProjectApp/"]
#RUN dotnet restore "ProjectApp/ProjectApp.csproj"
#COPY . .
#WORKDIR "/src/ProjectApp"
#RUN dotnet build "ProjectApp.csproj" -c Release -o /app/build

RUN dotnet restore 
RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish "ProjectApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
#COPY --from=publish /app/publish .
COPY --from=publish /app/ .
#ENTRYPOINT ["dotnet", "ProjectApp.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ProjectApp.dll