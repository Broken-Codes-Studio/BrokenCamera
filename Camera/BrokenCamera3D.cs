namespace Addon.BrokenCamera.Cameras;

using Godot;

using BrokenSigilCollection;
using BrokenSigilCollection.Interface;

using Managers;
using Host;
using Interfaces;
using Resources;

/// <summary>
/// Abstract base class for 3D Broken Cameras.
/// </summary>
public partial class BrokenCamera3D : Node3D, IFunctional, IBrokenCamera
{

    #region Signals
    /// <summary>
    /// Emitted when the [param BrokenCamera2D] becomes active.
    /// </summary>
    [Signal]
    public delegate void BecameActiveEventHandler();
    /// <summary>
    /// Emitted when the [param BrokenCamera2D] becomes inactive.
    /// </summary>
    [Signal]
    public delegate void BecameInactiveEventHandler();
    /// <summary>
    /// Emitted when the [param Camera3D] starts to tween to another [param BrokenCamera3D].
    /// </summary>
    [Signal]
    public delegate void TweenStartedEventHandler();

    /// <summary>
    /// Emitted when the [param Camera3D] is to tweening towards another [param BrokenCamera3D].
    /// </summary>
    [Signal]
    public delegate void IsTweeningEventHandler();

    /// <summary>
    /// Emitted when the tween is interrupted due to another [param BrokenCamera3D]
    /// becoming active. The argument is the [param BrokenCamera3D] that interrupted the tween.
    /// </summary>
    [Signal]
    public delegate void TweenInterruptedEventHandler(BrokenCamera3D bcam3D);

    /// <summary>
    /// Emitted when the [param Camera3D] completes its tween to the [param BrokenCamera3D].
    /// </summary>
    [Signal]
    public delegate void TweenCompletedEventHandler();
    #endregion

    /// <summary>
    /// The Camera3D instance associated with this BrokenCamera3D.
    /// </summary>
    public Camera3D camera3D { get; protected set; } = null;

    private BrokenCameraHost3D _bcamHostOwner = null;
    /// <summary>
    /// The host owner of this BrokenCamera3D.
    /// </summary>
    public BrokenCameraHost3D BcamHostOwner
    {
        get => _bcamHostOwner;
        set
        {
            _bcamHostOwner = value;
            if (IsInstanceValid(BcamHostOwner))
                BcamHostOwner.BcamAddedToScene(this);
        }
    }

    private bool _active = false;
    /// <summary>
    /// Indicates whether this BrokenCamera3D is active.
    /// </summary>
    public bool Active
    {
        get => _active;
        set
        {
            _active = value;

            if (value)
                EmitSignal(SignalName.BecameActive);
            else
                EmitSignal(SignalName.BecameInactive);
        }
    }

    private int _priority = 0;
    /// <summary>
    /// The priority of this BrokenCamera3D.
    /// </summary>
    [Export]
    public int Priority
    {
        get => _priority;
        set
        {
            _priority = value < 0 ? 0 : value;
            if (_hasValidBcamOwner())
                BcamHostOwner.BcamPriorityUpdated(this);
        }
    }
    /// <summary>
    /// The visibility of this BrokenCamera3D.
    /// </summary>
    public bool Visibility { get => Visible; set => Visible = value; }

    #region TweenResource property getters
    /// <summary>
    /// The TweenResource associated with this BrokenCamera3D.
    /// </summary>
    [Export]
    public TweenResource tweenResource = new();
    /// <summary>
    /// Indicates whether to skip the tween.
    /// </summary>
    [Export]
    public bool TweenSkip { get; set; } = false;
    /// <summary>
    /// The duration of the tween.
    /// </summary>
    public float TweenDuration => tweenResource.Duration;
    /// <summary>
    /// The transition type of the tween.
    /// </summary>
    public Tween.TransitionType TweenTransition => tweenResource.Transition;
    /// <summary>
    /// The ease type of the tween.
    /// </summary>
    public Tween.EaseType TweenEase => tweenResource.Ease;

    #endregion

    #region Camera3DResouce property getters

    /// <summary>
    /// The Camera3DResource associated with this BrokenCamera3D.
    /// </summary>
    [Export]
    public Camera3DResource camera3DResource { get; set; } = new();

