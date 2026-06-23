namespace Addon.BrokenCamera.Resources;

using Godot;
using Godot.Collections;

/// <summary>
/// Resource for BrokenCamera3D to override various Camera3D properties.
/// </summary>
[GlobalClass]
public partial class Camera3DResource : Resource
{

    /*  Resource for [BrokenCamera3D] to override various [Camera3D] properties.

        The overrides defined here will be applied to the [Camera3D] upon the
        [BrokenCamera3D] becoming active.
    */

    /// <summary>
    /// Overrides [member Camera3D.cull_mask].
    /// </summary>
    [Export(PropertyHint.Layers3DRender)]
    public uint CullMask = 1048575;

    /// <summary>
    /// Overrides [member Camera3D.h_offset].
    /// </summary>
    [Export(PropertyHint.Range, "0,1,0.001,hide_slider,suffix:m")]
    public float H_Offset = 0f;
    /// <summary>
    /// Overrides [member Camera3D.v_offset].
    /// </summary>
    [Export(PropertyHint.Range, "0,1,0.001,hide_slider,suffix:m")]
    public float V_Offset = 0f;

    private Camera3D.ProjectionType _projection = Camera3D.ProjectionType.Perspective;
    /// <summary>
    /// Overrides [member Camera3D.projection].
    /// </summary>
    [Export]
    public Camera3D.ProjectionType Projection
    {
        get => _projection;
        set
        {
            _projection = value;
            NotifyPropertyListChanged();
        }
    }

    /// <summary>
    /// Overrides [member Camera3D.fov].
    /// </summary>
    [Export(PropertyHint.Range, "1,179,0.1,degrees")]
    public float FOV = 75f;

    /// <summary>
    /// Overrides [member Camera3D.size].
    /// </summary>
    [Export(PropertyHint.Range, "0.001,100,0.001,suffix:m,or_greater")]
    public float Size = 1f;

    /// <summary>
    /// Overrides [member Camera3d.frustum_offset].
    /// </summary>
    [Export]
    public Vector2 FrustumOffset = Vector2.Zero;

    /// <summary>
    /// Overrides [member Camera3D.near].
    /// </summary>
    [Export(PropertyHint.Range, "0.001,10,0.001,suffix:m,or_greater")]
    public float Near = 0.05f;

    /// <summary>
    /// Overrides [member Camera3D.far].
    /// </summary>
    [Export(PropertyHint.Range, "0.01,4000,0.001,suffix:m,or_greater")]
    public float Far = 4000f;

    public Camera3DResource() { }

#if TOOLS
    /// <summary>
    /// Validates the property based on the projection type.
    /// </summary>
    public override void _ValidateProperty(Dictionary property)
    {
        if (property["name"].AsStringName() == "fov" && Projection != Camera3D.ProjectionType.Perspective)
            property["usage"] = (int)PropertyUsageFlags.NoEditor;

        if (property["name"].AsStringName() == "size" && Projection == Camera3D.ProjectionType.Perspective)
            property["usage"] = (int)PropertyUsageFlags.NoEditor;

        if (property["name"].AsStringName() == "frustum_offset" && Projection != Camera3D.ProjectionType.Frustum)
            property["usage"] = (int)PropertyUsageFlags.NoEditor;
    }
#endif

}