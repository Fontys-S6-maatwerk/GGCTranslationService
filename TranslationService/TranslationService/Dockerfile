FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TranslationService/TranslationService.csproj", "TranslationService/"]
RUN dotnet restore "TranslationService/TranslationService.csproj"
COPY . .
WORKDIR "/src/TranslationService"
RUN dotnet build "TranslationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TranslationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TranslationService.dll"]
