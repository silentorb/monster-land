using System.Linq;
using Godot;
using monsterland.simulation.characters;
using monsterland.simulation.general;

namespace monsterland.client.input;

public partial class PlayerController : CharacterController {
  public Player player;
  [Export] public float movementDeadzone = 0.2f;
  [Export] public float actionDeadzone = 0.2f;

  public void connectPlayer(Player newPlayer) {
    if (newPlayer == null) {
      disconnectPlayer();
    }
    else {
      player = newPlayer;
      player.controller = this;
    }
  }

  void disconnectPlayer() {
    if (player != null) {
      if (player.controller == this) {
        player.controller = null;
      }

      player = null;
    }
  }

  public override void _Ready() {
    base._Ready();
    if (player == null) {
      connectPlayer(GameState.instance?.getOrCreateAvailablePlayer());
    }
  }

  void update() {
    var inputManager = Client.instance.inputManager;
    direction = inputManager.getVector(player.id,
      "game_move_left", "game_move_right",
      "game_move_up", "game_move_down");

    if (direction.Length() < movementDeadzone) {
      direction = Vector2.Zero;
    }

    var actionVector = inputManager.getVector(player.id,
      "game_action_left", "game_action_right",
      "game_action_up", "game_action_down");

    if (actionVector.Length() > actionDeadzone) {
      var accessory = character.accessories.FirstOrDefault(a => a.canUse());

      if (accessory != null) {
        character.tryUseAccessoryInDirection(accessory, actionVector.Normalized());
      }
    }
  }

  public override void _PhysicsProcess(double delta) {
    if (character != null && character.isAlive()) {
      update();
    }

    base._PhysicsProcess(delta);
  }
}