    /// <summary>
    /// The cull mask of the Camera3D.
    /// </summary>
    public uint CullMask
    {
        get => camera3DResource.CullMask;
        set
        {
            camera3DResource.CullMask = value;
            if (Active)
                BcamHostOwner.camera3D.CullMask = value;
        }
    }

    /// <summary>
    /// The horizontal offset of the Camera3D.
    /// </summary>
    public float H_Offset
    {
        get => camera3DResource.H_Offset;
        set
        {
            camera3DResource.H_Offset = value;
            if (Active)
                BcamHostOwner.camera3D.HOffset = value;
        }
    }

    /// <summary>
    /// The vertical offset of the Camera3D.
    /// </summary>
    public float V_Offset
    {
        get => camera3DResource.V_Offset;
        set
        {
            camera3DResource.V_Offset = value;
            if (Active)
                BcamHostOwner.camera3D.VOffset = value;
        }
    }

    /// <summary>
    /// The projection type of the Camera3D.
    /// </summary>
    public Camera3D.ProjectionType Projection
    {
        get => camera3DResource.Projection;
        set
        {
            camera3DResource.Projection = value;
            //if(IsActive)
            //BcamHostOwner.camera3D.Projection = value;
        }
    }

    /// <summary>
    /// The field of view of the Camera3D.
    /// </summary>
    public float FOV
    {
        get => camera3DResource.FOV;
        set
        {
            camera3DResource.FOV = value;
            if (Active)
                BcamHostOwner.camera3D.Fov = value;
        }
    }

    /// <summary>
    /// The size of the Camera3D.
    /// </summary>
    public float Size
    {
        get => camera3DResource.Size;
        set
        {
            camera3DResource.Size = value;
            if (Active)
                BcamHostOwner.camera3D.Size = value;
        }
    }

    /// <summary>
    /// The frustum offset of the Camera3D.
    /// </summary>
    public Vector2 FrustumOffset
    {
        get => camera3DResource.FrustumOffset;
        set
        {
            camera3DResource.FrustumOffset = value;
            if (Active)
                BcamHostOwner.camera3D.FrustumOffset = value;
        }
    }

    /// <summary>
    /// The far clipping distance of the Camera3D.
    /// </summary>
    public float Far
    {
        get => camera3DResource.Far;
        set
        {
            camera3DResource.Far = value;
            if (Active)
                BcamHostOwner.camera3D.Far = value;
        }
    }

    /// <summary>
    /// The near clipping distance of the Camera3D.
    /// </summary>
    public float Near
    {
        get => camera3DResource.Near;
        set
        {
            camera3DResource.Near = value;
            if (Active)
                BcamHostOwner.camera3D.Near = value;
        }
    }

    #endregion

    #region Logic Resources

    [ExportGroup("Follow")]
    private Node3D[] _rollowTargetNodes = System.Array.Empty<Node3D>();
    [Export]
    public Node3D[] FollowTargetNodes
    {
        get => _rollowTargetNodes;
        set
        {
            _rollowTargetNodes = value;

            if (value is null)
            {
                followTargets = System.Array.Empty<Transform3D>();
                return;
            }

            followTargets = new Transform3D[value.Length];

            for (short i = 0; i < value.Length; i++)
            {
                followTargets[i] = value[i].IsInsideTree() ? value[i].GlobalTransform : value[i].Transform;
            }
        }
    }
    protected Transform3D[] followTargets = System.Array.Empty<Transform3D>();
    [Export]
    public FollowMode3D FollowMode { get; protected set; } = null;

    [ExportGroup("Look at")]
    private Node3D[] _lookAtTargetNodes = System.Array.Empty<Node3D>();
    [Export]
    public Node3D[] LookAtTargetNodes
    {
        get => _lookAtTargetNodes;
        set
        {
            _lookAtTargetNodes = value;

            if (value is null)
            {
                lookAtTargets = System.Array.Empty<Transform3D>();
                return;
            }

            lookAtTargets = new Transform3D[value.Length];

            for (short i = 0; i < value.Length; i++)
            {
                lookAtTargets[i] = value[i].IsInsideTree() ? value[i].GlobalTransform : value[i].Transform;
            }
        }
    }
    protected Transform3D[] lookAtTargets = System.Array.Empty<Transform3D>();
    [Export]
    public LookAtMode3D LookAtMode { get; protected set; } = null;

