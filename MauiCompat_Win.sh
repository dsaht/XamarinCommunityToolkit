dotnet new mauilib -o ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat -n Xamarin.CommunityToolkit.MauiCompat
dotnet new mauilib -o ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat -n Xamarin.CommunityToolkit.Markup.MauiCompat

dotnet new sln -o ./src/CommunityToolkit/ -n Xamarin.CommunityToolkit.MauiCompat
dotnet sln ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat.sln add ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Xamarin.CommunityToolkit.MauiCompat.csproj

dotnet new sln -o ./src/Markup/ -n Xamarin.CommunityToolkit.Markup.MauiCompat
dotnet sln ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat.sln add ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/Xamarin.CommunityToolkit.Markup.MauiCompat.csproj

