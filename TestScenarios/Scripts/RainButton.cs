using Godot;

public partial class RainButton : Button
{
    public override void _Ready()
    {
        Pressed += OnButtonPressed;
    }

    private void OnButtonPressed()
    {
        switch (WeatherComponent.Instance.CurrentWeather)
        {
            case WeatherType.Rain:
                WeatherComponent.Instance.ChangeWeather(WeatherType.Normal);
                break;
            case WeatherType.Normal:
                WeatherComponent.Instance.ChangeWeather(WeatherType.Rain);
                break;
        }
    }
}
