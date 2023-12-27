using Godot;

namespace MonsterLand; 

[Tool]
public partial class Character : CharacterBody2D {
  
  [Export]
  public CharacterDefinition definition;

  private AnimatedSprite2D sprite;

  void initializeSprite() {
    if (definition == null)
      return;

    if (sprite != null) {
      sprite.Frame = definition.sprite;
    }
  }

  public override void _Ready() {
    base._Ready();
    sprite = GetNode<AnimatedSprite2D>("Sprite");
    initializeSprite();
  }

  public override void _Process(double delta) {
    base._Process(delta);
    if (Engine.IsEditorHint()) {
      initializeSprite();
    }
  }
}