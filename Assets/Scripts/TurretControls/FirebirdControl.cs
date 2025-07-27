using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class FirebirdCotrol : MonoBehaviour
{

    #region 

    [Header("Basic Turret Stats")]
    private string turretName = "Firebird";
    private string turretMod;
    private int price;
    public int damage;
    public float rotationSpeed;
    public float rotationAcceleration;

    [Header("Turret Stats")]
    public float reload_time;
    public float burn_damage;
    public float min_damage_range;
    public float max_damage_range;
    public int energy_capacity;
    public int energy_consumption;
    public int weak_damage_percent;
    public float cone_angle;

    [Header("Visuals")]
    

    [Header("References")]
    private RectTransform crosshair;
    private ParticleSystem ps;
    private FirebirdStatsLoader firebird;

    #endregion

    void Start()
    {
        firebird = GetComponent<FirebirdStatsLoader>();
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
                transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,rotationSpeed * Time.deltaTime);

            }
        }
    }
  
    private void StatsSetter()
    {
        turretMod = transform.parent.parent.parent.name.Split(' ')[4];
        var(turretStats, turretFixed) = firebird.GetFullStats(turretName, turretMod);
        price = turretStats.price;
        damage = turretStats.damage;
        rotationSpeed = turretStats.rotation_speed;
        rotationAcceleration = turretStats.rotation_acceleration;

        reload_time = turretStats.reload_time;
        burn_damage = turretStats.burn_damage;
        min_damage_range = turretStats.min_damage_range;
        max_damage_range = turretStats.max_damage_range;
        energy_capacity = turretFixed.energy_capacity;
        energy_consumption = turretFixed.energy_consumption;
        weak_damage_percent = turretFixed.weak_damage_percent;
        cone_angle = turretFixed.cone_angle;

        var shape = ps.shape;
        shape.angle = cone_angle;
        rotationSpeed = rotationSpeed / 10f;
    }
    public void OnShoot(InputValue value)
    {
        if (transform.tag == "Player")
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
}
