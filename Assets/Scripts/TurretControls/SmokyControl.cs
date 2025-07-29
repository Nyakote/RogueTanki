using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class SmokyControl : TurretControlBase
{

    #region 

    [Header("Basic Turret Stats")]
    private string turretName = "Smoky";
    private string turretMod;
    public float price;
    public float min_damage;
    public float max_damage;
    public float impact_force;
    public float recoil;
    public float reload_time;
    public float rotation_speed;
    public float critical_chance;
    public float critical_damage;
    public float min_damage_range;
    public float max_damage_range;

    [Header("Turret Stats")]
    public float weak_damage_percent;
    public float auto_aim_up;
    public float auto_aim_down;

    [Header("Visuals")]


    [Header("References")]
    private RectTransform crosshair;
    private ParticleSystem ps;
    private SmokyStatsLoader smoky;

    #endregion

    void Start()
    {
        smoky = GetComponent<SmokyStatsLoader>();
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
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotation_speed * Time.deltaTime);

            }
        }
    }

    private void StatsSetter()
    {
        turretMod = transform.parent.parent.parent.name.Split(' ')[4];
        var (turretStats, turretFixed) = smoky.GetFullStats(turretName, turretMod);
        price = turretStats.price;
        min_damage = turretStats.min_damage;
        max_damage = turretStats.max_damage;
        impact_force = turretStats.impact_force;
        recoil = turretStats.recoil;
        reload_time = turretStats.reload_time;
        rotation_speed = turretStats.rotation_speed;
        critical_chance = turretStats.critical_chance;
        critical_damage = turretStats.critical_damage;
        min_damage_range = turretStats.min_damage_range;
        max_damage_range = turretStats.max_damage_range;
        weak_damage_percent = turretFixed.weak_damage_percent;
        auto_aim_up = turretFixed.auto_aim_up;
        auto_aim_down = turretFixed.auto_aim_down;




        rotation_speed = rotation_speed / 10f;
    }
    public void OnShoot(InputValue value)
    {
        if (transform.tag == "Player")
        {
            ps.Emit(1);
        }
    }
    public override float GetRotateSpeed() => rotation_speed;
}
