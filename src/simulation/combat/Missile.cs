using Godot;

namespace MonsterLand.simulation.combat;

public partial class Missile : Area2D {
  public Vector2 velocity;
  public int faction = 1;
  public float range;
  public Damage damage;

  public override void _PhysicsProcess(double delta) {
	Translate(velocity * (float)delta);

	base._PhysicsProcess(delta);

	if (HasOverlappingBodies()) {
	  var bodies = GetOverlappingBodies();
	  foreach (var body in bodies) {
		if (body is characters.Character character) {
		  if (character.faction != faction) {
			character.damage(ref damage);
		  }
		}
	  }

	  QueueFree();
	}
  }
}
