using UnityEngine;
using UnityEngine.UI;

public class CanvasUI : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;

    private GameObject turret;
    [SerializeField] GameObject playerRoot;
    private HealthComponent hc;

    [SerializeField] Image healthBar;
    public float originalWidth;

    [SerializeField] Image energyBar;
    public float originalEnergWidth;

    public float currentEnergy;
    public float maxEnergy;
    
    public bool isInitialized= false;

    private string turretName;
    private string turretMod;




    void Update()
    {
        if (!isInitialized) return;

        currentHealth = hc.currentHealth;
        float healthPercent = currentHealth / maxHealth;
        float newSize = Mathf.Clamp(healthPercent * originalWidth, 0, originalWidth);

        RectTransform rt = healthBar.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(newSize, rt.sizeDelta.y);

        float energyPercent = currentEnergy / maxEnergy;
        float newEnergySize = Mathf.Clamp(energyPercent * originalEnergWidth, 0, originalEnergWidth);

        RectTransform eb = energyBar.GetComponent<RectTransform>();
        eb.sizeDelta = new Vector2(newEnergySize, rt.sizeDelta.y);
    }



    public void Initialize(GameObject turretI, string name, string mod)
    {
        turret = turretI;
        turretName = name;
        Invoke("StatsSetter", 1f);
    }
    public void CRInitialize(float CEnergy)
    {
        currentEnergy = CEnergy;
    }

    private void StatsSetter()
    {
        transform.SetParent(turret.transform, false);
        hc = playerRoot.GetComponent<HealthComponent>();
        RectTransform rt = healthBar.GetComponent<RectTransform>();
        originalWidth = rt.sizeDelta.x;
        maxHealth = hc.maxHealth;
        currentHealth = hc.currentHealth;

        RectTransform eb = energyBar.GetComponent<RectTransform>();
        originalEnergWidth = eb.sizeDelta.x;
        maxEnergy = 1000f;


        isInitialized = true;
    }
}
