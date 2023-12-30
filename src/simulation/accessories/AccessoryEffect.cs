using Godot;

namespace monsterland.simulation.accessories; 

public partial class AccessoryEffect : Resource {
  [Export] public float range = 64;

  public virtual void activate(ref AccessoryActivation activation) {
  }
}