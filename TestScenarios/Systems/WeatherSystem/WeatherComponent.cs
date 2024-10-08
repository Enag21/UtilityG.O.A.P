using Godot;
using UGOAP.CommonUtils;

namespace UGOAP.TestScenarios.Systems.WeatherSystem;

[GlobalClass]
public partial class WeatherComponent : Singleton<WeatherComponent>, IWeatherSystem
{
    [Export]
    GpuParticles2D _rain;

    public WeatherType CurrentWeather { get; private set; } = WeatherType.Normal;

    public void ChangeWeather(WeatherType type)
    {
        switch (type)
        {
            case WeatherType.Normal:
                CurrentWeather = WeatherType.Normal;
                Autoloads.EventBus.EmitRainEnded();
                _rain.Visible = false;
                break;
            case WeatherType.Rain:
                CurrentWeather = WeatherType.Rain;
                Autoloads.EventBus.EmitRainStarted();
                _rain.Visible = true;
                break;
        }
    }
}