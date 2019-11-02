FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine as build-ENV
COPY . ./app
WORKDIR /app/src
RUN dotnet publish -c Release -r linux-musl-x64 -o publish-folder
FROM mcr.microsoft.com/dotnet/core/runtime-deps:2.2-alpine as runtime
COPY --from=build-env /app/src/publish-folder ./
RUN apk add --update \
    icu-libs \
    && rm -rf /var/cache/apk/*
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS=https://+:443
EXPOSE 443/TCP
ENTRYPOINT ["./samples"]