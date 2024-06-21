namespace UGOAP.BehaviourSystem.Actions;

public partial class InRangeAction : ActionBase
{
    public override IActionState ActionState { get; set; }
    public override IActionLogic ActionLogic { get; set; }

    public override void Start()
    {
        if (!InRange())
        {
            ActionState.User.NavigationComponent.SetDestination(ActionState.Provider.Location, 0.3f);
            ActionState.User.NavigationComponent.NavigationFinished += () => ActionLogic.Start();
        }
    }
    public override void Stop() => ActionLogic.Stop();
    public override void Update(float delta)
    {
        if (!InRange()) return;
        ActionLogic.Update(delta);
    }

    private bool InRange() => ActionState.User.Location.DistanceTo(ActionState.Provider.Location) < 0.3f;
}