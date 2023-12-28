using System.Linq;
using Godot;
using MonsterLand.simulation.ai;

namespace MonsterLand;

public partial class AiController : simulation.characters.CharacterController {
  private float directionTimer = 1;
  public float directionDuration = 1;
  private float actionChance = 0;

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
          actor = character,
          direction = character.Transform.Origin.DirectionTo(target.Transform.Origin)
        };
        accessory.tryActivate(ref activation);
      }
    }
  }

  void updateLogic(float delta) {
    if (!character.isAlive())
      return;
    
    updateDirection(delta);

    actionChance += delta;
    if (GD.Randf() < actionChance) {
      attack();
      actionChance = 0;
    }
  }

  public override void _PhysicsProcess(double delta) {
    updateLogic((float)delta);
    base._PhysicsProcess(delta);
  }
}
