using Godot;

namespace MonsterLand;

public partial class PlayerController : CharacterController {
  public override void _PhysicsProcess(double delta) {
    var directionX = Input.GetAxis("ui_left", "ui_right");
    var directionY = Input.GetAxis("ui_up", "ui_down");
    direction = new Vector2(directionX, directionY);
    base._PhysicsProcess(delta);
  }
}
