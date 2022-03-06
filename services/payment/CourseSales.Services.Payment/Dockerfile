FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as sdk
WORKDIR /app
EXPOSE 80
EXPOSE 443
# *.csproj -> ismi yazmama gerek yok çünkü bir tane var.
#{.csproj bulunduğu yer} {image içerisinde kopyalamak istediğim yer}
COPY shared/CourseSales.Shared/*.csproj shared/CourseSales.Shared/
COPY services/payment/CourseSales.Services.Payment/*.csproj services/payment/CourseSales.Services.Payment/
RUN dotnet restore services/payment/CourseSales.Services.Payment/*.csproj
# . . diyerek app içerisindekileri app içerisinde kopyala diyoruz. yukarıdaki kopyaladıklarım haricindeki herşeyi kopyala
COPY . . 
RUN dotnet publish services/payment/CourseSales.Services.Payment/*.csproj -c Release -o PaymentPublishFolder
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runtime
WORKDIR /app
#sdk image'inden kopyala. sdk image içerisindeki /app/CatalogPublishFolder klasörünü, runtime içerisindeki app'in içine kopyala
COPY --from=sdk /app/PaymentPublishFolder .
ENTRYPOINT [ "dotnet", "CourseSales.Services.Payment.dll" ]