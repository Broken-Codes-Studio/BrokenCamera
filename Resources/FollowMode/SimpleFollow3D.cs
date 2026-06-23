namespace Addon.BrokenCamera.Resources;

using Godot;

using System;

[GlobalClass]
public partial class SimpleFollow3D : DampingFollow3D
{
    public override Vector3 ProcessLogic(Transform3D[] targetsTransforms, Transform3D cameraTransform, double delta)
    {
        Vector3 followPosition = targetsTransforms.Length > 0 ? targetsTransforms[0].Origin : cameraTransform.Origin;
        followPosition += FollowOffset;

        return interpolatePosition(followPosition, cameraTransform.Origin, delta);
    }
}
