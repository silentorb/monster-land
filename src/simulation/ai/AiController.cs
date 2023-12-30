using System.Linq;
using Godot;
using Godot.Collections;
using monsterland.simulation.accessories;
using monsterland.simulation.characters;

namespace monsterland.simulation.ai;

public partial class AiController : CharacterController {
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
      var target = AiQuery.getRandomEnemy(character, accessory.definition.effects[0].range + 16);
      if (target != null) {
        // var spaceState = character.GetWorld2D().DirectSpaceState;
        // var query = PhysicsRayQueryParameters2D.Create(character.Position, target.Position,
        //   exclude: new Array<Rid> { character.GetRid() });
        // var result = spaceState.IntersectRay(query);
        // if (result.Count > 0) {
        var actionDirection = character.Transform.Origin.DirectionTo(target.Transform.Origin);
        character.tryUseAccessoryInDirection(accessory, actionDirection);
        // }
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
