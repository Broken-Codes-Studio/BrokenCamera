namespace Addon.BrokenCamera.Resources;

using Godot;

#if TOOLS
[Tool]
#endif
[GlobalClass]
public abstract partial class LookAtMode3D : Resource
{
        public abstract Quaternion ProcessLogic(Transform3D[] targetsTransforms, Transform3D cameraTransform, double delta);
}
