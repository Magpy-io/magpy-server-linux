# dotnet build -c Release
dotnet publish --runtime linux-x64 --self-contained
vpk pack -u Magpy -v 1.0.0 -p ./bin/Release/net8.0/linux-x64/publish --packTitle Magpy --packAuthors "Magpy Team." -e Magpy