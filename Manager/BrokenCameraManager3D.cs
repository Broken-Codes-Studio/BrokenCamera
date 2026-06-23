
using Interfaces.Utility;

namespace Addon.BrokenCamera.Managers;

using Godot;

using Host;
using Cameras;

#if TOOLS
[Tool]
#endif
/// <summary>
/// Manages 3D Broken Cameras and their hosts.
/// </summary>
public partial class BrokenCameraManager3D : BrokenCameraManager<BrokenCamera3D, BrokenCameraHost3D>, ISingleton<BrokenCameraManager3D>
{
    /// <summary>
    /// Singleton instance of the BrokenCameraManager3D.
    /// </summary>
    public static BrokenCameraManager3D Instance { get; private set; }

    public BrokenCameraManager3D()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        Instance = this;
    }
}