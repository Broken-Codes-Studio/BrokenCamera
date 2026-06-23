/*#if TOOLS
namespace Addon.BrokenCamera.Editor.Gizmos;

using Godot;

using Addon.BrokenCamera.Cameras;

[Tool]
public partial class BrokenCameraGizmoPlugin3D : CustomPluginGizmo<BrokenCamera3D>
{
	//TODO: Add custom Icon
	const string ICON_PATH = "uid://dkiefpjsrj37n";

	public BrokenCameraGizmoPlugin3D()
	{
		gizmo_icon = GD.Load<Texture2D>(ICON_PATH);
		ResourceName = "BrokenCamera";
		HandleMaterials();
	}

	public override string _GetGizmoName() => "BrokenCamera";

}
#endif*/