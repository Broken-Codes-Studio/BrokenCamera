/*#if TOOLS
namespace Addon.BrokenCamera.Editor.Gizmos;

using Godot;

[Tool]
public partial class CustomPluginGizmo<[MustBeVariant] T> : EditorNode3DGizmoPlugin
{

    public Texture2D gizmo_icon { private get; set; }

    float _gizmo_scale = .035f;

    public CustomPluginGizmo()
    {
        HandleMaterials();
    }

    protected void HandleMaterials()
    {
        CreateIconMaterial(_GetGizmoName(), gizmo_icon, false, Colors.White);
        CreateMaterial("main", Color.Color8(252, 127, 127, 255));
    }

    public override bool _HasGizmo(Node3D spatial) => spatial is T;

    public override void _Redraw(EditorNode3DGizmo gizmo)
    {
        gizmo.Clear();

        Material icon = GetMaterial(_GetGizmoName(), gizmo);
        gizmo.AddUnscaledBillboard(icon, _gizmo_scale);

        Material material = GetMaterial("main", gizmo);
        gizmo.AddLines(_DrawFrustum(), material);

    }

    private Vector3[] _DrawFrustum()
    {

        Vector3[] lines = new Vector3[16];

        float dis = 0.25f;
        float width = dis * 1.25f;
        float len = dis * 1.5f;

        //Trapezoid
        lines[0] = Vector3.Zero;
        lines[1] = new Vector3(-width, dis, -len);

        lines[2] = Vector3.Zero;
        lines[3] = new Vector3(width, dis, -len);

        lines[4] = Vector3.Zero;
        lines[5] = new Vector3(-width, -dis, -len);

        lines[6] = Vector3.Zero;
        lines[7] = new Vector3(width, -dis, -len);

        #region Square

        //Left
        lines[8] = new Vector3(-width, dis, -len);
        lines[9] = new Vector3(-width, -dis, -len);

        //Buttom
        lines[10] = new Vector3(-width, -dis, -len);
        lines[11] = new Vector3(width, -dis, -len);

        //Right
        lines[12] = new Vector3(width, -dis, -len);
        lines[13] = new Vector3(width, dis, -len);

        //Top
        lines[14] = new Vector3(width, dis, -len);
        lines[15] = new Vector3(-width, dis, -len);

        #endregion

        return lines;

    }

}
#endif */