using System.Collections.Generic;
using System.Linq;
using Godot;
using monsterland.simulation.characters;

namespace monsterland.simulation.general;

public class GameState {
  public readonly List<Character> characters = new();
  public GameMode mode;
  private float restartTimer;
  private bool isRestarting = false;

  public GameState(GameMode newMode) {
    mode = newMode;
  }

  void startRestart() {
    isRestarting = true;
    restartTimer = mode.restartTimerDuration;
  }

  public bool areAllPlayersDead() {
    // Filter out players without active characters
    var activePlayers = Global.instance.players.AsEnumerable()
      .Select(p => p?.controller?.character)
      .Where(p => p != null);

    // Multiple enumerations are fine in this case.
    // It is a small set and more performant than allocating a List.
    // A stack allocation may be slightly faster but not worth the added complexity.
    return activePlayers.Any() && activePlayers.All(c => !c.isAlive());
  }

  public void update(float delta) {
    if (isRestarting) {
      restartTimer -= delta;
      if (restartTimer <= 0) {
        restartTimer = 0;
        isRestarting = false;
        Global.instance.startGame();
      }
    }
    else if (areAllPlayersDead()) {
      startRestart();
    }
  }
}
