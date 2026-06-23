namespace Addon.BrokenCamera.Resources;

using Godot;
using Godot.Collections;

[GlobalClass]
public abstract partial class DampingFollow3D : FollowMode3D
{

    /// <summary>
    /// Offsets the target position.
    /// </summary>
    [Export]
    public Vector3 FollowOffset { get; set; } = Vector3.Zero;

    private bool _followDamping = false;
    /// <summary>
    /// Applies a damping effect on the camera's movement.
    /// Leading to heavier / slower camera movement as the targeted node moves around.
    /// This is useful to avoid sharp and rapid camera movement.
    /// </summary>
    [Export]
    public bool FollowDamping
    {
        get => _followDamping;
        set
        {
            _followDamping = value;
            NotifyPropertyListChanged();
        }
    }

    private Vector3 _followDampingValue = new(.1f, .1f, .1f);
    /// <summary>
    /// Defines the damping amount. The ideal range should be somewhere between 0-1.<br/>
    /// The damping amount can be specified in the individual axis.<br/>
    /// <b>Lower value</b> = faster / sharper camera movement.<br/>
    /// <b>Higher value</b> = slower / heavier camera movement.
    /// </summary>
    [Export]
    public Vector3 FollowDampingValue
    {
        get => _followDampingValue;
        set
        {
            var theValue = value;
            theValue.X = value.X < 0 ? 0 : value.X;
            theValue.Y = value.Y < 0 ? 0 : value.Y;
            theValue.Z = value.Z < 0 ? 0 : value.Z;

            _followDampingValue = theValue;
        }
    }

    private Vector3 _followVelocityRef = Vector3.Zero;

    public override void _ValidateProperty(Dictionary property)
    {
        if (property["name"].AsStringName() == "FollowDampingValue" && !FollowDamping)
            property["usage"] = (int)PropertyUsageFlags.NoEditor;
    }

    protected Vector3 interpolatePosition(Vector3 targetsPositions, Vector3 cameraPosition, double delta)
    {
        if (FollowDamping)
        {
            Vector3 position = cameraPosition;
            for (int index = 0; index < 3; index++)
            {
                position[index] = smoothDamp(
                    targetsPositions[index],
                    cameraPosition[index],
                    index,
                    _followVelocityRef[index],
                    new(this, MethodName.setFollowVelocity),
                    FollowDampingValue[index],
                    delta
                );
            }
            return position;
        }
        else
            return targetsPositions;
    }

    protected float smoothDamp(float targetAxis, float selfAxis, int index, float currentVelocity, Callable setVelocity, float dampingTime, double delta)
    {
        dampingTime = Mathf.Max(.0001f, dampingTime);
        float omega = 2 / dampingTime;
        float x = omega * (float)delta;
        float exponential = 1 / (1 + x + .48f * x * x + .235f * x * x * x);
        float diff = selfAxis - targetAxis;
        float _target_Axis = targetAxis;

        float maxChange = Mathf.Inf * dampingTime;
        diff = Mathf.Clamp(diff, -maxChange, maxChange);
        targetAxis = selfAxis - diff;

        float temp = (currentVelocity + omega * diff) * (float)delta;
        setVelocity.Call(index, (currentVelocity - omega * temp) * exponential);
        float output = targetAxis + (diff + temp) * exponential;

        //To prevent overshooting
        if ((_target_Axis - selfAxis > .0f) == (output > _target_Axis))
        {
            output = _target_Axis;
            setVelocity.Call(index, (output - _target_Axis) / delta);
        }

        return output;
    }

    protected void setFollowVelocity(int index, float value)
    {
        _followVelocityRef[index] = value;
    }

}
