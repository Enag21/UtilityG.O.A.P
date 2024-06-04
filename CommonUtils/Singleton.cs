using Godot;

namespace UGOAP.CommonUtils;

public partial class Singleton<T> : Node where T : Node, new()
{
    private static T _instance;
    public static T Instance { get { return _instance ??= new T(); } }

    public override void _Ready()
    {
        _instance = this as T;
    }
    public override void _EnterTree()
    {
        base._EnterTree();
        if (_instance != null)
        {
            QueueFree();
        }
        _instance = this as T;
    }
}