FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["CrowdLending/CrowdLending.csproj", "CrowdLending/"]
RUN dotnet restore "CrowdLending/CrowdLending.csproj"
COPY . .
WORKDIR "/src/CrowdLending"
RUN dotnet build "CrowdLending.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CrowdLending.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CrowdLending.dll"]