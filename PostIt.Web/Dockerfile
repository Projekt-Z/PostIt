FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PostIt.Web/PostIt.Web.csproj", "PostIt.Web/"]
RUN dotnet restore "PostIt.Web/PostIt.Web.csproj"
COPY . .
WORKDIR "/src/PostIt.Web"
RUN dotnet build "PostIt.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PostIt.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PostIt.Web.dll"]