    protected Transform3D rawTransform;

    #endregion

    /// <summary>
    /// The process type of this BrokenCamera3D.
    /// </summary>
    [ExportCategory("Process")]
    [Export]
    public ProcessType processType { get; protected set; } = ProcessType.Frame;

    #region Override methods


    /// <summary>
    /// Called when the node enters the scene tree.
    /// </summary>
    public override void _EnterTree()
    {
        BrokenCameraManager3D.Instance.BcamAdded(this);

        if (BrokenCameraManager3D.Instance.BrokenCameraHosts.Length > 0)
            BcamHostOwner = BrokenCameraManager3D.Instance.BrokenCameraHosts[0];

        followTargets = new Transform3D[FollowTargetNodes.Length];
        for (short i = 0; i < FollowTargetNodes.Length; i++)
        {
            followTargets[i] = FollowTargetNodes[i].IsInsideTree() ? FollowTargetNodes[i].GlobalTransform : FollowTargetNodes[i].Transform;
        }

        lookAtTargets = new Transform3D[LookAtTargetNodes.Length];
        for (short i = 0; i < LookAtTargetNodes.Length; i++)
        {
            lookAtTargets[i] = LookAtTargetNodes[i].IsInsideTree() ? LookAtTargetNodes[i].GlobalTransform : LookAtTargetNodes[i].Transform;
        }

        if (IsInsideTree())
            rawTransform = GlobalTransform;
        else
            rawTransform = Transform;
    }

    /// <summary>
    /// Called when the node exits the scene tree.
    /// </summary>
    public override void _ExitTree()
    {
        BrokenCameraManager3D.Instance.BcamRemoved(this);

        if (_hasValidBcamOwner())
            BcamHostOwner.BcamRemovedFromScene(this);
    }

    /// <summary>
    /// Called every frame. 'delta' is the elapsed time since the previous frame.
    /// </summary>
    public override void _Process(double delta)
    {
        if (processType is ProcessType.Frame)
            Perform(delta);
    }

    /// <summary>
    /// Called every physics frame. 'delta' is the elapsed time since the previous physics frame.
    /// </summary>
    public override void _PhysicsProcess(double delta)
    {
        if (processType is ProcessType.Physics)
            Perform(delta);
    }

    public bool Perform(double delta)
    {

        for (short i = 0; i < FollowTargetNodes.Length; i++)
        {
            followTargets[i] = FollowTargetNodes[i].IsInsideTree() ? FollowTargetNodes[i].GlobalTransform : FollowTargetNodes[i].Transform;
        }

        for (short i = 0; i < LookAtTargetNodes.Length; i++)
        {
            lookAtTargets[i] = LookAtTargetNodes[i].IsInsideTree() ? LookAtTargetNodes[i].GlobalTransform : LookAtTargetNodes[i].Transform;
        }

        Vector3 position = GlobalPosition;
        Quaternion rotation = rawTransform.Basis.GetRotationQuaternion();

        if (FollowMode is not null && followTargets is not null)
            position = FollowMode.ProcessLogic(followTargets, rawTransform, delta);

        if (LookAtMode is not null && lookAtTargets is not null)
            rotation = LookAtMode.ProcessLogic(lookAtTargets, rawTransform, delta);

        rawTransform.Origin = position;
        rawTransform.Basis = new(rotation);

        GlobalTransform = rawTransform;

        return true;
    }

    #endregion

    /// <summary>
    /// Checks if the BrokenCameraHost3D owner is valid.
    /// </summary>
    /// <returns>True if the owner is valid, false otherwise.</returns>
    protected bool _hasValidBcamOwner()
    {
        if (BcamHostOwner == null)
            return false;
        if (!IsInstanceValid(BcamHostOwner))
            return false;
        if (!IsInstanceValid(BcamHostOwner.camera3D))
            return false;
        return true;
    }

}