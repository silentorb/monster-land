using System.Linq;
using Godot;
using monsterland.client;
using monsterland.client.input;
using monsterland.simulation.characters;
using monsterland.simulation.general;

namespace monsterland.simulation.entities;

public partial class PlayerSpawn : Node2D {
  [Export] public PackedScene characterScene;
  [Export] public CharacterDefinition defaultDefinition;
  [Export] public int faction;

  public Character spawnPlayerCharacter(Player player, Vector2 position) {
    var controller = new PlayerController();
    controller.connectPlayer(player);
    var character = CharacterUtility.spawnCharacter(
      GetTree(), characterScene, player.characterDefinition, controller, position
    );
    character.faction = faction;
    return character;
  }

  void checkSpawm() {
    if (!Global.instance.players.Any()) {
      var player = Global.instance.getOrCreateAvailablePlayer();
      Client.instance.inputManager.connectAvailableDevices(player.id);
    }

    {
      var player = Global.instance.players.FirstOrDefault(p => p.controller == null);
      if (player != null) {
        player.characterDefinition ??= defaultDefinition;
        spawnPlayerCharacter(player, Position);
      }
    }
  }

  public override void _Ready() {
    base._Ready();
    checkSpawm();
  }
}
