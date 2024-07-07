using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class PointerInformation : MonoBehaviour
{
    // class tracking pointer information
    private Country lastClicked = null;
    private GameObject countryPopup;
    private TextMeshProUGUI textMeshProUGUI;
    private TextMeshProUGUI populationTextMesh;

    public Country LastClicked {
        get => lastClicked;
        set {
            lastClicked = value;
            if (value == null) {
                countryPopup.SetActive(false);
                return;
            }
        }
    }

    public void UpdateCountryPopup(Country country) {
        if (country == null) {
            countryPopup.SetActive(false);
            return;
        }
        countryPopup.SetActive(true);
        textMeshProUGUI.text = country.CountryName;
        List<(string, double)> statusBarList =
            new List<(string, double)>{("War", country.Aggressiveness),
            ("Technology", country.Technology),
            ("Fertility", country.Fertility),
            ("Food", country.Food)};
        foreach ((string status, double stat) in statusBarList) {
            SetCircularStatusBar(status, stat);
        }
        populationTextMesh.text = "Population: " + country.PopulationCount.ToString();
    }

    private void SetCircularStatusBar(string upgradeType, double value) {
        GameObject upgradeParent = GameObject.Find(upgradeType);
        CircularFillBar circularFillBar = upgradeParent.GetComponent<CircularFillBar>();
        circularFillBar.UpdateFillValue((float) value);
    }

    // Start is called before the first frame update
    void Start()
    {
        countryPopup = GameObject.Find("CountryPopup");
        textMeshProUGUI = GameObject.Find("CountryName").GetComponent<TextMeshProUGUI>();
        populationTextMesh = GameObject.Find("PopulationCount").GetComponent<TextMeshProUGUI>();
        countryPopup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCountryPopup(lastClicked);
    }
}
