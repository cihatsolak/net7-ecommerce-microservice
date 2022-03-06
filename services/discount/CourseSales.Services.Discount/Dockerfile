FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as sdk
WORKDIR /app
EXPOSE 80
EXPOSE 443
# *.csproj -> ismi yazmama gerek yok çünkü bir tane var.
#{.csproj bulunduğu yer} {image içerisinde kopyalamak istediğim yer}
COPY shared/CourseSales.Shared/*.csproj shared/CourseSales.Shared/
COPY services/discount/CourseSales.Services.Discount/*.csproj services/discount/CourseSales.Services.Discount/
RUN dotnet restore services/discount/CourseSales.Services.Discount/*.csproj
# . . diyerek app içerisindekileri app içerisinde kopyala diyoruz. yukarıdaki kopyaladıklarım haricindeki herşeyi kopyala
COPY . . 
RUN dotnet publish services/discount/CourseSales.Services.Discount/*.csproj -c Release -o DiscountPublishFolder
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runtime
WORKDIR /app
#sdk image'inden kopyala. sdk image içerisindeki /app/CatalogPublishFolder klasörünü, runtime içerisindeki app'in içine kopyala
COPY --from=sdk /app/DiscountPublishFolder .
ENTRYPOINT [ "dotnet", "CourseSales.Services.Discount.dll" ]