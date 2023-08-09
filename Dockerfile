FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY **/*.sln .

COPY . ./
RUN cd FDT.Simulation.API && dotnet restore
RUN cd FDT.Simulation.API && dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /out .
ENTRYPOINT ["dotnet", "FDT.Simulation.API.dll"]