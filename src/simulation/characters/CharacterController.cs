using Godot;

namespace MonsterLand.simulation.characters;

public partial class CharacterController : Node {
  public Character character;
  protected Vector2 direction;

  public override void _Ready() {
    base._Ready();
    character = GetParent<Character>();
  }

  public override void _PhysicsProcess(double delta) {
    base._PhysicsProcess(delta);
    if (character == null)
      return;

    if (direction.IsZeroApprox()) {
      character.Velocity = character.Velocity.MoveToward(Vector2.Zero, 10f);
    }
    else {
      character.Velocity = direction.Normalized() * character.definition.speed;
    }

    character.MoveAndSlide();
  }
}
