using Godot;

namespace monsterland.simulation.general;

public partial class GameLevel : Node {
  [Export] public GameMode mode;

  public override void _Ready() {
    base._Ready();
    
    if (mode?.hud != null) {
      var hud = mode.hud.Instantiate();
      GetTree().Root.CallDeferred("add_child", hud);
    }
  }
}
