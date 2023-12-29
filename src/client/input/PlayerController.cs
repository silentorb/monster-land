using Godot;
using monsterland.simulation.general;

namespace monsterland.client.input;

public partial class PlayerController : simulation.characters.CharacterController {
  public Player player;

  public override void _Ready() {
    base._Ready();
    player = GameState.instance?.getOrCreateAvailablePlayer();
    if (player != null) {
      player.controller = this;
    }
  }

  public override void _PhysicsProcess(double delta) {
    if (character != null && character.isAlive()) {
      direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
    }

    base._PhysicsProcess(delta);
  }
}
