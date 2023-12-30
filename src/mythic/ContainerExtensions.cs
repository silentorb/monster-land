using System;
using System.Collections.Generic;
using System.Linq;

namespace monsterland.mythic;

public static class ContainerExtensions {
  public static void removeAll<K, V>(this Dictionary<K, V> container, Func<K, V, bool> predicate) where V : struct {
    var keys = new List<K>();
    foreach (var (key, value) in container) {
      if (predicate(key, value)) {
        keys.Add(key);
      }
    }

    foreach (var key in keys) {
      container.Remove(key);
    }
  }
}
