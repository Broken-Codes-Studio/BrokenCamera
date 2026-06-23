namespace Addon.BrokenCamera.Host;

using Godot;

using System;

using Managers;
using Cameras;

/// <summary>
/// Manages the 3D Broken Camera host, ensuring only one active camera at a time and handling camera transitions.
/// </summary>
[GlobalClass]
public partial class BrokenCameraHost3D : BrokenCameraHost<BrokenCamera3D>
{
    #region Variables
    public Camera3D camera3D { get; protected set; }

    protected Transform3D activeBcam3DGlobTransform = new();
    protected Transform3D prevActiveBcam3DTransform = new();
    protected bool cam3DPositionChanged = false;
    protected bool cam3DRotationChanged = false;

    protected float prevCamHOffset = 0f;
    protected bool camHOffsetChanged = false;

    protected float prevCamVOffset = 0f;
    protected bool camVOffsetChanged = false;

    protected float prevCamFov = 75f;
    protected bool camFovChanged = false;

    protected float prevCamSize = 1f;
    protected bool camSizeChanged = false;

    protected Vector2 prevCamFrustumOffset = Vector2.Zero;
    protected bool camFrustumOffsetChanged = false;

    protected float prevCamNear = 0.05f;
    protected bool camNearChanged = false;

    protected float prevCamFar = 4000f;
    protected bool camFarChanged = false;
    #endregion

    /// <summary>
    /// Returns configuration warnings if the host is not a child of a Camera3D.
    /// </summary>
    public override string[] _GetConfigurationWarnings()
    {
        Camera3D parentCamera = GetParentOrNull<Camera3D>();

        if (parentCamera is null)
            return new[] { "Needs to be a child of a Camera3D in order to work." };
        else
            return Array.Empty<string>();
    }

    /// <summary>
    /// Called when the node enters the scene tree.
    /// </summary>
    public override void _EnterTree()
    {
        base._Ready();

        Camera3D parentCamera = GetParentOrNull<Camera3D>();

        if (parentCamera is null)
            return;

        isChildOfCamera = true;

        camera3D = parentCamera;

        BrokenCameraManager3D.Instance.BcamHostAdded(this);

        checkCameraHostAmount();

        if (multipleBcamHost)
        {
            GD.PrintErr(
                "Only one BrokenCameraHost can exist in a scene \n Multiple BrokenCameraHosts will be supported in https://github.com/ramokz/broken-camera/issues/26");
            QueueFree();
        }

        BrokenCamera3D[] BrokenCamera3Ds = BrokenCameraManager3D.Instance.BrokenCameras;
        if (BrokenCamera3Ds.Length > 0)
            foreach (BrokenCamera3D bcam3D in BrokenCamera3Ds)
            {
                BcamAddedToScene(bcam3D);
                bcam3D.BcamHostOwner = this;
            }
    }

    /// <summary>
    /// Called when the node exits the scene tree.
    /// </summary>
    public override void _ExitTree()
    {
        BrokenCameraManager3D.Instance.BcamHostRemoved(this);
        checkCameraHostAmount();
    }

    /// <summary>
    /// Called when the node is ready.
    /// </summary>
    public override void _Ready()
    {
        if (!IsInstanceValid(ActiveBcam))
            return;

        activeBcam3DGlobTransform = ActiveBcam.GlobalTransform;
    }

    protected override void tweenFollowChecker(double delta)
    {
        if (IsInstanceValid(ActiveBcam))
            activeBcam3DGlobTransform = ActiveBcam.GlobalTransform;

        base.tweenFollowChecker(delta);
    }

    /// <summary>
    /// Checks if the camera host amount exceeds one.
    /// </summary>
    protected void checkCameraHostAmount()
    {
        multipleBcamHost = BrokenCameraManager3D.Instance.BrokenCameraHosts.Length > 1;
    }

