using System.Linq;

namespace monsterland.simulation.ai;

public static class AiQuery {
  public static characters.Character getNearestEnemy(characters.Character character) {
	var characters = general.GameState.instance.characters;
	var nearestDistance = float.MaxValue - 1;
	characters.Character nearest = null;
	var faction = character.faction;

	foreach (var other in characters.AsEnumerable().Where(c => c.isAlive() && c.faction != faction)) {
	  var distance = character.Transform.Origin.DistanceTo(other.Transform.Origin);
	  if (distance < nearestDistance) {
		nearestDistance = distance;
		nearest = other;
	  }
	}

	return nearest;
  }
}
