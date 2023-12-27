namespace MonsterLand; 

public static class AiQuery {
  public static Character getNearestEnemy(Character character) {
    var characters = GameState.instance.characters;
    var nearestDistance = float.MaxValue - 1;
    Character nearest = null;
    foreach (var other in characters) {
      var distance = character.Transform.Origin.DistanceTo(other.Transform.Origin);
      if (distance < nearestDistance) {
        nearestDistance = distance;
        nearest = other;
      }
    }
    return nearest;
  }
}
