using Godot;

namespace monsterland.simulation.characters;

public static class CharacterUtility {
  public static Character spawnCharacter(SceneTree tree, PackedScene characterScene, CharacterDefinition definition,
    CharacterController controller, Vector2 position) {
    if (characterScene.Instantiate() is Character character) {
      character.initialize(definition);
      character.Position = position;
      character.AddChild(controller);
      // tree.Root.AddChild(character);
      tree.CurrentScene.CallDeferred("add_child", character);
      return character;
    }

    return null;
  }
}
