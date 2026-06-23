namespace Addon.BrokenCamera.Resources;

using Godot;

[GlobalClass]
public partial class GroupLookAt3D : DampingLookAt3D
{
    public override Quaternion ProcessLogic(Transform3D[] targetsTransforms, Transform3D cameraTransform, double delta)
    {
        Aabb bounds = new(targetsTransforms[0].Origin, Vector3.Zero);
        foreach (Transform3D transform in targetsTransforms)
        {
            bounds = bounds.Expand(transform.Origin);
        }

        Transform3D lookAtTransform = Transform3D.Identity;
        lookAtTransform.Origin = bounds.GetCenter();

        return interpolateRotation(lookAtTransform, cameraTransform, delta);
    }
}