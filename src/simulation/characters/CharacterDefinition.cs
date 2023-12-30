using Godot;
using monsterland.simulation.accessories;

namespace monsterland.simulation.characters;

[Tool]
[GlobalClass]
public partial class CharacterDefinition : Resource {
  [Export] public int sprite = 100;
  [Export] public float speed = 100;
  [Export] public Godot.Collections.Array<AccessoryDefinition> accessories = new ();
  [Export] public int health = 100;
}
