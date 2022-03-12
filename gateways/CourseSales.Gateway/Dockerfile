FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as sdk
WORKDIR /app
EXPOSE 80

COPY gateways/CourseSales.Gateway/*.csproj gateways/CourseSales.Gateway/
RUN dotnet restore gateways/CourseSales.Gateway/*.csproj
# . . diyerek app içerisindekileri app içerisinde kopyala diyoruz. yukarıdaki kopyaladıklarım haricindeki herşeyi kopyala
COPY . . 
RUN dotnet publish gateways/CourseSales.Gateway/*.csproj -c Release -o GatewayPublishFolder
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runtime
WORKDIR /app
#sdk image'inden kopyala. sdk image içerisindeki /app/CatalogPublishFolder klasörünü, runtime içerisindeki app'in içine kopyala
COPY --from=sdk /app/GatewayPublishFolder .
ENTRYPOINT [ "dotnet", "CourseSales.Gateway.dll" ]