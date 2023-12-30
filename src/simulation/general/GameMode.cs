using Godot;

namespace monsterland.simulation.general; 

[GlobalClass]
public partial class GameMode : Resource {
  [Export] public PackedScene hud;
  [Export(PropertyHint.File)] public string firstLevel;
  [Export] public float restartTimerDuration = 5;
}
