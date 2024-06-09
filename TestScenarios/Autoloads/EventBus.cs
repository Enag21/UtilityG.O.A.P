using Godot;
using UGOAP.CommonUtils;

namespace UGOAP.TestScenarios.Autoloads;

public partial class EventBus : Singleton<EventBus>
{
    [Signal] public delegate void RainStartedEventHandler();
    [Signal] public delegate void RainEndedEventHandler();

    public static void EmitRainStarted() => Instance.EmitSignal(SignalName.RainStarted);
    public static void EmitRainEnded() => Instance.EmitSignal(SignalName.RainEnded);
}