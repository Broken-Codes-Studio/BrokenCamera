namespace Addon.BrokenCamera.Resources;

using Godot;
using Godot.Collections;

[GlobalClass]
public partial class GroupFollow3D : DampingFollow3D
{

    /// <summary>
    /// Sets a distance offset from the centre of the target's position.
    /// The distance is applied to the [param PhantomCamera3D]'s local z axis.
    /// </summary>
    [Export]
    public float FollowDistance { get; set; } = 1f;

    private bool _autoFollowDistance = false;
    /// <summary>
    /// Enables the [param PhantomCamera3D] to automatically distance
    /// itself as the [param follow targets] move further apart.<br/>
    /// It looks at the longest axis between the different targets and interpolates
    /// the distance length between the [member auto_follow_distance_min] and
    /// [member follow_group_distance] properties.<br/>
    /// <b>Note:</b> Enabling this property hides and disables the [member follow_distance]
    /// property as this effectively overrides that property.
    /// </summary>
    [Export]
    public bool AutoFollowDistance
    {
        get => _autoFollowDistance;
        set
        {
            _autoFollowDistance = value;
            NotifyPropertyListChanged();
        }
    }

    /// <summary>
    /// Sets the minimum distance between the Camera and centre of [AABB].<br/>
    /// <b>Note:</b> This distance will only ever be reached when all the targets are in
    /// the exact same [param Vector3] coordinate, which will very unlikely
    /// happen, so adjust the value here accordingly.
    /// </summary>
    [Export]
    public float AutoFollowDistanceMin { get; set; } = 1f;

    /// <summary>
    /// Sets the maximum distance between the Camera and centre of [AABB].
    /// </summary>
    [Export]
    public float AutoFollowDistanceMax { get; set; } = 5f;

    /// <summary>
    /// Determines how fast the [member auto_follow_distance] moves between the
    /// maximum and minimum distance. The higher the value, the sooner the
    /// maximum distance is reached.<br/>
    /// This value should be based on the sizes of the [member auto_follow_distance_min]
    /// and [member auto_follow_distance_max].<br/>
    /// E.g. if the value between the [member auto_follow_distance_min] and
    /// [member auto_follow_distance_max] is small, consider keeping the number low
    /// and vice versa.
    /// </summary>
    [Export]
    public float AutoFollowDistanceDivisor { get; set; } = 10f;

    public override void _ValidateProperty(Dictionary property)
    {
        base._ValidateProperty(property);

        if (property["name"].AsStringName() == "FollowDistance" && AutoFollowDistance)
            property["usage"] = (int)PropertyUsageFlags.NoEditor;

        if (!AutoFollowDistance)
            switch (property["name"].AsStringName())
            {
                case "AutoFollowDistanceMin":
                case "AutoFollowDistanceMax":
                case "AutoFollowDistanceDivisor":
                    property["usage"] = (int)PropertyUsageFlags.NoEditor;
                    break;
            }
    }

    public override Vector3 ProcessLogic(Transform3D[] targetsTransforms, Transform3D cameraTransform, double delta)
    {
        Vector3 followPosition = cameraTransform.Origin;

        if (targetsTransforms.Length == 1)
            followPosition = targetsTransforms[0].Origin + FollowOffset + cameraTransform.Basis.Z * new Vector3(FollowDistance, FollowDistance, FollowDistance);
        else
        {
            Aabb bounds = new(targetsTransforms[0].Origin, Vector3.Zero);

            foreach (var transform in targetsTransforms)
            {
                bounds = bounds.Expand(transform.Origin);
            }

            float distance = FollowDistance;

            if (AutoFollowDistance)
            {
                distance = Mathf.Lerp(AutoFollowDistanceMin, AutoFollowDistanceMax, bounds.GetLongestAxisSize() / AutoFollowDistanceDivisor);
                distance = Mathf.Clamp(distance, AutoFollowDistanceMin, AutoFollowDistanceMax);
            }

            followPosition = bounds.GetCenter() + FollowOffset + cameraTransform.Basis.Z * new Vector3(distance, distance, distance);
        }

        return interpolatePosition(followPosition, cameraTransform.Origin, delta);
    }
}
