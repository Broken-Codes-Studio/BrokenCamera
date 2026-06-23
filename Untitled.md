# 🎥 Broken Camera

Broken Camera is a Godot 4 C# plugin that extends and simplifies common camera behaviors for the built-in `Camera3D` node. (`Camera2D` support is planned for a future release.)

Inspired by Unity's Cinemachine and the Phantom Camera addon for Godot, Broken Camera provides an intuitive way to build dynamic camera systems. It supports simple behaviors such as following and looking at targets with optional smoothing, as well as advanced features like framing multiple targets and transitioning between predefined cameras.

Because follow and look-at behaviors are implemented independently, different `FollowMode` and `LookAtMode` resources can be freely combined to create custom camera setups. Additional behaviors can also be implemented by inheriting from these resource types.

> ⚠️ **Dependency Notice**
> 
> This addon depends on the **Broken Sigil** addon. Please install it alongside Broken Camera before enabling the plugin.

## ✨ Features

### 🎯 Priority

Determines which `BrokenCamera` instance should control the active `Camera3D`.

When a camera's priority becomes higher than the currently active `BrokenCamera`, the system automatically transitions to the new camera.

### 🚶 Follow Modes

Control how the camera follows or positions itself relative to its target(s).

#### Glued

Locks the camera directly to its target.

#### Simple

Follows the target using configurable offsets and damping.

#### Group

Follows the center point of a collection of targets.

### 👀 Look-At Modes

Control where the camera looks by managing its rotation.

#### Mimic

Copies the target's rotation.

#### Simple

Looks at the target using an optional offset.

#### Group

Looks at the center point of a collection of targets.

### 🎬 Tweening

Controls how the camera transitions between active `BrokenCamera` instances.

### 🛠️ Custom Behaviors

Create your own follow and look-at behaviors by inheriting from the `FollowMode` and `LookAtMode` resource classes.

## 💾 Installation

### GitHub Submodule (Recommended)

Add the repository as a submodule:

```bash
git submodule add <repository-url> addons/BrokenCamera
git submodule update --init --recursive
```

Then build the project and enable the plugin:

```text
Project → Project Settings → Plugins
```

### GitHub Download

1. Download the latest release from the repository.
    
2. Extract the archive.
    
3. Copy the `addons/BrokenCamera` folder into your project's root directory.
    
4. Build the project.
    
5. Enable the plugin:
    

```text
Project → Project Settings → Plugins
```

For additional information, see the official Godot documentation on plugin installation.

## 🙏 Credits

- Unity's **Cinemachine** package for much of the original inspiration.
    
- **Phantom Camera** for demonstrating advanced camera workflows in Godot.
    
- **Godot Engine** and its contributors.