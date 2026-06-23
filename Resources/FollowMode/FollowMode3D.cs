namespace Addon.BrokenCamera.Resources;

using Godot;

#if TOOLS
[Tool]
#endif
[GlobalClass]
public abstract partial class FollowMode3D : Resource
{
    public abstract Vector3 ProcessLogic(Transform3D[] targetsTransforms, Transform3D cameraTransform, double delta);
}
