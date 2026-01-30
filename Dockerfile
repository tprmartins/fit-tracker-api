FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto e restaura as dependências
COPY ["src/FitTracker.Api/FitTracker.Api.csproj", "src/FitTracker.Api/"]
COPY ["src/FitTracker.Application/FitTracker.Application.csproj", "src/FitTracker.Application/"]
COPY ["src/FitTracker.Core/FitTracker.Core.csproj", "src/FitTracker.Core/"]
COPY ["src/FitTracker.Domain/FitTracker.Domain.csproj", "src/FitTracker.Domain/"]
COPY ["src/FitTracker.Infra/FitTracker.Infra.csproj", "src/FitTracker.Infra/"]

RUN dotnet restore "src/FitTracker.Api/FitTracker.Api.csproj"

# Copia o restante dos arquivos e compila
COPY . .
WORKDIR "/src/src/FitTracker.Api"
RUN dotnet build "FitTracker.Api.csproj" -c Release -o /app/build

# Publica a aplicação
FROM build AS publish
RUN dotnet publish "FitTracker.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagem final de execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
# Porta padrão do Cloud Run
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FitTracker.Api.dll"]