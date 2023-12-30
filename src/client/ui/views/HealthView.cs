using System;
using Godot;
using monsterland.simulation.general;

namespace monsterland.client.ui.views;

public partial class HealthView : Control {
  public Control bar;
  public Player player;
  private float value;

  public override void _Ready() {
    base._Ready();
    bar = FindChild("bar") as Control;
    if (bar != null) {
      bar.Scale = new Vector2(0, 1);
    }
  }

  static float lerp(float a, float b, float t) {
    return a + (b - a) * t;
  }

  public override void _Process(double delta) {
    base._Process(delta);
    if (bar != null) {
      var character = player?.controller?.character;
      if (character != null) {
        var maxValue = (float)character.definition.health;
        var targetValue = character.health / maxValue;
        const float incrementRate = 0.2f;
        var increment = incrementRate * (float)delta;
        value += Math.Min(increment, Math.Max(-increment, targetValue - value));
        value = lerp(value, targetValue, (float)delta * 2);
        bar.Scale = new Vector2(value, 1);
      }
    }
  }
}
