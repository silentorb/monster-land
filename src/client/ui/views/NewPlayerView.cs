using Godot;

namespace monsterland.client.ui.views;

public enum NewPlayerMode {
  inactive,
  notReady,
  ready,
}

public partial class NewPlayerView : Control {
  [Export] public PlayerId playerId;
  public NewPlayerMode mode = NewPlayerMode.inactive;

  private Control activePanel;
  private Control inactivePanel;
  private Label statusLabel;

  public override void _Ready() {
    base._Ready();
    activePanel = GetNode<Control>("active_panel");
    inactivePanel = GetNode<Control>("inactive_panel");
    if (FindChild("player_name") is Label playerName) {
      playerName.Text = $"Player {playerId}";
    }

    statusLabel = FindChild("player_status") as Label;
  }

  public void activate() {
    activePanel.Visible = true;
    inactivePanel.Visible = false;
    mode = NewPlayerMode.notReady;
  }

  public void deactivate() {
    activePanel.Visible = false;
    inactivePanel.Visible = true;
    mode = NewPlayerMode.inactive;
  }

  public void setReady(bool isReady) {
    mode = isReady ? NewPlayerMode.ready : NewPlayerMode.notReady;
    if (statusLabel != null) {
      statusLabel.Text = mode == NewPlayerMode.ready
        ? "Ready"
        : "Not Ready";
    }
  }
}
