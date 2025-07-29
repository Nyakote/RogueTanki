using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.Globalization;


public class MapLoader : MonoBehaviour
{
    public string xmlFileName = "Silence"; // without .xml
    private string propsFolder = "MapElements/Props";  

    void Start()
    {
        LoadMap();
    }

    void LoadMap()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(xmlFileName);
        if (textAsset == null)
        {
            Debug.LogError("Map XML file not found in Resources!");
            return;
        }

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);

        XmlNodeList propNodes = xmlDoc.GetElementsByTagName("prop");
        foreach (XmlNode prop in propNodes)
        {
            string propName = prop.Attributes["name"].Value;
            XmlNode pos = prop["position"];
            XmlNode rot = prop["rotation"];

            Vector3 position = new Vector3(
            float.Parse(pos["x"].InnerText, CultureInfo.InvariantCulture) / 100f,
            float.Parse(pos["z"].InnerText, CultureInfo.InvariantCulture) / 100f,
            float.Parse(pos["y"].InnerText, CultureInfo.InvariantCulture) / 100f
            );

            Quaternion rotation = Quaternion.Euler(0f, -float.Parse(rot["z"].InnerText, CultureInfo.InvariantCulture) * Mathf.Rad2Deg, 0f);


            GameObject prefab = Resources.Load<GameObject>($"{propsFolder}/{propName}");
            if (prefab != null)
                Instantiate(prefab, position, rotation);
            else
                Debug.LogWarning($"Prefab not found: {propName}");
        }

        Debug.Log("Map loaded.");
    }
}
