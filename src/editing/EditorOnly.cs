using Godot;

namespace monsterland.editing; 

public partial class EditorOnly : Node {
  public override void _Ready() {
    base._Ready();
    if (!Engine.IsEditorHint()) {
      QueueFree();
    }
  }
}
