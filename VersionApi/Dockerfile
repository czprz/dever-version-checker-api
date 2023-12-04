FROM mcr.microsoft.com/dotnet/aspnet:8.0

USER app

EXPOSE 80
EXPOSE 443

WORKDIR /app

COPY /publish /app

ENTRYPOINT ["dotnet", "VersionApi.dll"]
