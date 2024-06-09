using Godot;
using UGOAP.TestScenarios.Systems.WeatherSystem;

namespace UGOAP.TestScenarios.Scripts;

public partial class RainButton : Button
{
    public override void _Ready()
    {
        Pressed += OnButtonPressed;
    }

    private void OnButtonPressed()
    {
        switch (Systems.WeatherSystem.WeatherComponent.Instance.CurrentWeather)
        {
            case WeatherType.Rain:
                Systems.WeatherSystem.WeatherComponent.Instance.ChangeWeather(WeatherType.Normal);
                break;
            case WeatherType.Normal:
                Systems.WeatherSystem.WeatherComponent.Instance.ChangeWeather(WeatherType.Rain);
                break;
        }
    }
}