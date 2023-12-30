using System;
using Godot;
using Godot.Collections;

namespace monsterland.simulation.spatial;

public static class CollisionUtility {
  static T getMatchingNode2D<T>(Array<Node2D> bodies, Func<T, bool> predicate) where T : Node2D {
    foreach (var body in bodies) {
      if (body is T t && predicate(t)) {
        return t;
      }
    }

    return null;
  }

  private static readonly Array<Node2D> emptyArray = new Array<Node2D>();

  static Array<Node2D> getOverlappingBodies(Area2D area) {
    return area.HasOverlappingBodies()
      ? area.GetOverlappingBodies()
      : emptyArray;
  }

  public static void checkCollisionAll<T>(Area2D area, Func<T, bool> predicate, Action<T> action) where T : Node2D {
    var bodies = getOverlappingBodies(area);
    foreach (var body in bodies) {
      if (body is T t && predicate(t)) {
        action(t);
      }
    }
  }

  public static T getFirstCollision<T>(Area2D area, Func<T, bool> predicate) where T : Node2D {
    var bodies = getOverlappingBodies(area);
    return getMatchingNode2D(bodies, predicate);
  }
}
