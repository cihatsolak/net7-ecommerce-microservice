FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as sdk
WORKDIR /app
EXPOSE 80

COPY shared/CourseSales.Shared/*.csproj shared/CourseSales.Shared/
COPY identityserver/CourseSales.IdentityServer/*.csproj identityserver/CourseSales.IdentityServer/
RUN dotnet restore identityserver/CourseSales.IdentityServer/*.csproj
COPY . .
RUN dotnet publish identityserver/CourseSales.IdentityServer/*.csproj -c Release -o IdentityPublishFolder
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=sdk /app/IdentityPublishFolder .
ENTRYPOINT [ "dotnet", "CourseSales.IdentityServer.dll" ]