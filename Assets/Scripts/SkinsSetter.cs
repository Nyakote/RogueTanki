using UnityEngine;

public class SkinsSetter : MonoBehaviour
{

    public string hullName;       // e.g. "Wasp"
    public string turretName;     // e.g. "Smoky"
    public int hullMod;           // 0 to 3 for M0 to M3
    public int turretMod;         // 0 to 3
    public string[] camoNames;    // e.g. { "Camo1", "Camo2" }

    public Material baseShaderMaterial; // Your Shader Graph material template

    private Renderer[] hullRenderers;
    private Renderer[] turretRenderers;

    void Start()
    {
        Transform parent = transform.parent;
        if (parent.CompareTag("Player")){ return; }
        // Example: Get renderers of hull and turret children
        hullRenderers = GetComponentsInChildren<Renderer>(); // filter by hull only if needed
        turretRenderers = GetComponentsInChildren<Renderer>(); // filter by turret only if needed

        ApplyAppearance();
    }

    public void ApplyAppearance()
    {
        // Assuming camoNames has at least one element
        string camoName = camoNames[0];

        Texture2D camoTex = Resources.Load<Texture2D>($"Paints/Skins/{camoName}_image");
        if (camoTex == null)
        {
            Debug.LogError($"Camo texture '{camoName}_image' not found in Resources/Paints/Skins");
            return;
        }

        // Assuming you have a Renderer and material already assigned
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            Material matInstance = new Material(baseShaderMaterial); // create instance to avoid shared material edits
            matInstance.SetTexture("_CamoTex", camoTex); // Replace "_CamoTex" with your shader property
            renderer.material = matInstance;
        }
    }

}
