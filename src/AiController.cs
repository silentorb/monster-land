using Godot;

namespace MonsterLand; 

public partial class AiController : CharacterController {
  private float directionTimer = 1;
  public float directionDuration = 1;

  void newDirection() {
    var angle = GD.Randf() * Mathf.Pi * 2;
    direction = new Vector2(1, 0).Rotated(angle);
  }

  public override void _Ready() {
    newDirection();
    base._Ready();
  }

  void updateDirection(float delta) {
    directionTimer -= delta;
    if (directionTimer <= 0) {
      directionTimer = directionDuration;
      newDirection();
    }
  }

  public override void _PhysicsProcess(double delta) {
    updateDirection((float)delta);
    base._PhysicsProcess(delta);
  }
}
