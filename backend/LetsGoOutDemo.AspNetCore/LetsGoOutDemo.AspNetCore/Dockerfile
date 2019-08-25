FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
# EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["LetsGoOutDemo.AspNetCore/LetsGoOutDemo.AspNetCore.csproj", "LetsGoOutDemo.AspNetCore/"]
RUN dotnet restore "LetsGoOutDemo.AspNetCore/LetsGoOutDemo.AspNetCore.csproj"
COPY . .
WORKDIR "/src/LetsGoOutDemo.AspNetCore"
RUN dotnet build "LetsGoOutDemo.AspNetCore.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "LetsGoOutDemo.AspNetCore.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "LetsGoOutDemo.AspNetCore.dll"]
