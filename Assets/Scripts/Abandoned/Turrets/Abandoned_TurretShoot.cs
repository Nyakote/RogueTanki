/*using UnityEngine;
using UnityEngine.ProBuilder;

public class TurretShoot : MonoBehaviour
{
    #region Variables

    [Header("Proj/muzzle")]
    [SerializeField] GameObject bulletprefab;
    [SerializeField] Transform muzzle;

    [Header("Turret Stats")]
    public float minDamage;
    public float maxDamage;
    public float impactForce;
    public float recoil;
    public float reloadTime;
    public float criticalChance;
    public float criticalDamage;
    public float minDamageRange;
    public float maxDamageRange;

    [Header("ShootControls")]
    private float timeSinceLastShot = -Mathf.Infinity;

    [Header("References")]
    private TurretControl tr;
    private ParticleSystem ps;
    private AutoAim autoAim;

    #endregion

    #region UnityMethods
    private void Start()
    {
        tr = GetComponent<TurretControl>();
        ps = GetComponentInChildren<ParticleSystem>();
        Invoke("StatsSetter", 1f);

    }
 
    void Update()
    {
        autoAim = GetComponent<AutoAim>();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    #endregion

    #region CustomMethods
    void Shoot()
    {
        autoAim.TargetDetection();
        if (Time.time - timeSinceLastShot >= reloadTime)
        {
            GameObject projObj = Instantiate(bulletprefab, muzzle.position, muzzle.rotation);
            Projectile proj = projObj.GetComponent<Projectile>();

            if (proj != null)
            {
                proj.Initialize(minDamage, maxDamage, impactForce, criticalChance, criticalDamage, minDamageRange, maxDamageRange);
            }
            timeSinceLastShot = Time.time;
        }
        else
        {
            //Debug.Log($"Reloading... {GetReloadProgress()}");
        }
    }

    void StatsSetter()
    {
        minDamage = tr.minDamage;
        maxDamage = tr.maxDamage;
        impactForce = tr.impactForce;
        recoil = tr.recoil;
        reloadTime = tr.reloadTime;
        criticalChance = tr.criticalChance;
        criticalDamage = tr.criticalDamage;
        minDamageRange = tr.minDamageRange;
        maxDamageRange = tr.maxDamageRange;
    }

    public float GetReloadProgress()
    {
        return Mathf.Clamp01((Time.time - timeSinceLastShot) / reloadTime);
    }
    private void OnDrawGizmos()
    {
        if (muzzle == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(muzzle.position, 100f);

        // Visualize forward direction
        Gizmos.color = Color.white;
        Gizmos.DrawRay(muzzle.position, muzzle.forward * 100f);

        // Calculate pitch directions
        Vector3 pitchUp = Quaternion.AngleAxis(9, muzzle.right) * muzzle.forward;
        Vector3 pitchDown = Quaternion.AngleAxis(-12, muzzle.right) * muzzle.forward;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(muzzle.position, pitchUp * 100f);   // Max pitch
        Gizmos.DrawRay(muzzle.position, pitchDown * 100f); // Min pitch

        // Yaw directions at muzzle width scale (simulate spread based on scale.x)
        float yawLimit = Mathf.Atan((muzzle.localScale.x*2) / 100f) * Mathf.Rad2Deg;
        Vector3 yawLeft = Quaternion.AngleAxis(-yawLimit, muzzle.up) * muzzle.forward;
        Vector3 yawRight = Quaternion.AngleAxis(yawLimit, muzzle.up) * muzzle.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(muzzle.position, yawLeft * 100f);  // Left yaw
        Gizmos.DrawRay(muzzle.position, yawRight * 100f); // Right yaw
    }

    #endregion
}
*/