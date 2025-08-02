using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TankPicker : MonoBehaviour
{
    public Dropdown hullDropdown;
    public Dropdown turretDropdown;
    public Dropdown hullModDropdown;
    public Dropdown turretModDropdown;
    public Dropdown paint;
    public Button playButton;

    private string[] availableHulls = { "Wasp", "Hornet", "Hunter", "Viking", "Dictator", "Titan", "Mammoth" };
    private string[] availableTurrets = { "Smoky", "Firebird", "Freeze", "Izida", "Railgun", "Ricochet", "Shaft", "Thunder", "Twins" };
    private string[] availableModsTurret = { "M0", "M1", "M2", "M3" };
    private string[] availableModsHull = { "M0", "M1", "M2", "M3" };
    private string[] avaiblePaints = { "black", "blue", "dirt", "green", "lead", "red", "orange", "white" };

    private void Start()
    {
        PopulateDropdown(hullDropdown, availableHulls);
        PopulateDropdown(turretDropdown, availableTurrets);
        PopulateDropdown(hullModDropdown, availableModsHull);
        PopulateDropdown(turretModDropdown, availableModsTurret);
        PopulateDropdown(paint, avaiblePaints);

        playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    private void PopulateDropdown(Dropdown dropdown, string[] options)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(new System.Collections.Generic.List<string>(options));
    }

    private void OnPlayButtonClicked()
    {
        string selectedHull = availableHulls[hullDropdown.value];
        string selectedTurret = availableTurrets[turretDropdown.value];
        string selectedHullMod = availableModsHull[hullModDropdown.value];
        string selectedTurretMod = availableModsTurret[turretModDropdown.value];
        string selectedPaint = avaiblePaints[paint.value];

        PlayerPrefs.SetString("SelectedHull", selectedHull);
        PlayerPrefs.SetString("SelectedTurret", selectedTurret);
        PlayerPrefs.SetString("SelectedHullMod", selectedHullMod);
        PlayerPrefs.SetString("SelectedTurretMod", selectedTurretMod);
        PlayerPrefs.SetString("SelectedPaint", selectedPaint);

        SceneManager.LoadScene("DM Maps");
    }
}
