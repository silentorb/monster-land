using System.Collections.Generic;
using System.Linq;
using Godot;
using monsterland.simulation.characters;

namespace monsterland.simulation.general;

public partial class GameState : Node {
  public static GameState instance { get; private set; }
  public readonly List<Character> characters = new();
  public readonly List<Player> players = new();

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

  public Player getOrCreateAvailablePlayer() {
    var available = players.FirstOrDefault(p => p.controller == null);
    if (available != null) 
      return available;

    var player = new Player();
    players.Add(player);
    return player;
  }
}
