namespace Addon.BrokenCamera.Host;

using Godot;
using System;

using System.Collections.Generic;
using BrokenSigilCollection;
using BrokenSigilCollection.Interface;
using Interfaces;

/// <summary>
/// Abstract base class for managing Broken Camera hosts.
/// </summary>
public abstract partial class BrokenCameraHost<T> : Node where T : Node, IFunctional, IBrokenCamera
{
    [Export]
    public ProcessType processType { get; protected set; } = ProcessType.Frame;

    [Export]
    public bool IgnoreTimeScale = false;

    public T ActiveBcam { get; protected set; } = null;

    protected List<T> bcamList = new();

    protected int activeBcamPriority = -1;
    protected bool activeBcamMissing = true;

    public bool triggerBcamTween { get; protected set; } = false;
    protected float tweenElapsedTime = 0f;
    protected float tweenDuration = 0f;
    protected float tweenTransition = 0f;
    protected int tweenEase = 2;

    protected bool isChildOfCamera = false;
    protected bool multipleBcamHost = false;

    DateTime lastUpdated = DateTime.Now;

    #region Override Methods
    /// <summary>
    /// Called every frame if processType is set to Frame.
    /// </summary>
    public override void _Process(double delta)
    {
        if (processType is ProcessType.Frame)
            Perform(delta);
    }

    /// <summary>
    /// Called every physics frame if processType is set to Physics.
    /// </summary>
    public override void _PhysicsProcess(double delta)
    {
        if (processType is ProcessType.Physics)
            Perform(delta);
    }
    public bool Perform(double delta)
    {
        double currectDelta = delta;

        if (IgnoreTimeScale)
        {
            var now = DateTime.Now;
            var unscaledDelta = (now - lastUpdated).TotalSeconds;
            lastUpdated = now;

            currectDelta = unscaledDelta;
        }

        tweenFollowChecker(currectDelta);

        return true;
    }

    #endregion

    /// <summary>
    /// Adds a Broken Camera to the scene.
    /// </summary>
    public void BcamAddedToScene(T bcam)
    {
        if (!bcamList.Contains(bcam))
        {
            bcamList.Add(bcam);

            findBcamWithHighestPriority();
        }
    }

    /// <summary>
    /// Removes a Broken Camera from the scene.
    /// </summary>
    public void BcamRemovedFromScene(T bcam)
    {
        if (bcamList.Contains(bcam))
        {
            bcamList.Remove(bcam);

            if (bcam == ActiveBcam)
            {
                activeBcamMissing = true;
                activeBcamPriority = -1;
                findBcamWithHighestPriority();
            }
        }
    }

    /// <summary>
    /// Updates the priority of a Broken Camera.
    /// </summary>
    public void BcamPriorityUpdated(T bcam)
    {
        if (!IsInstanceValid(bcam))
            return;

        int currentBcamPriority = bcam.Priority;

        if (currentBcamPriority >= activeBcamPriority)
        {
            if (bcam != ActiveBcam)
                assignNewActiveBcam(bcam);
        }
        if (bcam == ActiveBcam)
        {
            if (currentBcamPriority < activeBcamPriority)
            {
                activeBcamPriority = currentBcamPriority;
                findBcamWithHighestPriority();
            }
            else
                activeBcamPriority = currentBcamPriority;
        }
    }

    /// <summary>
    /// Refreshes the priority of the Broken Camera list.
    /// </summary>
    public void RefreshBcamListPriority()
    {
        activeBcamPriority = -1;
        findBcamWithHighestPriority();
    }

    /// <summary>
    /// Checks if the camera should follow or tween.
    /// </summary>
    protected virtual void tweenFollowChecker(double delta)
    {
        if (triggerBcamTween)
            bcamTween(delta);
        else
            bcamFollow(delta);
    }

    /// <summary>
    /// Finds the Broken Camera with the highest priority.
    /// </summary>
    protected virtual void findBcamWithHighestPriority()
    {
        foreach (T bcam in bcamList)
        {
            if (!bcam.Visibility)
                continue;
            if (bcam.Priority > activeBcamPriority)
            {
                assignNewActiveBcam(bcam);
            }
            bcam.TweenSkip = false;
            activeBcamMissing = false;
        }
    }

    #region Abstract Methods
    protected abstract void assignNewActiveBcam(T bcam);

    protected abstract void bcamTween(double delta);
    protected abstract void bcamFollow(double delta);
    #endregion
}

