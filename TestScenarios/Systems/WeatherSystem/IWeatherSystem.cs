namespace UGOAP.TestScenarios.Systems.WeatherSystem;

public interface IWeatherSystem
{
    void ChangeWeather(WeatherType type);
}

public enum WeatherType
{
    Normal,
    Rain,
}