using Godot;
using monsterland.simulation.characters;

namespace monsterland.simulation.general; 

[GlobalClass]
public partial class GameMode : Resource {
  [Export] public PackedScene hud;
  [Export(PropertyHint.File)] public string firstLevel;
  [Export] public float restartTimerDuration = 5;
  [Export] public ClassList classes;
}
