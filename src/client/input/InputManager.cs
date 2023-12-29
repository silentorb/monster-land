global using DeviceId = System.Int32;
using System;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace monsterland.client.input;

public static class InputConstants {
  public const PlayerId noPlayer = 0;
}

public enum InputSourceType : uint {
  joyButton,
}

public struct InputSource {
  public InputSourceType type;
  public JoyButton joyButton;
}

public enum InputSourceMode : uint {
  justReleased = 0,
  justPressed = 1,
  released = 2,
  pressed = 4,
}

public class DeviceState {
  public Dictionary<object, InputSourceMode> buttons = new();
}

public partial class InputManager : GodotObject {
  public readonly Dictionary<PlayerId, DeviceId> playerDevices = new();
  public readonly Dictionary<string, List<InputSource>> inputMap = new();
  public readonly Dictionary<DeviceId, DeviceState> deviceStates = new();

  public void initializeInputMap() {
    foreach (var action in InputMap.GetActions()) {
      var list = new List<InputSource>();
      foreach (var e in InputMap.ActionGetEvents(action)) {
        if (e is InputEventJoypadButton joyEvent) {
          list.Add(new InputSource {
            type = InputSourceType.joyButton,
            joyButton = joyEvent.ButtonIndex,
          });
        }
      }

      inputMap.Add(action, list);
    }
  }

  public void initialize() {
    initializeInputMap();
  }

  public void initializeDeviceState(DeviceId device, DeviceState state) {
    foreach (var inputs in inputMap.Values) {
      foreach (var input in inputs) {
        if (input.type == InputSourceType.joyButton) {
          var mode = Input.IsJoyButtonPressed(device, input.joyButton)
            ? InputSourceMode.pressed
            : InputSourceMode.released;

          state.buttons.TryAdd(input.joyButton, mode);
        }
      }
    }
  }

  public DeviceState newDeviceState(DeviceId device) {
    var state = new DeviceState();
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

  public void updateDeviceState(DeviceId device, DeviceState state) {
    foreach (var (key, previousMode) in state.buttons) {
      var joyButton = (JoyButton)key;
      var isPressed = Input.IsJoyButtonPressed(device, joyButton);
      state.buttons[joyButton] = getNextInputSourceMode(isPressed, previousMode);
    }
  }

  public void update() {
    foreach (var (device, state) in deviceStates) {
      updateDeviceState(device, state);
    }

    var devices = Input.GetConnectedJoypads();
    foreach (var device in devices) {
      if (!deviceStates.ContainsKey(device)) {
        deviceStates.Add(device, newDeviceState(device));
      }
    }
  }

  public PlayerId connectDevice(DeviceId device) {
    for (var player = 1; player <= 4; ++player) {
      if (!playerDevices.ContainsKey(player)) {
        playerDevices.Add(player, device);
        return player;
      }
    }

    return InputConstants.noPlayer;
  }

  public void disconnectPlayer(PlayerId player) {
    playerDevices.Remove(player);
  }

  public void disconnectDevice(DeviceId device) {
    foreach (var entry in playerDevices) {
      if (entry.Value == device) {
        playerDevices.Remove(entry.Key);
        break;
      }
    }
  }

  public bool isJustPressed(DeviceId device, string action) {
    if (deviceStates.TryGetValue(device, out var state)) {
      if (inputMap.TryGetValue(action, out var inputs)) {
        return inputs.Any(input => state.buttons[input.joyButton] == InputSourceMode.justPressed);
      }
    }

    return false;
  }
}
