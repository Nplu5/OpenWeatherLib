namespace OpenWeather.Tests
{
    internal class TestData
    {
        // Created from actual data with: http://easyonlineconverter.com/converters/dot-net-string-escape.html
        // 30.07.2019 21:00
        // 31.07.2019 00:00
        internal static readonly string Forecast = "{   \"cod\":\"200\",   \"message\":0.0086,   \"cnt\":2,   \"list\":[      {         \"dt\":1564520400,         \"main\":{            \"temp\":17.78,            \"temp_min\":17.66,            \"temp_max\":17.78,            \"pressure\":1016.72,            \"sea_level\":1016.72,            \"grnd_level\":983.77,            \"humidity\":69,            \"temp_kf\":0.12         },         \"weather\":[            {               \"id\":800,               \"main\":\"Clear\",               \"description\":\"Klarer Himmel\",               \"icon\":\"01n\"            }         ],         \"clouds\":{            \"all\":9         },         \"wind\":{            \"speed\":3.39,            \"deg\":276.678         },         \"sys\":{            \"pod\":\"n\"         },         \"dt_txt\":\"2019-07-30 21:00:00\"      },      {         \"dt\":1564531200,         \"main\":{            \"temp\":16.55,            \"temp_min\":16.46,            \"temp_max\":16.55,            \"pressure\":1017.04,            \"sea_level\":1017.04,            \"grnd_level\":984.05,            \"humidity\":81,            \"temp_kf\":0.09         },         \"weather\":[            {               \"id\":500,               \"main\":\"Rain\",               \"description\":\"Leichter Regen\",               \"icon\":\"10n\"            }         ],         \"clouds\":{            \"all\":49         },         \"wind\":{            \"speed\":2.03,            \"deg\":260.798         },         \"rain\":{            \"3h\":0.688         },         \"sys\":{            \"pod\":\"n\"         },         \"dt_txt\":\"2019-07-31 00:00:00\"      }   ],   \"city\":{      \"id\":2825297,      \"name\":\"Stuttgart\",      \"coord\":{         \"lat\":48.7784,         \"lon\":9.18      },      \"country\":\"DE\",      \"population\":589793,      \"timezone\":7200   }}";

    }
}
