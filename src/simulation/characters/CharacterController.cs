using Godot;

namespace monsterland.simulation.characters;

public partial class CharacterController : Node {
  public Character character;
  protected Vector2 direction;

  public override void _Ready() {
    base._Ready();
    character = GetParent<Character>();
  }

  public override void _PhysicsProcess(double delta) {
    base._PhysicsProcess(delta);
    if (character == null || !character.isAlive())
      return;
      
    if (direction.IsZeroApprox()) {
      character.Velocity = character.Velocity.MoveToward(Vector2.Zero, 10f);
    }
    else {
      // Limit the direction length
      if (direction.Length() > 1) {
        direction = direction.Normalized();
      }
      
      character.Velocity = direction * character.definition.speed;
    }

    character.MoveAndSlide();
  }
}
