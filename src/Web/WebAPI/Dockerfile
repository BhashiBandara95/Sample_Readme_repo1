﻿ARG ASPNET_VERSION="7.0.0-preview.1-alpine3.15"
ARG SDK_VERSION="7.0.100-preview.1-alpine3.15"
ARG BASE_ADRESS="mcr.microsoft.com/dotnet"

FROM $BASE_ADRESS/aspnet:$ASPNET_VERSION AS base
WORKDIR /app

EXPOSE 5000

RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM $BASE_ADRESS/sdk:$SDK_VERSION AS build

COPY ./global.json ./
COPY ./nuget.config ./
COPY ./Directory.Build.props ./

WORKDIR /src

COPY ./src/Web/WebAPI/*.csproj ./Web/WebAPI/
COPY ./src/Contracts/ECommerce/*.csproj ./Contracts/ECommerce/

RUN dotnet restore -v m ./Web/WebAPI

COPY ./src/Web/WebAPI/. ./Web/WebAPI/
COPY ./src/Contracts/ECommerce/. ./Contracts/ECommerce/

WORKDIR /src/Web/WebAPI
RUN dotnet build -c Release --no-restore -v m -o /app/build

FROM build AS publish
RUN dotnet publish -c Release --no-restore -v m -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]