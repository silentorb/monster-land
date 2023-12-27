using Godot;

namespace MonsterLand; 

[Tool]
public partial class CharacterTool : Node {
  
  public override void _Process(double delta) {
    base._Process(delta);
    if (Engine.IsEditorHint()) {
      GetParent().GetNode<AnimatedSprite2D>("Sprite");
      var j = GetParent();
      var k = GetParent() as MonsterLand.Character;
      (GetParent() as Character)?.initializeSprite();
    }
    else {
      QueueFree();
    }
  }
}
