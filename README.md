# 架構API Log 紀錄

## Github 套件發行
Release:
dotnet pack --configuration Release

Push:
dotnet nuget push ./Fklib.ApiRRLog/bin/Release/FkLibApiLog.0.0.7.nupkg --source "github"

