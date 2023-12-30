using Godot;

namespace monsterland.simulation.general;

public partial class GameLevel : Node {
  [Export] public GameMode mode;

  public override void _Ready() {
    base._Ready();
    Global.instance?.setModeIfUnset(mode);

    if (Global.instance?.mode?.hud != null) {
      var hud = mode.hud.Instantiate();
      GetTree().Root.CallDeferred("add_child", hud);
    }
  }
}
