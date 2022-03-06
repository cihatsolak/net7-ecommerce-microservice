FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as sdk
WORKDIR /app
EXPOSE 80
EXPOSE 443
# *.csproj -> ismi yazmama gerek yok çünkü bir tane var.
#{.csproj bulunduğu yer} {image içerisinde kopyalamak istediğim yer}
COPY shared/CourseSales.Shared/*.csproj shared/CourseSales.Shared/
COPY services/order/CourseSales.Services.Order.API/*.csproj services/order/CourseSales.Services.Order.API/
COPY services/order/CourseSales.Services.Order.Application/*.csproj services/order/CourseSales.Services.Order.Application/
COPY services/order/CourseSales.Services.Order.Domain/*.csproj services/order/CourseSales.Services.Order.Domain/
COPY services/order/CourseSales.Services.Order.Infrastructure/*.csproj services/order/CourseSales.Services.Order.Infrastructure/
RUN dotnet restore services/order/CourseSales.Services.Order.API/*.csproj
# . . diyerek app içerisindekileri app içerisinde kopyala diyoruz. yukarıdaki kopyaladıklarım haricindeki herşeyi kopyala
COPY . . 
RUN dotnet publish services/order/CourseSales.Services.Order.API/*.csproj -c Release -o OrderPublishFolder
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runtime
WORKDIR /app
#sdk image'inden kopyala. sdk image içerisindeki /app/CatalogPublishFolder klasörünü, runtime içerisindeki app'in içine kopyala
COPY --from=sdk /app/OrderPublishFolder .
ENTRYPOINT [ "dotnet", "CourseSales.Services.Order.API.dll" ]