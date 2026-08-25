# Logi Actions SDK — Example Plugins

A collection of example plugins for the [Logi Actions SDK](https://logitech.github.io/actions-sdk-docs/), demonstrating common patterns for building plugins for the MX Creative Console and compatible devices.

Each plugin is a self-contained project you can build and run immediately. They are intended both as learning references for new developers and as concrete examples for AI-assisted development.

---

## Plugins

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

Each plugin is a standard .NET 10 project. From inside the plugin's `src/` directory:

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
- .NET 10 SDK
- MX Creative Console or compatible device (Loupedeck CT, Live, Live S, Razer Stream Controller)

## License

MIT — see [LICENSE](https://opensource.org/licenses/MIT)
