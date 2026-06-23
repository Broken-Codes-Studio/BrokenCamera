namespace Addon.BrokenCamera.Resources;

using Godot;

[GlobalClass]
public partial class GluedFollow3D : FollowMode3D
{
    public override Vector3 ProcessLogic(Transform3D[] targetsTransforms, Transform3D cameraTransform, double delta) => targetsTransforms.Length > 0 ? targetsTransforms[0].Origin : cameraTransform.Origin;

}
