using Interfaces.Utility;

namespace Addon.BrokenCamera.Managers;

using Godot;

using System.Collections.Generic;

using BrokenSigilCollection.Interface;
using Interfaces;
using Host;

#if TOOLS
[Tool]
#endif
/// <summary>
/// Abstract base class for managing Broken Cameras and their hosts.
/// </summary>
/// <typeparam name="T">The type of Broken Camera.</typeparam>
/// <typeparam name="TH">The type of Broken Camera Host.</typeparam>
public abstract partial class BrokenCameraManager<T, TH> : Node, ISingleton<BrokenCameraManager<T, TH>> where T : Node, IFunctional, IBrokenCamera where TH : BrokenCameraHost<T>
{
    private List<TH> _brokenCameraHosts = new();
    /// <summary>
    /// Array of Broken Camera Hosts.
    /// </summary>
    public TH[] BrokenCameraHosts => _brokenCameraHosts.ToArray();

    private List<T> _brokenCameras = new();
    /// <summary>
    /// Array of Broken Cameras.
    /// </summary>
    public T[] BrokenCameras => _brokenCameras.ToArray();

    public override void _EnterTree()
    {
        Engine.PhysicsJitterFix = 0;
    }

    /// <summary>
    /// Adds a Broken Camera Host to the manager.
    /// </summary>
    /// <param name="caller">The Broken Camera Host to add.</param>
    public void BcamHostAdded(TH caller)
    {
        _brokenCameraHosts.Add(caller);
    }

    /// <summary>
    /// Removes a Broken Camera Host from the manager.
    /// </summary>
    /// <param name="caller">The Broken Camera Host to remove.</param>
    public void BcamHostRemoved(TH caller)
    {
        _brokenCameraHosts.Remove(caller);
    }

    /// <summary>
    /// Adds a Broken Camera to the manager.
    /// </summary>
    /// <param name="caller">The Broken Camera to add.</param>
    /// <param name="hostSlot">The host slot index.</param>
    public void BcamAdded(T caller, int hostSlot = 0)
    {
        _brokenCameras.Add(caller);

        // if(BrokenCameraHosts.Count > 0)
        //    BrokenCameraHosts[hostSlot].BcamAddedToScene(caller);
    }

    /// <summary>
    /// Removes a Broken Camera from the manager.
    /// </summary>
    /// <param name="caller">The Broken Camera to remove.</param>
    public void BcamRemoved(T caller)
    {
        _brokenCameras.Remove(caller);
    }

    /// <summary>
    /// Clears the lists of Broken Camera Hosts and Broken Cameras.
    /// </summary>
    public void SceneChanged()
    {
        _brokenCameraHosts.Clear();
        _brokenCameras.Clear();
    }
}