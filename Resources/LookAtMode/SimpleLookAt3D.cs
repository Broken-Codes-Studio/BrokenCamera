namespace Addon.BrokenCamera.Resources;

using Godot;

[GlobalClass]
public partial class SimpleLookAt3D : DampingLookAt3D
{
    public override Quaternion ProcessLogic(Transform3D[] targetsTransforms, Transform3D cameraTransform, double delta)
    {
        Transform3D lookAtTransform = targetsTransforms.Length > 0 ? targetsTransforms[0] : cameraTransform;

        return interpolateRotation(lookAtTransform, cameraTransform, delta);
    }
}