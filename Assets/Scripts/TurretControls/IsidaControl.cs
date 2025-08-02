using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class IsidaControl : TurretControlBase
{

    #region 

    [Header("Basic Turret Stats")]
    private string turretName = "Isida";
    private string turretMod;
    private int price;
    public int damage;
    public float rotationSpeed;
    public float rotationAcceleration;


    [Header("Turret Stats")]
    public int healing;
    public float range;
    public float self_healing_percent;
    public float reload_time;
    public int energy_capacity;
    public float idle_energy_consumption;
    public float attack_energy_consumption;
    public float healing_energy_consumption;
    public float cone_angle;

    [Header("Visuals")]


    [Header("References")]
    private RectTransform crosshair;
    private ParticleSystem ps;
    private IsidaStatsLoader isida;

    #endregion

    void Start()
    {
        isida = GetComponent<IsidaStatsLoader>();
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
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            }
        }
    }

    private void StatsSetter()
    {
        turretMod = transform.parent.parent.parent.name.Split(' ')[4];
        var (turretStats, turretFixed) = isida.GetFullStats(turretName, turretMod);
        price = turretStats.price;
        damage = turretStats.damage;
        rotationSpeed = turretStats.rotation_speed;
        rotationAcceleration = turretStats.rotation_acceleration;

        reload_time = turretStats.reload_time;
        energy_capacity = turretFixed.energy_capacity;
        cone_angle = turretFixed.cone_angle;
        healing = turretStats.healing;
        self_healing_percent = turretStats.self_healing_percent;
        idle_energy_consumption = turretFixed.idle_energy_consumption;
        attack_energy_consumption = turretFixed.attack_energy_consumption;
        healing_energy_consumption = turretFixed.healing_energy_consumption;


        var shape = ps.shape;
        shape.angle = cone_angle;
        rotationSpeed = rotationSpeed / 10f;
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
        health.TakeDamage(Mathf.RoundToInt(UnityEngine.Random.Range(damage, damage)));

        Debug.Log("Particle collided with " + other.tag);
    }
    public override float GetRotateSpeed() => rotationSpeed;
    public override float MinDamage() => damage;
    public override float MaxDamage() => damage;
}
