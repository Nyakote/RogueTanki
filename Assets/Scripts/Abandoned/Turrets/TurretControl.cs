/*using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class TurretControl : MonoBehaviour
{
    #region Variables

    [Header("Name/Mod")]
    [SerializeField] string turretName = "";
    [SerializeField] string turretMod = "";
    [SerializeField] Transform hullTransform;
    [SerializeField] Transform mountpoint;

    [Header("Turret Stats")]
    public float price;
    public float minDamage;
    public float maxDamage;
    public float impactForce;
    public float recoil;
    public float reloadTime;
    public float rotationSpeed;
    public float criticalChance;
    public float criticalDamage;
    public float minDamageRange;
    public float maxDamageRange;

    [Header("Turret Control")]
    private float inputTurret;
    private bool isCentering = false;
    private float currentTurretYaw = 0f;
    private float startLocalYaw = 0f;

    [Header("References")]
    private TurretStatsLoader loader;
    #endregion

    #region UnityMethods
    private void Start()
    {
        float currentYaw = transform.eulerAngles.y;
        float hullYaw = hullTransform.eulerAngles.y;
        startLocalYaw = Mathf.DeltaAngle(hullYaw, currentYaw);
        currentTurretYaw = startLocalYaw;
        loader = Object.FindFirstObjectByType<TurretStatsLoader>();
        StatsSetterer();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            inputTurret = -1.0f;
        }
        else if (Input.GetKey(KeyCode.X))
        {
            inputTurret = 1.0f;
        }
        else
        {
            inputTurret = 0.0f;
        }   
    }

    private void LateUpdate()
    {
        TurretTurning();
        if (Input.GetKeyDown(KeyCode.C) || isCentering)
        {
            if (Mathf.Abs(currentTurretYaw - startLocalYaw) > 0.1f)
            {
                isCentering = true;
                TurretCentering();
            }
        }
    }

    #endregion

    #region CustomMethods

    void StatsSetterer()
    {
        TurretMod turretStats = loader.GetStats(turretName, turretMod);
        if (turretStats != null)
        {
            rotationSpeed = turretStats.rotation_speed;
            price = turretStats.price;
            minDamage = turretStats.min_damage;
            maxDamage = turretStats.max_damage;
            impactForce = turretStats.impact_force;
            recoil = turretStats.recoil;
            reloadTime = turretStats.reload_time;
            criticalChance = turretStats.critical_chance;
            criticalDamage = turretStats.critical_damage;
            minDamageRange = turretStats.min_damage_range;
            maxDamageRange = turretStats.max_damage_range;
        }
        else
        {
            Debug.LogError($"Turret stats not found for {turretName} {turretMod}");
        }
    }

    void TurretCentering()
    {
        if (inputTurret == 0)
        {
            currentTurretYaw = Mathf.MoveTowards(currentTurretYaw,startLocalYaw, rotationSpeed * Time.deltaTime);

            if (Mathf.Abs(currentTurretYaw - startLocalYaw) < 0.1f)
            {
                currentTurretYaw = startLocalYaw;
                isCentering = false;
            }

            float hullYaw = hullTransform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, hullYaw + currentTurretYaw, 0f);
        }
        else
        {
            isCentering = false;
        }
    }

  

    void TurretTurning()
    {
        transform.position = mountpoint.position;
        float hullXrot = hullTransform.eulerAngles.x;
        float hullZrot = hullTransform.eulerAngles.z;
        transform.forward = hullTransform.forward;
        float hullYaw = hullTransform.eulerAngles.y;
        currentTurretYaw += inputTurret * rotationSpeed * Time.deltaTime;
        Quaternion turretRotation = Quaternion.Euler(hullXrot, hullYaw + currentTurretYaw, hullZrot);
        transform.rotation = turretRotation;
    }

    #endregion
}

*/