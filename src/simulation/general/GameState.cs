using System.Collections.Generic;
using Godot;

namespace MonsterLand.simulation.general;

public partial class GameState : Node {
  public static GameState instance { get; private set; }
  public readonly List<characters.Character> characters = new();

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
}
