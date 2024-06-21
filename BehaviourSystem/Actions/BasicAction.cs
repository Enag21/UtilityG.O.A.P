namespace UGOAP.BehaviourSystem.Actions;

public partial class BasicAction : ActionBase
{
    public override IActionState ActionState { get; set; }
    public override IActionLogic ActionLogic { get; set; }

    public override void Start() => ActionLogic.Start();
    public override void Stop() => ActionLogic.Stop();
    public override void Update(float delta) => ActionLogic.Update(delta);
}
