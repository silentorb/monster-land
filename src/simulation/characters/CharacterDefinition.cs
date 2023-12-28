using Godot;
using MonsterLand.simulation.accessories;

namespace MonsterLand.simulation.characters;

[Tool]
[GlobalClass]
public partial class CharacterDefinition : Resource {
  [Export] public int sprite = 100;
  [Export] public float speed = 100;
  [Export] public Godot.Collections.Array<AccessoryDefinition> accessories = new ();
}
