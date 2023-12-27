using Godot;

namespace MonsterLand.client;

public partial class PlayerController : simulation.characters.CharacterController {
  public override void _PhysicsProcess(double delta) {
    direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
    base._PhysicsProcess(delta);
  }
}
