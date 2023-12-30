using System.Collections.Generic;
using System.Linq;
using Godot;

namespace monsterland.simulation.general;

public partial class Global : Node {
  public static Global instance { get; private set; }
  public GameState state;
  public GameMode mode;
  public readonly List<Player> players = new();

  public void resetGame() {
    foreach (var player in players) {
      player.controller = null;
    }
    
    state = new GameState(mode);
  }
  
  public override void _Ready() {
    base._Ready();
    instance = this;
  }

  public override void _ExitTree() {
    if (instance == this) {
      instance = null;
    }

    base._ExitTree();
  }
  
  public void setModeIfUnset(GameMode newMode) {
    mode ??= newMode;
  }

  public void startGame() {
    resetGame();
    GetTree().ChangeSceneToFile(mode.firstLevel);
  }

  public override void _PhysicsProcess(double delta) {
    base._PhysicsProcess(delta);
    state?.update((float)delta);
  }
  
  public Player addPlayer(PlayerId id) {
    var player = new Player(id);
    players.Add(player);
    return player;
  }

  public Player getPlayerById(PlayerId id) {
    return players.FirstOrDefault(p => p.id == id);
  }

  public void removePlayer(PlayerId id) {
    players.RemoveAll(p => p.id == id);
  }

  public Player getOrCreateAvailablePlayer() {
    var available = players.FirstOrDefault(p => p.controller == null);
    if (available != null)
      return available;

    var nextId = players.Any()
      ? players.Select(p => p.id).Max() + 1
      : 1;

    return addPlayer(nextId);
  }

}
