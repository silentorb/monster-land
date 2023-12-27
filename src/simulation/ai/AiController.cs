using System.Linq;
using Godot;

namespace MonsterLand; 

public partial class AiController : simulation.characters.CharacterController {
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

  void attack() {
    var accessory = character.accessories.FirstOrDefault(a => a.canUse());

    if (accessory != null) {
      var target = AiQuery.getNearestEnemy(character);
      if (target != null) {
        var activation = new AccessoryActivation {
          direction = character.Transform.Origin.DirectionTo(target.Transform.Origin) 
        };
        accessory.tryActivate(activation);
      }
    }
  }

  void updateLogic(float delta) {
    updateDirection(delta);
    attack();
  }

  public override void _PhysicsProcess(double delta) {
    updateLogic((float)delta);
    base._PhysicsProcess(delta);
  }
}
