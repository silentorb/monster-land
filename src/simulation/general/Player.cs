global using PlayerId = System.Int32;
using Godot;
using monsterland.client.input;

namespace monsterland.simulation.general; 

public partial class Player : GodotObject {
  public PlayerId id = 0;
  public PlayerController controller;
}
