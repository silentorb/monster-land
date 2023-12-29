using Godot;
using monsterland.simulation.combat;

namespace monsterland.simulation.accessories;

[GlobalClass]
public partial class AttackEffect : AccessoryEffect {
  [Export] public int damageAmount;
  [Export] public DamageType damageType;
  [Export] public PackedScene spawnable;
  [Export] public float missileSpeed;

  public override void activate(ref AccessoryActivation activation) {
    if (spawnable?.Instantiate() is Missile missile) {
      activation.actor.GetTree().Root.AddChild(missile);
      missile.velocity = activation.direction * missileSpeed;
      missile.Position = activation.actor.Position + activation.direction * 32;
      missile.faction = activation.actor.faction;
      missile.damage = new Damage {
        source = activation.actor,
        amount = damageAmount,
        type = damageType,
      };
    }
  }
}
