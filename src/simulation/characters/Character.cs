using System.Collections.Generic;
using Godot;

namespace MonsterLand;

[Tool]
public partial class Character : CharacterBody2D {
  [Export] public simulation.characters.CharacterDefinition definition;
  [Export] public int faction = 1;

  private AnimatedSprite2D sprite;
  public readonly List<Accessory> accessories = new();

  public void initializeSprite() {
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
	GameState.instance?.characters.Add(this);
  }

  public override void _ExitTree() {
	GameState.instance?.characters.Remove(this);
	base._ExitTree();
  }

  void update(float delta) {
	foreach (var accessory in accessories) {
	  accessory.update(delta);
	}
  }

  public override void _Process(double delta) {
	base._Process(delta);
	if (Engine.IsEditorHint()) {
	  initializeSprite();
	}

	update((float)delta);
  }
}
