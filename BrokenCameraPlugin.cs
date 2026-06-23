#if TOOLS
namespace Addon.BrokenCamera;

using Godot;

using System;

//using Editor.Gizmos;

[Tool]
public partial class BrokenCameraPlugin : EditorPlugin
{

	#region Constants
	private const string BCAM_HOST = "BrokenCameraHost";
	private const string BCAM_2D = "BrokenCamera2D";
	private const string BCAM_3D = "BrokenCamera3D";

	private const string CAMERA_3D_RESOURCE = "Camera3DResource";
	private const string TWEEN_RESOURCE = "TweenResource";

	private readonly StringName BROKEN_CAMERA_MANAGER = "BrokenCameraManager";
	#endregion

	#region Variables

	// private BrokenCameraGizmoPlugin3D _bcam3DGizmoPlugin;

	#endregion

	#region Override Methods

	public override void _EnterTree()
	{
		//TODO: Fix and replace Icons
		// Broken Camera Nodes
		//AddCustomType(BCAM_2D,"Node2D",GD.Load<Script>("res://addons/phantom_camera/scripts/phantom camera/BrokenCamera2D.cs"),GD.Load<Texture2D>("res://addons/phantom_camera/icons/phantom_camera_2d.svg"));
		AddCustomType(BCAM_3D, "Node3D", GD.Load<Script>("uid://b4af25ug0h4m5"), GD.Load<Texture2D>("uid://dkiefpjsrj37n"));
		AddCustomType($"{BCAM_HOST}3D", "Node", GD.Load<Script>("uid://b25basoyirlrb"), GD.Load<Texture2D>("uid://dkiefpjsrj37n"));

		AddCustomType(TWEEN_RESOURCE, "Resource", GD.Load<Script>("uid://h4hmebv3okdq"), GD.Load<Texture2D>("uid://r5odxuktty0u"));
		AddCustomType(CAMERA_3D_RESOURCE, "Resource", GD.Load<Script>("uid://lfqy1ys7sfru"), GD.Load<Texture2D>("uid://8e51jf5fna86"));

		#region Follow Mode

		AddCustomType("FollowMode3D", "Resource", GD.Load<Script>("uid://b6ua4aqc0tcvw"), GD.Load<Texture2D>("uid://8e51jf5fna86"));
		AddCustomType("GluedFollow3D", "FollowMode3D", GD.Load<Script>("uid://8aenvuemkpv8"), GD.Load<Texture2D>("uid://8e51jf5fna86"));
		AddCustomType("DampingFollow3D", "FollowMode3D", GD.Load<Script>("uid://bmtb6oadrcl2g"), GD.Load<Texture2D>("uid://8e51jf5fna86"));
		AddCustomType("SimpleFollow3D", "DampingFollow3D", GD.Load<Script>("uid://bk40pkp6vjllo"), GD.Load<Texture2D>("uid://8e51jf5fna86"));
		AddCustomType("GroupFollow3D", "DampingFollow3D", GD.Load<Script>("uid://bp401ob6gu3eg"), GD.Load<Texture2D>("uid://8e51jf5fna86"));

		#endregion

		#region Look at Mode

		AddCustomType("LookAtMode3D", "Resource", GD.Load<Script>("uid://dsu3gvxcxgljw"), GD.Load<Texture2D>("uid://8e51jf5fna86"));
		AddCustomType("MimicLookAt3D", "LookAtMode3D", GD.Load<Script>("uid://m5gtrfjcwusm"), GD.Load<Texture2D>("uid://8e51jf5fna86"));
		AddCustomType("DampingLookAt3D", "LookAtMode3D", GD.Load<Script>("uid://cvosr8pgqvl1d"), GD.Load<Texture2D>("uid://8e51jf5fna86"));
		AddCustomType("SimpleLookAt3D", "DampingLookAt3D", GD.Load<Script>("uid://co40q5yby7kmq"), GD.Load<Texture2D>("uid://8e51jf5fna86"));
		AddCustomType("GroupLookAt3D", "DampingLookAt3D", GD.Load<Script>("uid://cu77qkw4k0ij5"), GD.Load<Texture2D>("uid://8e51jf5fna86"));

		#endregion

		AddAutoloadSingleton($"{BROKEN_CAMERA_MANAGER}3D", "res://addons/BrokenCamera/Manager/BrokenCameraManager3D.cs");

		//Broken Camera 3D Gizmo

		// _bcam3DGizmoPlugin?.Dispose();
		// _bcam3DGizmoPlugin?.Free();

		// _bcam3DGizmoPlugin = new();

		// AddNode3DGizmoPlugin(_bcam3DGizmoPlugin);
	}

	public override void _ExitTree()
	{

		// if (_bcam3DGizmoPlugin is not null)
		// {
		// 	RemoveNode3DGizmoPlugin(_bcam3DGizmoPlugin);

		// 	_bcam3DGizmoPlugin?.Dispose();
		// 	_bcam3DGizmoPlugin?.Free();

		// 	_bcam3DGizmoPlugin = null;
		// }

		RemoveAutoloadSingleton($"{BROKEN_CAMERA_MANAGER}3D");

		//RemoveCustomType(BCAM_2D);
		RemoveCustomType(BCAM_3D);
		RemoveCustomType($"{BCAM_HOST}3D");

		RemoveCustomType(CAMERA_3D_RESOURCE);
		RemoveCustomType(TWEEN_RESOURCE);

		#region Follow Mode

		RemoveCustomType("FollowMode3D");
		RemoveCustomType("GluedFollow3D");
		RemoveCustomType("DampingFollow3D");
		RemoveCustomType("SimpleFollow3D");
		RemoveCustomType("GroupFollow3D");

		#endregion

		#region Look at Mode

		RemoveCustomType("LookAtMode3D");
		RemoveCustomType("MimicLookAt3D");
		RemoveCustomType("DampingLookAt3D");
		RemoveCustomType("SimpleLookAt3D");
		RemoveCustomType("GroupLookAt3D");

		#endregion
	}

	#endregion
}
#endif
