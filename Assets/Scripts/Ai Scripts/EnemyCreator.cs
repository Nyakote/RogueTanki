﻿using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.LowLevel;
using System.IO;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyCreator : MonoBehaviour
{
    [SerializeField] Transform enemyroot;
    private Transform father;
    BoxCollider bc;

    private string[] availableHulls = { "Wasp", "Hornet", "Hunter", "Viking", "Dictator", "Titan", "Mammoth" };
    private string[] availableTurrets = { "Smoky", "Firebird", "Freeze", "Railgun", "Ricochet", "Shaft", "Thunder", "Twins" };
    private int hullMod;
    private int turretMod;
    int hullID;
    int turretID;

    private NavMeshAgent layerGround;
    private string hull;
    private string turret;


    GameObject hullInstance;
    GameObject turretInstance;
    string randomSkinName;
    public Material TankPainter;

    void Start()
    {
        father = GameObject.Find("Enemies").transform;
        transform.SetParent(father);
        bc = enemyroot.GetComponent<BoxCollider>();
        TankCreator();
        TankSetter();
        Vector3 euler = transform.rotation.eulerAngles;
        if (euler.x != 0f || euler.z != 0f)
        {
            transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
        }
    }



    void TankCreator()
    {
        hullID = UnityEngine.Random.Range(0, availableHulls.Length);
        turretID = UnityEngine.Random.Range(0, availableTurrets.Length);

        hull = availableHulls[hullID];
        turret = availableTurrets[turretID];

    }

    void TankSetter()
    {
        GameObject hullPrefab = Resources.Load<GameObject>("Hulls/" + hull);
        GameObject turretPrefab = Resources.Load<GameObject>("Turrets/" + turret);

        if (hullPrefab && turretPrefab)
        {
            hullInstance = Instantiate(hullPrefab, enemyroot.position, transform.rotation);
            hullInstance.transform.SetParent(enemyroot);
            hullInstance.transform.localPosition = Vector3.zero;
            hullInstance.transform.localRotation = Quaternion.identity;

            Transform turretMount = hullInstance.transform.Find("mount");

            if (turretMount != null)
            {
                turretInstance = Instantiate(turretPrefab);
                Quaternion orinalRot = transform.rotation;
                transform.rotation = Quaternion.identity;

                turretInstance.transform.SetParent(turretMount);
                turretInstance.transform.localPosition = Vector3.zero;
                turretInstance.transform.localRotation = Quaternion.identity;

                Renderer[] allBounds = hullInstance.GetComponentsInChildren<Renderer>(true).Concat(turretInstance.GetComponentsInChildren<Renderer>(true)).ToArray();
                Bounds bounds = new Bounds();
                bool hasBounds = false;

                foreach (Renderer renderer in allBounds)
                {
                    if (!renderer.enabled || !renderer.gameObject.activeInHierarchy) continue;
                    if (renderer is ParticleSystemRenderer) continue;
                    if (renderer.bounds.size == Vector3.zero) continue;
                    if (!hasBounds)
                    {
                        bounds = renderer.bounds;
                        hasBounds = true;
                    }
                    else
                    {
                        bounds.Encapsulate(renderer.bounds);
                    }
                }

                if (hasBounds)
                {
                    bc.center = enemyroot.InverseTransformPoint(bounds.center);
                    bc.size = bounds.size;
                }
                transform.rotation = orinalRot;
            }

            transform.name = "Enemy" + " " + hull + " " + turret + " M" + hullMod + " M" + turretMod;
            Skin();
            EnemyAI enemyAI = GetComponent<EnemyAI>();
            if (enemyAI != null)
                enemyAI.Initialize(hullMod, turretMod, availableHulls[hullID], availableTurrets[turretID], bc.bounds.size.x / 2);
        }

    }
    public void Initialize(NavMeshAgent ground, int hull, int turret)
    {
        layerGround = ground;
        hullMod = hull;
        turretMod = turret;
    }

        void Skin()
        {
            string[] availableSkins = new string[]
            {
                    "acid",
                    "amilia",
                    "anubis",
                    "apple",
                    "arab",
                    "barkhan",
                    "besthelper",
                    "black",
                    "blink",
                    "blizzard",
                    "blueray",
                    "blue",
                    "carbon",
                    "champion",
                    "clay",
                    "creator",
                    "digital",
                    "dima",
                    "dirt",
                    "dragon",
                    "drug",
                    "electra",
                    "euro2012",
                    "flame",
                    "flash",
                    "flora",
                    "flowers",
                    "flow",
                    "foreign",
                    "forester",
                    "galaxy",
                    "garland",
                    "ginga",
                    "green",
                    "halloween2020",
                    "heartbeat",
                    "helios",
                    "holiday",
                    "inferno",
                    "irbis",
                    "izumurud",
                    "jaguar",
                    "kedr",
                    "khokhloma",
                    "krio",
                    "lava",
                    "lead",
                    "marine",
                    "marsh",
                    "mary",
                    "matrix",
                    "metallic",
                    "moderator",
                    "moonwalker",
                    "needles",
                    "nefrit",
                    "newyear2020",
                    "newyear2021",
                    "nightcity",
                    "nightmare",
                    "orange",
                    "outburst",
                    "phenix",
                    "plexus",
                    "prodigy",
                    "pumpkins",
                    "punk",
                    "python",
                    "radiance",
                    "redl",
                    "red",
                    "reporter",
                    "rock",
                    "roger",
                    "rosequartz",
                    "rustle",
                    "rust",
                    "safari",
                    "savanna",
                    "smokescreen",
                    "space",
                    "spark",
                    "spectator",
                    "spectr",
                    "spid",
                    "standstone",
                    "storm",
                    "surf",
                    "taiga",
                    "tester",
                    "thunderstorm",
                    "tina",
                    "tot",
                    "triangles",
                    "tundra",
                    "urban",
                    "virus",
                    "vlastelin",
                    "white",
                    "witch",
                    "with_love",
                    "zeus"
            };
            int randomIndex = UnityEngine.Random.Range(0, availableSkins.Length);
            randomSkinName = availableSkins[randomIndex];
            ApplyAppearance();
        }

    public void ApplyAppearance()
    {
        Texture2D camoTex = Resources.Load<Texture2D>($"Paints/Skins/{randomSkinName}_image");
        Texture2D hullElements = Resources.Load<Texture2D>($"Hulls/Skins/{hull.ToLower()}_m{hullMod}");
        Texture2D turretElements = Resources.Load<Texture2D>($"Turrets/Skins/{turret.ToLower()}_m{turretMod}");

        Renderer turretSkin = turretInstance.GetComponentInChildren<Renderer>();
        Renderer hullSkin = hullInstance.GetComponentInChildren<Renderer>();

        Material hullMaterial = new Material(TankPainter);
        Material turretMaterial = new Material(TankPainter);

        hullMaterial.SetTexture("_Base", hullElements);
        hullMaterial.SetTexture("_Layer1", camoTex);

        turretMaterial.SetTexture("_Base", turretElements);
        turretMaterial.SetTexture("_Layer1", camoTex);


        turretSkin.material = turretMaterial;
        hullSkin.material = hullMaterial;

        if (camoTex == null) Debug.LogError($"Camo texture not found: {randomSkinName}");
        if (hullElements == null) Debug.LogError($"Hull skin not found: {hull.ToLower()}_m{hullMod}");
        if (turretElements == null) Debug.LogError($"Turret skin not found: {turret.ToLower()}_m{turretMod}");

    }

}
