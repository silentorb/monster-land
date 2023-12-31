using Godot;
using Godot.Collections;

namespace monsterland.simulation.characters;

[GlobalClass]
public partial class ClassList : Resource {
  [Export] public Array<CharacterDefinition> classes = new ();
}
