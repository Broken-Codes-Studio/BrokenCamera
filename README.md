
<p align="center">
  <a href="https://godotengine.org/download/windows/">
      <img alt="Static Badge" src="https://img.shields.io/badge/Godot-4.6%2B-blue">
  </a>
  <a href="LICENSE">
    <img alt="GitHub License" src="https://img.shields.io/github/license/Broken-Codes-Studio/BrokenSigil">
  </a>
</p>

# 🎥 What is it?

Broken Camera is a Godot 4 C# plugin that extends and simplifies common camera behaviors for the built-in `Camera3D` node (`Camera2D` support is planned for a future release).

Inspired by Unity's Cinemachine and the Phantom Camera addon for Godot, Broken Camera provides an intuitive way to create dynamic camera systems. It supports simple behaviors such as following and looking at targets with optional smoothing, as well as more advanced features like framing multiple targets and transitioning between predefined cameras.

Because follow and look-at behaviors are implemented independently, users can freely combine different `FollowMode` and `LookAtMode` resources to create custom camera setups. Additional behaviors can also be implemented by inheriting from these resource types.

> ⚠️ **Dependency Notice**
> 
> This addon depends on the [**Broken Sigil**](https://github.com/Broken-Codes-Studio/BrokenSigil) addon. Please install it alongside Broken Camera before enabling the plugin.

## ✨ Features

### 🎯 Priority

Determines which `BrokenCamera` instance should control the active `Camera3D`.

When a camera's priority becomes higher than the currently active `BrokenCamera`, the system automatically transitions to the new camera.

### 🚶Follow Modes

Define how the camera follows or repositions itself relative to its target(s).

#### Glued

Locks the camera directly to its target.

#### Simple

Follows the target using optional offsets and damping.

#### Group

Follows the center point of a collection of targets.

### 👀 Look At Modes

Define where the camera should look by controlling its rotation.

#### Mimic

Copies the target's rotation.

#### Simple

Looks at the target using an optional offset.

#### Group

Looks at the center point of a collection of targets.

### 🎬 Tweening

Controls how the camera transitions when switching between active `BrokenCamera` instances.

### 🛠️ Custom Behaviors

Create custom follow and look-at behaviors by inheriting from the `FollowMode` and `LookAtMode` resource classes.
## 💾 Installation

### GitHub Submodule (Recommended)

Add the repository as a submodule:

```bash
git submodule add <repository-url> addons/BrokenCamera
git submodule update --init --recursive
```

Then build the project and enable the plugin from:

```
Project > Project Settings > Plugins
```

### GitHub Download

1. Download the latest [`main branch`](https://github.com/Broken-Codes-Studio/BrokenCamera/archive/refs/heads/main.zip).
    
2. Extract the archive.
    
3. Copy the `addons/BrokenCamera` folder into your project's root directory.
    
4. Build the Project
    
5. Enable the plugin from:
    

```
Project > Project Settings > Plugins
```

For additional information, see the [official Godot documentation](https://docs.godotengine.org/en/stable/tutorials/plugins/editor/installing_plugins.html).
## 🙏 Credits

-  [Unity's Cinemachine Package](https://unity.com/unity/features/editor/art-and-design/cinemachine) for much of the original inspiration.
    
- [Phantom Camera Addon](https://github.com/ramokz/phantom-camera) for demonstrating advanced camera workflows in Godot.
    
- [Godot Engine](https://godotengine.org/) and its contributors.