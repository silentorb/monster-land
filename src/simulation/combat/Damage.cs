using MonsterLand.simulation.characters;

namespace MonsterLand.simulation.combat; 

public struct Damage {
  public Character source;
  public int amount;
  public DamageType type;
}
