using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TankPicker : MonoBehaviour
{
    public Dropdown hullDropdown;
    public Dropdown turretDropdown;
    public Button playButton;

    private string[] availableHulls = { "Wasp", "Hornet", "Hunter", "Viking", "Dictator", "Titan", "Mommoth" };
    private string[] availableTurrets = { "Smoky", "Firebird", "Freeze", "Izida", "Railgun", "Ricochet", "Shaft", "Thunder", "Twins" };

    private void Start()
    {
        PopulateDropdown(hullDropdown, availableHulls);
        PopulateDropdown(turretDropdown, availableTurrets);

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

        PlayerPrefs.SetString("SelectedHull", selectedHull);
        PlayerPrefs.SetString("SelectedTurret", selectedTurret);

        SceneManager.LoadScene("TestScene");
    }
}
