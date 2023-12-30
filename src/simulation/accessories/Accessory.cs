using System;
using System.Linq;
using Godot;

namespace monsterland.simulation.accessories; 

public class Accessory {
  public AccessoryDefinition definition;
  public float cooldown;

  void activate(ref AccessoryActivation activation) {
    cooldown = definition.cooldown;
    foreach (var effect in definition.effects) {
      effect.activate(ref activation);
    }
  }

  public bool canUse() {
    return cooldown <= 0 && definition.effects.Any();
  }

  public bool tryActivate(ref AccessoryActivation activation) {
    if (!canUse())
      return false;
    
    activate(ref activation);
    
    return true;
  }

  public void update(float delta) {
    cooldown = Math.Max(0, cooldown - delta);
  }
}
