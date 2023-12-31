using Godot;
using monsterland.simulation.characters;
using monsterland.simulation.spatial;

namespace monsterland.simulation.entities; 

public partial class HealingItem : Area2D {
  [Export] public int healAmount = 1;
  
  public override void _PhysicsProcess(double delta) {
    base._PhysicsProcess(delta);
    var character = CollisionUtility.getFirstCollision<Character>(this, c => c.faction == 0);
    if (character != null) {
      character.modifyHealth(healAmount);
      QueueFree();
    }
  }
}
