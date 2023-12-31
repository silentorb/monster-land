using Godot;
using Godot.Collections;
using monsterland.client.input;
using monsterland.simulation.characters;
using monsterland.simulation.general;

namespace monsterland.client.ui.views;

public enum NewPlayerMode {
  inactive,
  notReady,
  ready,
}

public partial class NewPlayerView : Control {
  [Export] public PlayerId playerId;
  public NewPlayerMode mode = NewPlayerMode.inactive;
  private AnimatedSprite2D classSprite;

  private Control activePanel;
  private Control inactivePanel;
  private Label statusLabel;
  private int classIndex = 0;

  public override void _Ready() {
    base._Ready();
    
    activePanel = GetNode<Control>("active_panel");
    inactivePanel = GetNode<Control>("inactive_panel");
    classSprite =(AnimatedSprite2D) FindChild("class_sprite");
    var k = FindChild("class_sprite");
    if (FindChild("player_name") is Label playerName) {
      playerName.Text = $"Player {playerId}";
    }

    statusLabel = FindChild("player_status") as Label;
  }

  public void activate() {
    activePanel.Visible = true;
    inactivePanel.Visible = false;
    mode = NewPlayerMode.notReady;
    changeClass(0);
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

  public void removePlayer(InputManager inputManager, PlayerId player) {
    inputManager.disconnectPlayer(player);
    deactivate();
    Global.instance.removePlayer(player);
  }

  public void checkReady(InputManager inputManager) {
    if (inputManager.isJustPressed(playerId, "ui_ready")) {
      inputManager.isJustPressed(playerId, "ui_ready");
      if (mode != NewPlayerMode.inactive) {
        setReady(mode == NewPlayerMode.notReady);
      }
    }
    else if (inputManager.isJustPressed(playerId, "ui_cancel")) {
      if (mode == NewPlayerMode.ready) {
        setReady(false);
      }
    }
  }

  public void checkLeaving(InputManager inputManager) {
    if (inputManager.isJustPressed(playerId, "ui_cancel") &&
        mode == NewPlayerMode.notReady) {
      removePlayer(inputManager, playerId);
    }
  }

  static Array<CharacterDefinition> getClasses() {
    return Global.instance.mode.classes.classes;
  }

  void changeClass(int offset) {
    var classes = getClasses();
    var classCount = classes.Count;
    classIndex = (classIndex + offset + classCount) % classCount;
    classSprite.Frame = classes[classIndex].sprite;
  }

  private void checkClassChange(InputManager inputManager) {
    if (inputManager.isJustPressed(playerId, "ui_left")) {
      changeClass(-1);
    }
    else if (inputManager.isJustPressed(playerId, "ui_right")) {
      changeClass(1);
    }
  }

  public override void _Process(double delta) {
    base._Process(delta);
    var inputManager = Client.instance?.inputManager;
    if (inputManager != null && mode != NewPlayerMode.inactive) {
      checkReady(inputManager);
      checkLeaving(inputManager);
      checkClassChange(inputManager);
    }
  }
}