    /// <summary>
    /// Assigns a new active Broken Camera.
    /// </summary>
    protected override void assignNewActiveBcam(BrokenCamera3D bcam)
    {
        bool noPreviousBcam = false;

        if (IsInstanceValid(ActiveBcam) && !activeBcamMissing)
        {
            prevActiveBcam3DTransform = camera3D.GlobalTransform;

            prevCamHOffset = camera3D.HOffset;
            prevCamVOffset = camera3D.VOffset;
            prevCamFov = camera3D.Fov;
            prevCamSize = camera3D.Size;
            prevCamFrustumOffset = camera3D.FrustumOffset;
            prevCamNear = camera3D.Near;
            prevCamFar = camera3D.Far;

            ActiveBcam.Active = false;

            if (triggerBcamTween)
                ActiveBcam.EmitSignal(BrokenCamera3D.SignalName.TweenInterrupted, bcam);
        }
        else
            noPreviousBcam = true;

        ActiveBcam = bcam;
        activeBcamPriority = ActiveBcam.Priority;
        tweenDuration = ActiveBcam.TweenDuration;

        if (ActiveBcam.camera3DResource is not null)
        {
            if (noPreviousBcam)
            {
                camera3D.HOffset = ActiveBcam.H_Offset;
                camera3D.VOffset = ActiveBcam.V_Offset;
                camera3D.Fov = ActiveBcam.FOV;
                camera3D.Size = ActiveBcam.Size;
                camera3D.FrustumOffset = ActiveBcam.FrustumOffset;
                camera3D.Near = ActiveBcam.Near;
                camera3D.Far = ActiveBcam.Far;
            }
            else
            {
                if (prevCamHOffset != ActiveBcam.H_Offset)
                    camHOffsetChanged = true;
                if (prevCamVOffset != ActiveBcam.V_Offset)
                    camVOffsetChanged = true;
                if (prevCamFov != ActiveBcam.FOV)
                    camFovChanged = true;
                if (prevCamSize != ActiveBcam.Size)
                    camSizeChanged = true;
                if (prevCamFrustumOffset != ActiveBcam.FrustumOffset)
                    camFrustumOffsetChanged = true;
                if (prevCamNear != ActiveBcam.Near)
                    camNearChanged = true;
                if (prevCamFar != ActiveBcam.Far)
                    camFarChanged = true;
            }

            camera3D.CullMask = ActiveBcam.CullMask;
            camera3D.Projection = ActiveBcam.Projection;
        }

        ActiveBcam.Active = true;

        if (noPreviousBcam)
            prevActiveBcam3DTransform = ActiveBcam.GlobalTransform;
        else
        {
            if (prevActiveBcam3DTransform.Origin != ActiveBcam.GlobalPosition)
                cam3DPositionChanged = true;
            if (prevActiveBcam3DTransform.Basis.GetRotationQuaternion() != ActiveBcam.Quaternion)
                cam3DRotationChanged = true;
        }

        if (bcam.TweenSkip)
            tweenElapsedTime = bcam.TweenDuration;
        else
            tweenElapsedTime = 0;

        triggerBcamTween = !noPreviousBcam;
    }

    /// <summary>
    /// Follows the active Broken Camera.
    /// </summary>
    protected override void bcamFollow(double delta)
    {
        if (!IsInstanceValid(ActiveBcam))
            return;

        if (activeBcamMissing || !isChildOfCamera)
            return;

        camera3D.GlobalTransform = activeBcam3DGlobTransform;
    }

