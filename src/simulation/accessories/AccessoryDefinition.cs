using Godot;

namespace monsterland.simulation.accessories; 

[GlobalClass]
[Tool]
public partial class AccessoryDefinition : Resource {
  [Export] public float cooldown;
  [Export] public Godot.Collections.Array<AccessoryEffect> effects = new();
}
