using Godot;

namespace monsterland.simulation.accessories; 

public struct AccessoryActivation {
  public simulation.characters.Character actor = null;
  public Vector2 direction = Vector2.Zero;

  public AccessoryActivation() {
  }
}
