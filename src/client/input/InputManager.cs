global using DeviceId = System.Int32;
using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using monsterland.mythic;
using InputSource = System.Object;

namespace monsterland.client.input;

public static class InputConstants {
  public const PlayerId noPlayer = 0;
  public const int KeyboardMouseDevice = 100;
  public const int joyAxisDirectionBit = 0x10000;
}

public enum InputSourceType : uint {
  keyboard,
  joyButton,
}

public enum InputSourceMode : uint {
  justReleased = 0,
  justPressed = 1,
  released = 2,
  pressed = 3,
}

public enum JoyAxisDirection {
  leftX_left = 0,
  leftY_up = 1,
  rightX_left = 2,
  rightY_up = 3,
  trigger_left = 4,
  trigger_right = 5,
  leftX_right = leftX_left | InputConstants.joyAxisDirectionBit,
  leftY_down = leftY_up | InputConstants.joyAxisDirectionBit,
  rightX_right = rightX_left | InputConstants.joyAxisDirectionBit,
  rightY_down = rightY_up | InputConstants.joyAxisDirectionBit,
}

public class DeviceState {
  public readonly DeviceId id;
  public readonly Dictionary<object, InputSourceMode> buttons = new();

  public DeviceState(int id) {
    this.id = id;
  }
}

public class InputManager {
  // public readonly Dictionary<DeviceId, PlayerId> devicePlayerMap = new();
  public readonly Dictionary<string, List<InputSource>> inputMap = new();

  // public readonly Dictionary<DeviceId, DeviceState> deviceStates = new();
  public readonly Dictionary<PlayerId, List<DeviceState>> playerDevices = new();

  public void initializeInputMap() {
    foreach (var action in InputMap.GetActions()) {
      var list = new List<InputSource>();
      var events = InputMap.ActionGetEvents(action);

      foreach (var e in events) {
        switch (e) {
          case InputEventJoypadButton joyEvent:
            list.Add(joyEvent.ButtonIndex);
            break;
          case InputEventKey keyboard:
            list.Add(keyboard.Keycode);
            break;
          case InputEventJoypadMotion joyEvent:
            var axisDirection = (int)joyEvent.Axis | (
              joyEvent.AxisValue > 0
                ? InputConstants.joyAxisDirectionBit
                : 0
            );
            list.Add((JoyAxisDirection)axisDirection);
            break;
          default:
            break;
        }
      }

      inputMap.Add(action, list);
    }
  }

  public void initialize() {
    initializeInputMap();
  }

  bool isPressed(DeviceId device, object key) {
    return key switch {
      Key keyboard => device == InputConstants.KeyboardMouseDevice && Input.IsKeyPressed(keyboard),
      JoyButton joyButton => device != InputConstants.KeyboardMouseDevice &&
                             Input.IsJoyButtonPressed(device, joyButton),
      _ => false
    };
  }

  public void initializeDeviceState(DeviceId device, DeviceState state) {
    foreach (var inputs in inputMap.Values) {
      foreach (var input in inputs) {
        var mode = isPressed(device, input)
          ? InputSourceMode.pressed
          : InputSourceMode.released;

        state.buttons.TryAdd(input, mode);
      }
    }
  }

  public DeviceState newDeviceState(DeviceId device) {
    var state = new DeviceState(device);
    initializeDeviceState(device, state);
    return state;
  }

  static InputSourceMode getNextInputSourceMode(bool isPressed, InputSourceMode previous) {
    var pressedBit = Convert.ToInt32(isPressed);
    var previousBits = (int)previous;
    var changedBits = Convert.ToInt32(pressedBit == (previousBits & 0x1)) << 1;
    var value = pressedBit | changedBits;
    return (InputSourceMode)value;
  }

  public void updateDeviceState(DeviceState state) {
    foreach (var (key, previousMode) in state.buttons) {
      var isPressed = this.isPressed(state.id, key);
      state.buttons[key] = getNextInputSourceMode(isPressed, previousMode);
    }
  }

  public void update() {
    foreach (var (player, states) in playerDevices) {
      foreach (var state in states) {
        updateDeviceState(state);
      }
    }

    // var devices = Input.GetConnectedJoypads();
    // foreach (var device in devices) {
    //   if (!deviceStates.ContainsKey(device)) {
    //     deviceStates.Add(device, newDeviceState(device));
    //   }
    // }
  }

  public PlayerId connectDevice(DeviceId device) {
    for (var player = 1; player <= 4; ++player) {
      if (!playerDevices.ContainsKey(player)) {
        playerDevices.Add(player, new List<DeviceState> { newDeviceState(device) });
        return player;
      }
    }

    return InputConstants.noPlayer;
  }

  public bool isDeviceConnectedToPlayer(DeviceId device) {
    return playerDevices.Values.Any(d => d.Any(d2 => d2.id == device));
  }

  public void connectAvailableDevices(PlayerId player) {
    var devices = Input.GetConnectedJoypads().ToList();
    devices.Add(InputConstants.KeyboardMouseDevice);
    var newDevices = devices
      .Where(d => !isDeviceConnectedToPlayer(d))
      .Select(newDeviceState)
      .ToList();

    playerDevices.Add(player, newDevices);

  }

  public void disconnectPlayer(PlayerId player) {
    playerDevices.Remove(player);
  }

  // public void disconnectDevice(DeviceId device) {
  //   devicePlayerMap.Remove(device);
  // }
  public bool isDeviceInputPressed(DeviceId device, string action) {
    return inputMap.TryGetValue(action, out var inputs) &&
           inputs.Any(input => isPressed(device, input));
  }

  public bool isJustPressed(PlayerId player, string action) {
    if (playerDevices.TryGetValue(player, out var states)) {
      if (inputMap.TryGetValue(action, out var inputs)) {
        foreach (var state in states) {
          return inputs.Any(input => state.buttons[input] == InputSourceMode.justPressed);
        }
      }
    }

    return false;
  }

  static void checkKeyboardHalfAxis(List<object> inputs, ref float result, ref float absResult, int vector) {
    foreach (var input in inputs) {
      if (input is Key key) {
        var value = Input.IsKeyPressed(key) ? vector : 0;
        var absValue = Math.Abs(value);
        if (absValue > absResult) {
          result = value;
          absResult = absValue;
        }
      }
    }
  }

  public float getAxis(PlayerId player,
    StringName negative, StringName positive) {
    var result = 0f;
    var absResult = 0f;
    if (playerDevices.TryGetValue(player, out var devices)) {
      if (inputMap.TryGetValue(negative, out var negativeInputs)) {
        if (inputMap.TryGetValue(positive, out var positiveInputs)) {
          foreach (var device in devices) {
            if (device.id == InputConstants.KeyboardMouseDevice) {
              checkKeyboardHalfAxis(negativeInputs, ref result, ref absResult, -1);
              checkKeyboardHalfAxis(positiveInputs, ref result, ref absResult, 1);
            }
            else {
              foreach (var input in negativeInputs) {
                if (input is JoyAxisDirection) {
                  var value = Input.GetJoyAxis(device.id, (JoyAxis)((int)input & 0xF));
                  var absValue = Math.Abs(value);
                  if (absValue > absResult) {
                    result = value;
                    absResult = absValue;
                  }
                }
              }
            }
          }
        }
      }
    }

    return result;
  }

  public Vector2 getVector(PlayerId player,
    StringName negativeX, StringName positiveX,
    StringName negativeY, StringName positiveY) {
    var x = getAxis(player, negativeX, positiveX);
    var y = getAxis(player, negativeY, positiveY);
    return new Vector2(x, y);
  }
}