    /// <summary>
    /// Handles the tweening of the camera.
    /// </summary>
    protected override void bcamTween(double delta)
    {
        if (tweenElapsedTime == 0)
            ActiveBcam.EmitSignal(BrokenCamera3D.SignalName.TweenStarted);

        tweenElapsedTime = (float)Mathf.Min(tweenDuration, tweenElapsedTime + delta);

        ActiveBcam.EmitSignal(BrokenCamera3D.SignalName.IsTweening);
        if (cam3DPositionChanged)
        {
            camera3D.GlobalPosition = Tween.InterpolateValue(
                    prevActiveBcam3DTransform.Origin,
                    activeBcam3DGlobTransform.Origin - prevActiveBcam3DTransform.Origin,
                    tweenElapsedTime,
                    ActiveBcam.TweenDuration,
                    ActiveBcam.TweenTransition,
                    ActiveBcam.TweenEase
                ).AsVector3();
        }

        if (cam3DRotationChanged)
        {
            Quaternion prevActiveBcam3DQuat = new(prevActiveBcam3DTransform.Basis.Orthonormalized());
            camera3D.Quaternion = Tween.InterpolateValue(
                prevActiveBcam3DQuat,
                prevActiveBcam3DQuat.Inverse() * new Quaternion(activeBcam3DGlobTransform.Basis.Orthonormalized()),
                tweenElapsedTime,
                ActiveBcam.TweenDuration,
                ActiveBcam.TweenTransition,
                ActiveBcam.TweenEase
            ).AsQuaternion();
        }

        if (camHOffsetChanged)
        {
            camera3D.HOffset = Tween.InterpolateValue(
                prevCamHOffset,
                ActiveBcam.H_Offset - prevCamHOffset,
                tweenElapsedTime,
                ActiveBcam.TweenDuration,
                ActiveBcam.TweenTransition,
                ActiveBcam.TweenEase
            ).As<float>();
        }
        if (camVOffsetChanged)
        {
            camera3D.VOffset = Tween.InterpolateValue(
                prevCamVOffset,
                ActiveBcam.V_Offset - prevCamVOffset,
                tweenElapsedTime,
                ActiveBcam.TweenDuration,
                ActiveBcam.TweenTransition,
                ActiveBcam.TweenEase
            ).As<float>();
        }
        if (camFovChanged)
        {
            camera3D.Fov = Tween.InterpolateValue(
                prevCamFov,
                ActiveBcam.FOV - prevCamFov,
                tweenElapsedTime,
                ActiveBcam.TweenDuration,
                ActiveBcam.TweenTransition,
                ActiveBcam.TweenEase
            ).As<float>();
        }
        if (camSizeChanged)
        {
            camera3D.Size = Tween.InterpolateValue(
                prevCamSize,
                ActiveBcam.Size - prevCamSize,
                tweenElapsedTime,
                ActiveBcam.TweenDuration,
                ActiveBcam.TweenTransition,
                ActiveBcam.TweenEase
            ).As<float>();
        }
        if (camFrustumOffsetChanged)
        {
            camera3D.FrustumOffset = Tween.InterpolateValue(
                prevCamFrustumOffset,
                ActiveBcam.FrustumOffset - prevCamFrustumOffset,
                tweenElapsedTime,
                ActiveBcam.TweenDuration,
                ActiveBcam.TweenTransition,
                ActiveBcam.TweenEase
            ).As<Vector2>();
        }
        if (camNearChanged)
        {
            camera3D.Near = Tween.InterpolateValue(
                prevCamNear,
                ActiveBcam.Near - prevCamNear,
                tweenElapsedTime,
                ActiveBcam.TweenDuration,
                ActiveBcam.TweenTransition,
                ActiveBcam.TweenEase
            ).As<float>();
        }
        if (camFarChanged)
        {
            camera3D.Far = Tween.InterpolateValue(
                prevCamFar,
                ActiveBcam.Far - prevCamFar,
                tweenElapsedTime,
                ActiveBcam.TweenDuration,
                ActiveBcam.TweenTransition,
                ActiveBcam.TweenEase
            ).As<float>();
        }

        if (tweenElapsedTime < tweenDuration)
            return;

        triggerBcamTween = false;
        tweenElapsedTime = 0;

        cam3DPositionChanged = false;
        cam3DRotationChanged = false;

        camHOffsetChanged = false;
        camVOffsetChanged = false;
        camFovChanged = false;
        camSizeChanged = false;
        camFrustumOffsetChanged = false;
        camNearChanged = false;
        camFarChanged = false;

        ActiveBcam.EmitSignal(BrokenCamera3D.SignalName.TweenCompleted);
    }
}