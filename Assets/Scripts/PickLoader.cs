using System.Linq;
using UnityEngine;

public class PickLoader : MonoBehaviour
{
    [SerializeField] private Transform playerRoot;
    BoxCollider playerCollider;

    void Start()
    {
        playerCollider = playerRoot.GetComponent<BoxCollider>();

        string hullName = PlayerPrefs.GetString("SelectedHull", "Wasp");
        string turretName = PlayerPrefs.GetString("SelectedTurret", "Smoky");   

        GameObject hullPrefab = Resources.Load<GameObject>("Hulls/" + hullName);
        GameObject turretPrefab = Resources.Load<GameObject>("Turrets/" + turretName);

        if (hullPrefab && turretPrefab)
        {
            Quaternion originalRotation = playerRoot.rotation;
            playerRoot.rotation = Quaternion.identity;

            GameObject hullInstance = Instantiate(hullPrefab, playerRoot.position, playerRoot.rotation);
            hullInstance.transform.SetParent(playerRoot);
            hullInstance.transform.localPosition = Vector3.zero;
            hullInstance.transform.localRotation = Quaternion.identity;

            Transform turretMount = hullInstance.transform.Find("mount");

            if (turretMount != null)
            {
                GameObject turretInstance = Instantiate(turretPrefab);


                turretInstance.transform.SetParent(turretMount);
                turretInstance.transform.localPosition = Vector3.zero;
                turretInstance.transform.localRotation = Quaternion.identity;

                Renderer[] allRenderers = hullInstance.GetComponentsInChildren<Renderer>(true).Concat(turretInstance.GetComponentsInChildren<Renderer>(true)).ToArray();

                Bounds totalBounds = new Bounds();
                bool hasBounds = false;

                foreach (Renderer r in allRenderers)
                {
                    if (!r.enabled || !r.gameObject.activeInHierarchy) continue;
                    if (r is ParticleSystemRenderer) continue;
                    if (r.bounds.size == Vector3.zero) continue;
                    if (!hasBounds)
                    {
                        totalBounds = r.bounds;
                        hasBounds = true;
                    }
                    else
                    {
                        totalBounds.Encapsulate(r.bounds);
                    }
                }

                if (hasBounds)
                {
                    playerCollider.center = playerRoot.InverseTransformPoint(totalBounds.center);
                    playerCollider.size = totalBounds.size;
                }

                playerRoot.rotation = originalRotation;
            }
            transform.name = transform.name + " " + hullName + " " + turretName + " M0" + " M0";
            foreach (Transform t in playerRoot.GetComponentsInChildren<Transform>())
            {
                t.gameObject.tag = "Player";
            }
        }

    }
}
