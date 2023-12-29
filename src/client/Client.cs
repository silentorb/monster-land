using Godot;
using monsterland.client.input;

namespace monsterland.client; 

public partial class Client : Node {
  public static Client instance { get; private set; }
  public InputManager inputManager = new ();

  public override void _Ready() {
    base._Ready();
    instance = this;
    inputManager.initialize();
  }

  public override void _Process(double delta) {
    base._Process(delta);
    inputManager.update();
  }

  public override void _ExitTree() {
    if (instance == this) {
      instance = null;
    }

    base._ExitTree();
  }
}