using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using monsterland.client.input;
using monsterland.simulation.general;

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
      GameState.instance.addPlayer(player);
    }
  }

  public void removePlayer(InputManager inputManager, PlayerId player) {
    inputManager.disconnectPlayer(player);
    getViewByPlayerId(player)?.deactivate();
    GameState.instance.removePlayer(player);
  }

  public void checkJoiningForDevice(InputManager inputManager, DeviceId device) {
    if (!inputManager.isDeviceConnectedToPlayer(device) &&
        inputManager.isDeviceInputPressed(device, "ui_join")) {
      addPlayer(inputManager, device);
    }
  }
  
  public void checkJoining(InputManager inputManager) {
    checkJoiningForDevice(inputManager, InputConstants.KeyboardMouseDevice);

    var gamepads = Input.GetConnectedJoypads();
    foreach (var device in gamepads) {
      checkJoiningForDevice(inputManager, device);
    }
  }

  public void checkLeaving(InputManager inputManager) {
    foreach (var (player, _) in inputManager.playerDevices) {
      if (inputManager.isJustPressed(player, "ui_cancel") &&
          getViewByPlayerId(player)?.mode == NewPlayerMode.notReady) {
        removePlayer(inputManager, player);
      }
    }
  }

  public bool isReady() {
    return playerViews.Any(v => v.mode == NewPlayerMode.ready) &&
           playerViews.All(v => v.mode != NewPlayerMode.notReady);
  }

  public void startGame() {
    GetTree().ChangeSceneToFile(nextScene);
  }

  public void checkReady(InputManager inputManager) {
    foreach (var (player, _) in inputManager.playerDevices) {
      if (inputManager.isJustPressed(player, "ui_ready")) {
        inputManager.isJustPressed(player, "ui_ready");
        var view = getViewByPlayerId(player);
        if (view != null && view.mode != NewPlayerMode.inactive) {
          view.setReady(view.mode == NewPlayerMode.notReady);
        }
      }
      else if (inputManager.isJustPressed(player, "ui_cancel")) {
        var view = getViewByPlayerId(player);
        if (view.mode == NewPlayerMode.ready) {
          view.setReady(false);
        }
      }
    }

    if (isReady()) {
      startGame();
    }
  }

  public override void _Process(double delta) {
    base._Process(delta);
    var inputManager = Client.instance?.inputManager;
    if (inputManager != null) {
      // The order of these is important.
      // Some UI events will bleed over if the order is changed.
      checkReady(inputManager);
      checkLeaving(inputManager);
      checkJoining(inputManager);
    }
  }
}
