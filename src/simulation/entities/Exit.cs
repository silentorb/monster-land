using Godot;
using monsterland.simulation.characters;
using monsterland.simulation.spatial;

namespace monsterland.client.ui.views; 

public partial class Exit : Area2D {
  [Export(PropertyHint.File)] public string destination;

  void travel() {
    GetTree().ChangeSceneToFile(destination);
  }

  public override void _PhysicsProcess(double delta) {
    base._PhysicsProcess(delta);
    var character = CollisionUtility.getFirstCollision<Character>(this, c => c.faction == 0);
    if (character != null) {
      travel();
    }
  }
}
