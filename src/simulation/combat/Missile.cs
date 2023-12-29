using Godot;
using Godot.Collections;
using monsterland.simulation.characters;

namespace monsterland.simulation.combat;

public partial class Missile : Area2D {
  public Vector2 velocity;
  public int faction = 1;
  public float range = 64;
  public Damage damage;
  private float distanceTraveled;

  [Signal]
  public delegate void onFinishEventHandler(Character body);

  Character getHitCharacter(Array<Node2D> bodies) {
	foreach (var body in bodies) {
	  if (body is Character character) {
		if (character.faction != faction) {
		  return character;
		}
	  }
	}

	return null;
  }

  public void finish(Character character) {
	character?.damage(ref damage);
	EmitSignal(SignalName.onFinish, character);
	QueueFree();
  }

  public override void _PhysicsProcess(double delta) {
	var offset = velocity * (float)delta;
	Translate(offset);
	distanceTraveled += offset.Length();

	base._PhysicsProcess(delta);

	if (HasOverlappingBodies()) {
	  var bodies = GetOverlappingBodies();
	  var character = getHitCharacter(bodies);
	  finish(character);
	}
	else if (distanceTraveled >= range) {
	  finish(null);
	}
  }
}
