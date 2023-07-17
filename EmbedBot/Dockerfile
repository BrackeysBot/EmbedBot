FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EmbedBot/EmbedBot.csproj", "EmbedBot/"]
RUN dotnet restore "EmbedBot/EmbedBot.csproj"
COPY . .
WORKDIR "/src/EmbedBot"
RUN dotnet build "EmbedBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmbedBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmbedBot.dll"]
