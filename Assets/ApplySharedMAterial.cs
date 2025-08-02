using UnityEngine;

public class ApplySharedMAterial : MonoBehaviour
{

    public Material overlayMaterial;
    private Transform hull;
    private Transform turret;
    private string hullName = "hull";
    private string turretName;
    private PickLoader pickLoader;

    void Awake()
    {
        if (pickLoader == null)
            pickLoader = GetComponent<PickLoader>();
    }

    void Start()
    {
        Invoke(nameof(SetupHullAndTurret), 1f);
    }

    void SetupHullAndTurret()
    {
        turretName = pickLoader.turretName + "(Clone)";

        hull = transform.FindDeepChild(hullName);
        turret = transform.FindDeepChild(turretName);

        if (hull == null || turret == null)
        {
            Debug.LogError("Hull or turret not found!");
            return;
        }

        AddOverlayMaterial(hull.GetComponent<Renderer>(), overlayMaterial);
        AddOverlayMaterial(turret.GetComponent<Renderer>(), overlayMaterial);
    }

    void AddOverlayMaterial(Renderer renderer, Material overlay)
    {
        var mats = renderer.sharedMaterials;
        if (mats.Length > 1 && mats[mats.Length - 1] == overlay)
            return;

        Material[] newMats = new Material[mats.Length + 1];
        mats.CopyTo(newMats, 0);
        newMats[newMats.Length - 1] = overlay;
        renderer.sharedMaterials = newMats;
    }


}
