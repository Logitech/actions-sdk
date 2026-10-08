# Logi Actions SDK

An introduction to the Logi Actions SDK, with example plugin source code for the MX Creative Console and compatible devices.

The plugins in this repo are self-contained projects you can build and run immediately. They are intended both as learning references for new developers and as concrete examples for AI-assisted development.

## Resources

- [Logi Actions SDK Developer Docs](https://logitech.github.io/actions-sdk-docs/)
- [Logi Developer Discord](https://discord.gg/etJCPZytHg)

---

## Plugins

### [DemoPlugin](DemoPlugin/)

The baseline plugin — a good place to start. Demonstrates the basic building blocks of a plugin: simple commands, parameterised commands, an adjustment and image-based actions.

**Actions:**
- **Toggle Mute** (`PluginDynamicCommand`) — mutes and unmutes system volume by sending a `VolumeMute` key press through `ClientApplication`. Ships with a custom icon template (`.ict`).
- **Counter** (`PluginDynamicAdjustment`) — counts dial rotation ticks, with a reset.
- **Thumb up/down** (`PluginDynamicCommand`) — toggles between two embedded PNG images on each press.
- **Button Switches** (`PluginDynamicCommand`) — one command with four parameters (`Switch 0`–`Switch 3`), each toggling its own on/off state shown in the button's display name.

**Key patterns:**
- Minimal `Plugin` and `ClientApplication` setup
- `RunCommand()` handling, and `AddParameter()` for commands with several variants
- `ActionImageChanged()` to refresh a button's name or image after state changes
- Loading embedded images via `PluginResources`
- Custom icon templates in `package/icontemplates/`

**Source:** [`DemoPlugin/DemoPlugin/`](DemoPlugin/DemoPlugin/)

---

### [HapticsPlugin](HapticsPlugin/)

Demonstrates how to trigger haptic feedback from a plugin using the `PluginEvents` API.

**Actions:**
- **Press Counter** (`PluginDynamicCommand`) — increments a counter on each button press, firing a `State Change` waveform every press and a `Special` waveform every 10th press.
- **Tick Counter** (`PluginDynamicAdjustment`) — counts dial rotation ticks, firing a `Collision` waveform per detent and an `Alert` waveform on reset.

**Key patterns:**
- Registering named haptic events in `OnLoad()` via `PluginEvents.AddEvent()`
- Raising events with `PluginEvents.RaiseEvent()` from command and adjustment handlers
- Mapping events to waveform families in `events/extra/eventMapping.yaml`

**Source:** [`HapticsPlugin/src/`](HapticsPlugin/src/)

---

### [SnakePlugin](SnakePlugin/)

Demonstrates `PluginDynamicFolder` by implementing a fully playable Snake game rendered across the 3×3 button grid.

```
[EXIT][ ↑ ][   ]
[ ←  ][ ■ ][ → ]   ■ = centre: start / pause / unpause
[    ][ ↓ ][   ]
```

**Key patterns:**
- `PluginDynamicFolder` taking over all 9 button slots as a single canvas
- `GetNavigationArea()` returning `EncoderArea` to free all 9 slots
- Atomic tile rendering: all 9 `BitmapImage`s built in one lock so every tile sees the same game snapshot
- `CommandImageChanged(rawSlotKey)` called from a background `Task.Run` loop — raw key required (not `CreateCommandName`)
- `Close()` for programmatic folder exit
- Embedded PNG resource loaded via `PluginResources`

**Source:** [`SnakePlugin/src/`](SnakePlugin/src/)

---

## Building

Each plugin is a standard .NET project. Build from the directory containing its `.csproj`:

| Plugin | Project directory | Target |
| --- | --- | --- |
| DemoPlugin | `DemoPlugin/DemoPlugin/` | .NET 8 |
| HapticsPlugin | `HapticsPlugin/src/` | .NET 10 |
| SnakePlugin | `SnakePlugin/src/` | .NET 10 |

```sh
dotnet build -c Debug    # builds and hot-reloads into Logi Plugin Service
dotnet build -c Release  # builds a release version ready for packaging
```

To package for distribution:

```sh
logiplugintool pack ./bin/Release ./<PluginName>.lplug4
```

`logiplugintool` is installed via:

```sh
dotnet tool install -g Loupedeck.PluginTool
```

## Requirements

- [Logi Options+](https://www.logitech.com/en-us/software/logi-options-plus.html) installed (provides `PluginApi.dll`)
- .NET SDK matching the plugin you are building (see the table above)
- MX Creative Console or compatible device (Loupedeck CT, Live, Live S, Razer Stream Controller)

## License

MIT — see [LICENSE](https://opensource.org/licenses/MIT)
