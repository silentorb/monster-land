using System.Linq;
using Godot;
using monsterland.simulation.characters;
using monsterland.simulation.general;

namespace monsterland.client.input;

public partial class PlayerController : CharacterController {
  public Player player;
  [Export] public float actionDeadzone = 0.1f;

  public override void _Ready() {
    base._Ready();
    player = GameState.instance?.getOrCreateAvailablePlayer();
    if (player != null) {
      player.controller = this;
    }
  }

  void update() {
    direction = Input.GetVector("game_move_left", "game_move_right", "game_move_up", "game_move_down");
    var actionVector = Input.GetVector("game_action_left", "game_action_right", "game_action_up", "game_action_down");
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
