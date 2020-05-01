FROM netcore3.1-alpine
WORKDIR /app
COPY bin/publish /app

ARG ENVIRONMENT

ENV ASPNETCORE_ENVIRONMENT=${ENVIRONMENT} ASPNETCORE_URLS="http://*:80"

EXPOSE 80
ENTRYPOINT ["dotnet", "ConfigCenter.Admin.dll"]