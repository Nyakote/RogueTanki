using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class ShaftControl : TurretControlBase
{

    #region 

    [Header("Basic Turret Stats")]
    private string turretName = "Shaft";
    private string turretMod;
    public int price;
    public int scoped_min_damage;
    public int scoped_max_damage;
    public int arcade_min_damage;
    public int arcade_max_damage;
    public float arcade_impact_force;
    public float scoped_impact_force;
    public float recoil;
    public float arcade_reload_time;
    public float energy_consumption;
    public float rotation_speed;
    public float rotation_acceleration;
    public float horizontal_aim_speed;
    public float vertical_aim_speed;
    public float energy_per_shot;
    public float energy_recharge;
    public float piercing_percent;


    [Header("Turret Stats")]
    public int energy_capacity;
    public float scoped_exit_delay;
    public float scoped_entry_delay;
    public float scoped_rotation_acceleration;
    public int auto_aim_up;
    public int auto_aim_down;
    public int min_foliage_transparency_radius;
    public int max_foliage_transparency_radius;
    public int fov_min;
    public int fov_max;
    public float slowdown_start_point;
    public float slowdown_end_point;
    public float min_slowdown_factor;

    [Header("Visuals")]


    [Header("References")]
    private RectTransform crosshair;
    private ParticleSystem ps;
    private ShaftStatsLoader shaft;

    #endregion

    void Start()
    {
        shaft = GetComponent<ShaftStatsLoader>();
        crosshair = GameObject.Find("Crosshair").GetComponent<RectTransform>();
        ps = GetComponentInChildren<ParticleSystem>();
        Invoke("StatsSetter", 0.2f);
    }

    void Update()
    {
        if (transform.parent.parent.parent.tag == "Player")
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
        var (turretStats, turretFixed) = shaft.GetFullStats(turretName, turretMod);
      price= turretStats.price;
        scoped_min_damage = turretStats.scoped_min_damage;

        scoped_max_damage = turretStats.scoped_max_damage;
        arcade_min_damage = turretStats.arcade_min_damage;
        arcade_max_damage = turretStats.arcade_max_damage;
        arcade_impact_force = turretStats.arcade_impact_force;
        scoped_impact_force = turretStats.scoped_impact_force;
        recoil = turretStats.recoil;
        arcade_reload_time = turretStats.arcade_reload_time;
        energy_consumption = turretStats.energy_consumption;
        rotation_speed = turretStats.rotation_speed;
        rotation_acceleration = turretStats.rotation_acceleration;
        horizontal_aim_speed = turretStats.horizontal_aim_speed;
        vertical_aim_speed = turretStats.vertical_aim_speed;
        energy_per_shot = turretStats.energy_per_shot;
        energy_recharge = turretStats.energy_recharge;
        piercing_percent = turretStats.piercing_percent;
        energy_capacity = turretFixed.energy_capacity;
        scoped_exit_delay = turretFixed.scoped_exit_delay;
        scoped_entry_delay = turretFixed.scoped_entry_delay;
        scoped_rotation_acceleration = turretFixed.scoped_rotation_acceleration;
        auto_aim_up = turretFixed.auto_aim_up;
        auto_aim_down = turretFixed.auto_aim_down;
        min_foliage_transparency_radius = turretFixed.min_foliage_transparency_radius;
        fov_max = turretFixed.fov_max;
        fov_min = turretFixed.fov_min;
        max_foliage_transparency_radius = turretFixed.max_foliage_transparency_radius;
        slowdown_start_point = turretFixed.slowdown_start_point;
        slowdown_end_point = turretFixed.slowdown_end_point;
        min_slowdown_factor = turretFixed.min_slowdown_factor;

        rotation_speed = rotation_speed / 10f;
    }
    public void OnShoot(InputValue value)
    {
        if (transform.parent.parent.parent.tag == "Player")
        {
            var emmit = ps.emission;
            if (value.isPressed)
            {
                emmit.enabled = true;
            }
            else
            {
                emmit.enabled = false;
            }
        }
    }
    public override void HandleParticleCollision(GameObject other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Enemy")) return;

        HealthComponent health = other.GetComponent<HealthComponent>();
        health.TakeDamage(Mathf.RoundToInt(UnityEngine.Random.Range(arcade_min_damage, arcade_max_damage)));

        Debug.Log("Particle collided with " + other.tag);
    }
    public override float GetRotateSpeed() => rotation_speed;
    public override float MinDamage() => arcade_min_damage;
    public override float MaxDamage() => arcade_max_damage;
}
