using System;
using Godot;

namespace MonsterLand; 

public partial class Accessory : GodotObject {
  public AccessoryDefinition definition;
  public float cooldown;

  void activate() {
    cooldown = definition.cooldown;
  }

  public bool canUse() {
    return cooldown <= 0;
  }

  public bool tryActivate(AccessoryActivation activation) {
    if (!canUse())
      return false;
    
    activate();
    
    return true;
  }

  public void update(float delta) {
    cooldown = Math.Max(0, cooldown - delta);
  }
}
