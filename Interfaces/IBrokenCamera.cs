using Interfaces.Utility;

namespace Addon.BrokenCamera.Interfaces;

using BrokenSigilCollection.Interface;

/// <summary>
/// Interface for phantom camera functionality.
/// </summary>
public interface IBrokenCamera : IActive, IPriority<int>, ITween, IVisible
{

}
