FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release --output /output

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
# COPY --from=build /bin/release/net8.0/publish .
COPY --from=build /output .

# Desactivar HSTS (Strict-Transport-Security)
# ENV ASPNETCORE_Kestrel__Certificates__Default__CertificatePath=/https/aspnetapp.pfx
# ENV ASPNETCORE_Kestrel__Certificates__Default__Password=password
# ENV ASPNETCORE_Kestrel__Certificates__Default__AllowedHosts=*

# Start the application
ENTRYPOINT ["dotnet", "BackendChallengeTechFullStackN5.dll"]
