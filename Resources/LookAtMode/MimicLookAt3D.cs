namespace Addon.BrokenCamera.Resources;

using Godot;

[GlobalClass]
public partial class MimicLookAt3D : LookAtMode3D
{
    public override Quaternion ProcessLogic(Transform3D[] targetsTransforms, Transform3D cameraTransform, double delta) => targetsTransforms.Length > 0 ? targetsTransforms[0].Basis.GetRotationQuaternion() : cameraTransform.Basis.GetRotationQuaternion();
}
