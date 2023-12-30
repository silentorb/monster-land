using Godot;
using monsterland.simulation.accessories;

namespace monsterland.simulation.combat;

[GlobalClass]
public partial class AttackEffect : AccessoryEffect {
  [Export] public int damageAmount;
  [Export] public DamageType damageType;
  [Export] public PackedScene spawnable;
  [Export] public float missileSpeed;

  public override void activate(ref AccessoryActivation activation) {
    if (spawnable?.Instantiate() is Missile missile) {
      missile.velocity = activation.direction * missileSpeed;
      missile.Position = activation.actor.Position + activation.direction * 32;
      missile.faction = activation.actor.faction;
      missile.range = range;
      missile.damage = new Damage {
        source = activation.actor,
        amount = damageAmount,
        type = damageType,
      };
      
      activation.actor.GetTree().Root.AddChild(missile);
    }
  }
}
