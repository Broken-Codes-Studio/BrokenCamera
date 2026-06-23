namespace Addon.BrokenCamera.Resources;

using Godot;
using Godot.Collections;

[GlobalClass]
public abstract partial class DampingLookAt3D : LookAtMode3D
{

    /// <summary>
    /// Offsets the target position.
    /// </summary>
    [Export]
    public Vector3 LookAtOffset { get; set; } = Vector3.Zero;

    private bool _lookAtDamping = false;
    /// <summary>
    /// Applies a damping effect on the camera's movement.
    /// Leading to heavier / slower camera movement as the targeted node moves around.
    /// This is useful to avoid sharp and rapid camera movement.
    /// </summary>
    [Export]
    public bool LookAtDamping
    {
        get => _lookAtDamping;
        set
        {
            _lookAtDamping = value;
            NotifyPropertyListChanged();
        }
    }

    /// <summary>
    /// Defines the Rotational damping amount. The ideal range is typicall somewhere between 0-1.<br/>
    /// The damping amount can be specified in the individual axis.<br/>
    /// <b>Lower value</b> = faster / sharper camera rotation.<br/>
    /// <b>Higher value</b> = slower / heavier camera rotation.
    /// </summary>
    [Export(PropertyHint.Range, "0.0,1.0,0.001,or_greater")]
    public float LookAtDampingValue { get; set; } = .25f;

    public override void _ValidateProperty(Dictionary property)
    {
        if (property["name"].AsStringName() == "LookAtDampingValue" && !LookAtDamping)
            property["usage"] = (int)PropertyUsageFlags.NoEditor;
    }

    protected Quaternion interpolateRotation(Transform3D targetTransform, Transform3D cameraTransform, double delta)
    {
        Vector3 direction = (targetTransform.Origin - cameraTransform.Origin + LookAtOffset).Normalized();
        Basis targetBasis = Basis.LookingAt(direction);
        Quaternion targetQuat = targetBasis.GetRotationQuaternion().Normalized();
        Quaternion cameraQuaternion = cameraTransform.Basis.GetRotationQuaternion().Normalized();

        if (LookAtDamping)
        {
            Quaternion currentQuat = cameraQuaternion;

            float dampingTime = Mathf.Max(.0001f, LookAtDampingValue);
            float t = (float)Mathf.Min(1f, delta / dampingTime);

            float dot = currentQuat.Dot(targetQuat);

            if (dot < .0f)
            {
                targetQuat = -targetQuat;
                dot = -dot;
            }

            dot = Mathf.Clamp(dot, -1f, 1f);

            float theta = Mathf.Acos(dot) * t;

            float sinTheta = Mathf.Sin(theta);
            float sinThetaTotal = Mathf.Sin(Mathf.Acos(dot));

            // Stop interpolating once sin_theta_total reaches a very low value or 0
            if (sinThetaTotal < .00001f)
                return cameraQuaternion;

            float ratioA = Mathf.Cos(theta) - dot * sinTheta / sinThetaTotal;
            float ratioB = sinTheta / sinThetaTotal;

            return currentQuat * ratioA + targetQuat * ratioB;

        }
        else
            return targetQuat;
    }

}