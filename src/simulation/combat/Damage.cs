using monsterland.simulation.characters;

namespace monsterland.simulation.combat; 

public struct Damage {
  public Character source;
  public int amount;
  public DamageType type;
}
