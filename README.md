# OpenWeatherLib

[![Build Status](https://dev.azure.com/simonkrause91/OpenWeatherLib/_apis/build/status/Nplu5.OpenWeatherLib?branchName=master)](https://dev.azure.com/simonkrause91/OpenWeatherLib/_build/latest?definitionId=2&branchName=master)

## Getting Started

If you want to develop for the library and want to use the Console example application you first have to create a 'Secretes.cs' class.
```csharp
namespace ConsoleOpenWeatherLib
{
    internal class Secrets
    {
        internal static string OpenWeatherApiKey => "c0ef1de9b16207c5d7af3b282d6174c1";
    }
}
```