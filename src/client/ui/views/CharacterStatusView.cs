using Godot;
using monsterland.simulation.general;

namespace monsterland.client.ui.views; 

public partial class CharacterStatusView : Control {
  [Export] public int playerId;
  public Player player;
  private HealthView healthView;

  public override void _Ready() {
    base._Ready();
    healthView = FindChild("health") as HealthView;
  }

  public override void _Process(double delta) {
    base._Process(delta);
    if (player == null) {
      player = Global.instance.getPlayerById(playerId);
      if (player != null) {
        if (healthView != null) {
          healthView.player = player;
        }
      }
    }
  }
}
