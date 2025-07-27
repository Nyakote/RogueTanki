using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class ThunderControl : MonoBehaviour
{

    #region 

    [Header("Basic Turret Stats")]
    private string turretName = "Thunder";
    private string turretMod;
    public int price;
    public int min_damage;
    public int max_damage;
    public float impact_force;
    public float splash_impact_force;
    public float recoil;
    public float reload_time;
    public float rotation_speed;
    public float rotation_acceleration;
    public float min_damage_range;
    public float max_damage_range;
    [Header("Turret Stats")]
    public int weak_damage_percent;
    public int max_damage_radius;
    public int min_damage_radius;
    public int weak_damage_percent_splash;
    public int auto_aim_up;
    public int auto_aim_down;

    [Header("Visuals")]


    [Header("References")]
    private RectTransform crosshair;
    private ParticleSystem ps;
    private ThunderStatsLoader thunder;

    #endregion

    void Start()
    {
        thunder = GetComponent<ThunderStatsLoader>();
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
        var (turretStats, turretFixed) = thunder.GetFullStats(turretName, turretMod);
        price = turretStats.price;
        min_damage = turretStats.min_damage;
        max_damage = turretStats.max_damage;
        impact_force = turretStats.impact_force;
        splash_impact_force = turretStats.splash_impact_force;
        recoil = turretStats.recoil;
        reload_time = turretStats.reload_time;
        rotation_speed = turretStats.rotation_speed;
        rotation_acceleration = turretStats.rotation_acceleration;
        min_damage_range = turretStats.min_damage_range;
        max_damage_range = turretStats.max_damage_range;
        weak_damage_percent = turretFixed.weak_damage_percent;
        max_damage_radius = turretFixed.max_damage_radius;
        min_damage_radius = turretFixed.min_damage_radius;
        weak_damage_percent_splash = turretFixed.weak_damage_percent_splash;
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
}
