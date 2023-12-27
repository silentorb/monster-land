using Godot;

namespace MonsterLand.simulation.characters;

[Tool]
public partial class CharacterDefinition : Resource {
  [Export] public int sprite = 100;

  [Export] public float speed = 100;
}
