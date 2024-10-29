dotnet build ./src/MealBot.Database `
    --configuration Release

sqlpackage /Action:publish `
    /SourceFile:"./src/MealBot.Database/bin/Release/MealBot.Database.dacpac" `
    /TargetDatabaseName:MealBot `
    /TargetServerName:"localhost,3433" `
    /TargetUser:"sa" `
    /TargetPassword:"Pass@word1" `
    /TargetTrustServerCertificate:True