FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as sdk
WORKDIR /app
EXPOSE 80

# *.csproj -> ismi yazmama gerek yok çünkü bir tane var.
#{.csproj bulunduğu yer} {image içerisinde kopyalamak istediğim yer}
COPY shared/CourseSales.Shared/*.csproj shared/CourseSales.Shared/
COPY services/catalog/CourseSales.Services.Catalog/*.csproj services/catalog/CourseSales.Services.Catalog/
RUN dotnet restore services/catalog/CourseSales.Services.Catalog/*.csproj
# . . diyerek app içerisindekileri app içerisinde kopyala diyoruz. yukarıdaki kopyaladıklarım haricindeki herşeyi kopyala
COPY . . 
RUN dotnet publish services/catalog/CourseSales.Services.Catalog/*.csproj -c Release -o CatalogPublishFolder
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runtime
WORKDIR /app
#sdk image'inden kopyala. sdk image içerisindeki /app/CatalogPublishFolder klasörünü, runtime içerisindeki app'in içine kopyala
COPY --from=sdk /app/CatalogPublishFolder .
ENTRYPOINT [ "dotnet", "CourseSales.Services.Catalog.dll" ]