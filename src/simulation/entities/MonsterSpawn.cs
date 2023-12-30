using Godot;
using monsterland.simulation.ai;
using monsterland.simulation.characters;

namespace monsterland.simulation.entities;

public partial class MonsterSpawn : Node2D {
  [Export] public PackedScene characterScene;
  [Export] public CharacterDefinition definition;
  [Export] public int faction = 1;
  [Export] public float frequency = 10;
  [Export] public int count = 1;

  private float timer;

  public Character spawnCharacter() {
    var controller = new AiController();
    var character = CharacterUtility.spawnCharacter(
      GetTree(), characterScene, definition, controller, Position
    );
    character.faction = faction;
    return character;
  }

  public void spawnCharacters() {
    for (var i = 0; i < count; ++i) {
      spawnCharacter();
    }
  }

  public override void _Process(double delta) {
    base._Process(delta);
    timer += (float)delta;
    if (timer >= frequency) {
      timer -= frequency;
      spawnCharacters();
    }
  }
}
