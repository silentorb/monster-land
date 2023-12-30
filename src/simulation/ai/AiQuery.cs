using System;
using System.Linq;
using Godot;
using monsterland.simulation.characters;
using monsterland.simulation.general;

namespace monsterland.simulation.ai;

public static class AiQuery {
  public static Character getNearestEnemy(Character character, float range) {
    var characters = GameState.instance.characters;
    var nearestDistance = float.MaxValue - 1;
    Character nearest = null;
    var faction = character.faction;

    foreach (var other in characters.AsEnumerable().Where(c => c.isAlive() && c.faction != faction)) {
      var distance = character.Position.DistanceTo(other.Position);
      if (distance < range && distance < nearestDistance) {
        nearestDistance = distance;
        nearest = other;
      }
    }

    return nearest;
  }
  
  public static Character getRandomEnemy(Character character, float range) {
    var characters = GameState.instance.characters;
    var faction = character.faction;
    const int maxSize = 64;
    Span<int> buffer = stackalloc int[maxSize];
    var length = 0;

    var i = -1;
    foreach (var other in characters) {
      ++i;
      if (other.isAlive() && other.faction != faction) {
        var distance = character.Position.DistanceTo(other.Position);
        if (distance < range) {
          if (length >= maxSize - 1)
            break;

          buffer[length++] = i;
        }
      }
    }
    
    if (length == 0)
      return null;

    var roll = GD.RandRange(0, length - 1);
    var index = buffer[roll];
    return characters[index];
  }
}
