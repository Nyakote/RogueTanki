using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.LowLevel;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyCreator : MonoBehaviour
{
    [SerializeField] Transform enemyroot;
    private Transform father;
    BoxCollider bc;

    private string[] availableHulls = { "Wasp", "Hornet", "Hunter", "Viking", "Dictator", "Titan", "Mammoth" };
    private string[] availableTurrets = { "Smoky", "Firebird", "Freeze", "Izida", "Railgun", "Ricochet", "Shaft", "Thunder", "Twins" };
    private int hullMod;
    private int turretMod;
    int hullID;
    int turretID;

    private NavMeshAgent layerGround;
    private string hull;
    private string turret;

    void Start()
    {
        father = GameObject.Find("Enemies").transform;
        transform.SetParent(father);
        bc = enemyroot.GetComponent<BoxCollider>();
        TankCreator();
        TankSetter();

    }

    // Update is called once per frame
    void Update()
    {
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
            GameObject hullInstance = Instantiate(hullPrefab, enemyroot.position, transform.rotation);
            hullInstance.transform.SetParent(enemyroot);
            hullInstance.transform.localPosition = Vector3.zero;
            hullInstance.transform.localRotation = Quaternion.identity;

            Transform turretMount = hullInstance.transform.Find("mount");

            if (turretMount != null)
            {
                GameObject turretInstance = Instantiate(turretPrefab);
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
            float radius = bc.bounds.size.x / 2; 
            transform.name = "Enemy" + " " + hull + " " + turret + " M" + hullMod + " M" + turretMod;
            EnemyAI enemyAI = GetComponent<EnemyAI>();
            if (enemyAI != null)
                enemyAI.Initialize(hullMod, turretMod, availableHulls[hullID], availableTurrets[turretID], radius);
        }
       
    }
    public void Initialize(NavMeshAgent ground, int hull, int turret)
    {
        layerGround = ground;
        hullMod = hull;
        turretMod = turret;
    }



}
