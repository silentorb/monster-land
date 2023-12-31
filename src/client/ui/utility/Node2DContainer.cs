using System;
using Godot;

namespace monsterland.client.ui.utility;

public partial class Node2DContainer : Control {
  [Export] public Vector2 pixelSize = new(32, 32);

  public override void _Process(double delta) {
    base._Process(delta);
    if (GetChildCount() > 0) {
      if (GetChild(0) is Node2D child) {
        var rect = GetRect();
        var rawScale = rect.Size;
        var length = Math.Min(rawScale.X, rawScale.Y);
        var exactSize = new Vector2(length, length);
        child.Scale = exactSize / pixelSize;
        child.Position = rect.Position + exactSize / 2;
        var a = new Vector2(length, length);
        var b = rect.Position / pixelSize + child.Scale / 2;
        var c = 0;
      }
    }
  }
}
