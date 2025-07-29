using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class RailgunControl : TurretControlBase
{

    #region 

    [Header("Basic Turret Stats")]
    private string turretName = "Railgun";
    private string turretMod;
    private int price;
    public int minDamage;
    public int maxDamage;
    public float rotationSpeed;
    public float rotationAcceleration;

    [Header("Turret Stats")]
    public float impact_force;
    public float recoil;
    public float reload_time;
    public float charge_time;
    public float delay;
    public float piercing_damage_percent;
    public int auto_aim_up;
    public int auto_aim_down;

    [Header("Visuals")]


    [Header("References")]
    private RectTransform crosshair;
    private ParticleSystem ps;
    private RailgunStatsLoader railgun;

    #endregion

    void Start()
    {
        railgun = GetComponent<RailgunStatsLoader>();
        crosshair = GameObject.Find("Crosshair").GetComponent<RectTransform>();
        ps = GetComponentInChildren<ParticleSystem>();
        Invoke("StatsSetter", 0.2f);
    }

    void Update()
    {
        if (transform.tag == "Player")
        {
            MoveCrosshair();
            RotateTowardMouse();
        }
    }

    void MoveCrosshair()
    {
        crosshair.position = Input.mousePosition;
    }

    void RotateTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 direction = targetPoint - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            }
        }
    }

    private void StatsSetter()
    {
        turretMod = transform.parent.parent.parent.name.Split(' ')[4];
        var (turretStats, turretFixed) = railgun.GetFullStats(turretName, turretMod);
        price = turretStats.price;
        minDamage = turretStats.min_damage;
        maxDamage = turretStats.max_damage;
        rotationSpeed = turretStats.rotation_speed;
        rotationAcceleration = turretStats.rotation_acceleration;
        recoil = turretStats.recoil;
        impact_force = turretStats.impact_force;
        reload_time = turretStats.reload_time;
        charge_time = turretStats.charge_time;
        delay = turretStats.delay;
        piercing_damage_percent = turretStats.piercing_damage_percent;
        auto_aim_up = turretFixed.auto_aim_up;
        auto_aim_down = turretFixed.auto_aim_down;




        rotationSpeed = rotationSpeed / 10f;
    }
    public void OnShoot(InputValue value)
    {
        if (transform.tag == "Player")
        {
            ps.Emit(1);
        }
    }
    public override float GetRotateSpeed() => rotationSpeed;
}
