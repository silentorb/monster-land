using System.Collections.Generic;
using Godot;
using monsterland.simulation.accessories;
using monsterland.simulation.combat;
using monsterland.simulation.general;

namespace monsterland.simulation.characters;

[Tool]
public partial class Character : CharacterBody2D, Damageable {
  [Export] public CharacterDefinition definition;
  [Export] public int faction = 1;
  [Export] public int health = 100;

  private AnimatedSprite2D sprite;
  public readonly List<Accessory> accessories = new();

  public void setSpriteFrame(int frame) {
    if (sprite != null) {
      sprite.Frame = frame;
    }
  }
  
  public void initializeSprite() {
    if (definition == null)
      return;

    setSpriteFrame(definition.sprite);
  }

  public void initialize(CharacterDefinition newDefinition) {
    definition = newDefinition;
    health = definition.health;
  }

  public override void _Ready() {
    base._Ready();
    sprite = GetNode<AnimatedSprite2D>("Sprite");
    initializeSprite();
    Global.instance.state?.characters.Add(this);
    if (definition != null) {
      foreach (var accessoryDefinition in definition.accessories) {
        var accessory = new Accessory { definition = accessoryDefinition };
        accessories.Add(accessory);
      }
    }
  }

  public override void _ExitTree() {
    Global.instance.state?.characters.Remove(this);
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

  public bool isAlive() {
    return health > 0;
  }

  void die() {
    health = 0;
    setSpriteFrame(GD.RandRange(3, 7));
    if (faction != 0)
      QueueFree();
  }

  public void damage(ref Damage damage) {
    if (!isAlive())
      return;
    
    health = Mathf.Max(0, health - damage.amount);

    if (!isAlive()) {
      die();
    }
  }

  public void tryUseAccessoryInDirection(Accessory accessory, Vector2 direction) {
    var activation = new AccessoryActivation {
      actor = this,
      direction = direction
    };
    accessory.tryActivate(ref activation);
  }
}
