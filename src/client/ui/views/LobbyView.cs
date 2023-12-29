using System.Collections.Generic;
using System.Linq;
using Godot;
using monsterland.client.input;

namespace monsterland.client.ui.views;

public partial class LobbyView : Control {
  public readonly List<NewPlayerView> playerViews = new();
  [Export(PropertyHint.File)] public string nextScene;

  public override void _Ready() {
    base._Ready();
    var container = GetNode<Container>("players");
    if (container != null) {
      var children = container.GetChildren().OfType<NewPlayerView>();
      playerViews.AddRange(children);
    }
  }

  NewPlayerView getViewByPlayerId(PlayerId player) {
    return playerViews.FirstOrDefault(v => v.playerId == player);
  }

  public void addPlayer(InputManager inputManager, DeviceId device) {
    var player = inputManager.connectDevice(device);
    if (player != InputConstants.noPlayer) {
      getViewByPlayerId(player)?.activate();
    }
  }

  public void removePlayer(InputManager inputManager, PlayerId player) {
    inputManager.disconnectPlayer(player);
    getViewByPlayerId(player)?.deactivate();
  }

  public void checkJoining(InputManager inputManager) {
    var devices = Input.GetConnectedJoypads();
    foreach (var device in devices) {
      if (!inputManager.playerDevices.Values.Contains(device)) {
        if (inputManager.isJustPressed(device, "ui_join")) {
          addPlayer(inputManager, device);
        }
      }
    }
  }

  public void checkLeaving(InputManager inputManager) {
    foreach (var (player, device) in inputManager.playerDevices) {
      if (!Input.IsJoyKnown(device) || (inputManager.isJustPressed(device, "ui_cancel") &&
                                        getViewByPlayerId(player)?.mode == NewPlayerMode.notReady)) {
        removePlayer(inputManager, player);
      }
    }
  }

  public bool isReady() {
    return playerViews.Any(v => v.mode == NewPlayerMode.ready) &&
           playerViews.All(v => v.mode != NewPlayerMode.notReady);
  }

  public void checkReady(InputManager inputManager) {
    foreach (var (player, device) in inputManager.playerDevices) {
      if (inputManager.isJustPressed(device, "ui_ready")) {
        var view = getViewByPlayerId(player);
        if (view != null && view.mode != NewPlayerMode.inactive) {
          view.setReady(view.mode == NewPlayerMode.notReady);
        }
      }
      else if (inputManager.isJustPressed(device, "ui_cancel")) {
        var view = getViewByPlayerId(player);
        if (view.mode == NewPlayerMode.ready) {
          view.setReady(false);
        }
      }
    }

    if (isReady()) {
      GetTree().ChangeSceneToFile(nextScene);
    }
  }

  public override void _Process(double delta) {
    base._Process(delta);
    var inputManager = Client.instance?.inputManager;
    if (inputManager != null) {
      checkLeaving(inputManager);
      checkJoining(inputManager);
      checkReady(inputManager);
    }
  }
}
