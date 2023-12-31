using System.Collections.Generic;
using System.Linq;
using Godot;
using monsterland.client.input;
using monsterland.simulation.general;

namespace monsterland.client.ui.views;

public partial class LobbyView : Control {
  public readonly List<NewPlayerView> playerViews = new();
  [Export] public GameMode mode;

  public override void _Ready() {
    base._Ready();
    Global.instance?.setModeIfUnset(mode); 
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
      Global.instance.addPlayer(player);
    }
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

  public bool isReady() {
    return playerViews.Any(v => v.mode == NewPlayerMode.ready) &&
           playerViews.All(v => v.mode != NewPlayerMode.notReady);
  }

  public void startGame() {
    Global.instance.startGame();
  }

  public override void _Process(double delta) {
    base._Process(delta);
    var inputManager = Client.instance?.inputManager;
    if (inputManager != null) {
      if (isReady()) {
        startGame();
      }
      
      checkJoining(inputManager);
    }
  }
}
