using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UIElements;


public class TwinsControl : MonoBehaviour
{

    #region 

    [Header("Basic Turret Stats")]
    public string turretName = "Twins";
    public string turretMod;
    public int price;
    public float min_damage;
    public float max_damage;
    public float impact_force;
    public float recoil;
    public float rotation_speed;
    public float rotation_acceleration;
    public float min_damage_range;
    public float projectile_speed;
    public float weak_damage_percent;
    public float reload_time;
    public float max_damage_range;
    public float projectile_radius;

    [Header("Turret Stats")]
    public int auto_aim_up;
    public int auto_aim_down;
    public float timeBetweenShots = 0.2f;
    public float timepassed = 0f;
    bool right = true;
    [SerializeField] Transform RightC;
    [SerializeField] Transform LeftC;

    [Header("References")]
    private RectTransform crosshair;
    private ParticleSystem ps;
    private TwinsStatsLoader twins;
    private bool isHolding = false;
    #endregion

    void Start()
    {
        twins = GetComponent<TwinsStatsLoader>();
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
        if (isHolding && Time.time - timepassed >= timeBetweenShots)
        {
            if (right)
            {
                timepassed = Time.time;
                right = false;
                ps.transform.position = RightC.transform.position;
                ps.Emit(1);
            }
            else
            {
                timepassed = Time.time;
                right = true;
                ps.transform.position = LeftC.transform.position;
                ps.Emit(1);
            }
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
        var (turretStats, turretFixed) = twins.GetFullStats(turretName, turretMod);
        price = turretStats.price;
        min_damage = turretStats.min_damage;
        max_damage = turretStats.max_damage;
        impact_force = turretStats.impact_force;
        recoil = turretStats.recoil;
        rotation_speed = turretStats.rotation_speed;
        rotation_acceleration = turretStats.rotation_acceleration;
        min_damage_range = turretStats.min_damage_range;
        projectile_speed = turretStats.projectile_speed;
        weak_damage_percent = turretStats.weak_damage_percent;
        reload_time = turretStats.reload_time;
        max_damage_range = turretStats.max_damage_range;
        projectile_radius = turretStats.projectile_radius;
        auto_aim_up = turretFixed.auto_aim_up;
        auto_aim_down = turretFixed.auto_aim_down;



        var pjspeed = ps.main.startSpeed;
        pjspeed = projectile_speed;
        rotation_speed = rotation_speed / 10f;
    }
    public void OnShoot(InputValue value)
    {
        if (transform.tag == "Player")
        {
            if (value.isPressed)
            {
                isHolding = true;
            }
            else
            {
                isHolding = false;
            }
        }

    }
}
