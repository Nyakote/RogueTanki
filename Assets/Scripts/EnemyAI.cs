using System;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    #region Stats
    [Header("Hull Stats")]
    public string hullMod;
    public string hullName;

    public float HP;
    public float speed;
    public float turnSpeed;
    public float weight;
    public float power;
    [Header("Turret Stats")]
    public string turretMod;
    public string turretName;

    public float cost;
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


    float stoppingFactor = 1f;
    float turningstopFactor = 1f;
    float accelFactor = 3;
    float turnAccelFactor = 3;
    #endregion

    #region Ai_Control
    [Header("AI Control")]
    private bool isInAttackRange;
    private bool isTargetInSight;
    [SerializeField] float attackRange = 10f;
    [SerializeField] float sightRange = 20f;
    private bool walkToPos;
    Vector3 randomPosition;
    bool finishedMoving = false;
    float cooldownTime = 2f;
    Transform player;
    Transform location;
    LayerMask playerLayer;
    Rigidbody rb;
    HullStatsLoader HSL;
    SmokyStatsLoader TSL;
    float cooldownStartTime;
    Vector3 locationScale;

    #endregion
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        HSL = GetComponent<HullStatsLoader>();
        TSL = GetComponentInChildren<SmokyStatsLoader>();
        playerLayer = LayerMask.GetMask("Player");
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void FixedUpdate()
    {
        if (finishedMoving) { MovingCooldown(); }

        isInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        isTargetInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);

        if (!isInAttackRange && !isTargetInSight) { Patrolling(); }
        // if (isTargetInSight && !isInAttackRange) { MoveCloser(); }
        // if (isInAttackRange && isTargetInSight) { AttackPlayer(); }

    }
    void Patrolling()
    {
        if (!finishedMoving)
        {
            if (!walkToPos)
            {
                SearchForPoint();
            }

            if (walkToPos)
            {
                Vector3 direction = (randomPosition - transform.position);
                direction.y = 0f;
                float distance = direction.magnitude;

                Vector3 dirNormalized = direction.normalized;
                float angle = Vector3.SignedAngle(transform.forward, dirNormalized, Vector3.up);

                float turnDirection = Mathf.Sign(angle);
                float turnAmount = turnDirection * turnSpeed * Time.fixedDeltaTime;
                if (Mathf.Abs(angle) < Mathf.Abs(turnAmount)) turnAmount = angle;
                rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turnAmount, 0f));

                if (Mathf.Abs(angle) < 10f)
                {
                    accelFactor = Mathf.Max(1f, accelFactor - 0.2f);
                    Vector3 forwardMove = transform.forward * (speed / accelFactor) * Time.fixedDeltaTime;
                    rb.MovePosition(rb.position + forwardMove);
                }

                if (distance < 1f)
                {
                    walkToPos = false;
                    finishedMoving = true;
                    cooldownStartTime = Time.time;
                }
            }
        }
        else
        {

            if (stoppingFactor != speed)
            {
                stoppingFactor += 0.2f;
                if (stoppingFactor > speed) stoppingFactor = speed;

                Vector3 forwardMove = transform.forward * speed / stoppingFactor * Time.fixedDeltaTime;
                rb.MovePosition(rb.position + forwardMove);

                accelFactor = 3f;
            }
        }
    }

    void SearchForPoint()
    {
        if (location == null)
        {
            Debug.LogWarning("SearchForPoint called before location was assigned!");
            return;
        }

        float RandomPlaceX = UnityEngine.Random.Range(-locationScale.x / 2, locationScale.x / 2);
        float RandomPlaceZ = UnityEngine.Random.Range(-locationScale.z / 2, locationScale.z / 2);
        randomPosition = new Vector3(RandomPlaceX, transform.position.y, RandomPlaceZ) + location.position;
        walkToPos = true;
    }


    void MoveCloser()
    {
    }

    void AttackPlayer()
    {
        Quaternion angle = Quaternion.LookRotation(player.position - transform.position);

    }

    void StatsSetterHull()
    {
        HullMod hullStats = HSL.GetStats(hullName, hullMod);
        int currentMod = Convert.ToInt32(hullMod.Substring(1));
        float speedBuff = 2 + 0.2f * currentMod;

        HP = hullStats.HP;
        speed = hullStats.SPD * speedBuff;
        turnSpeed = hullStats.ROTSPD;
        weight = hullStats.WEIGHT;
        power = hullStats.PWR;
    }

    

    void MovingCooldown()
    {
        if (Time.time - cooldownStartTime >= 2f)
            finishedMoving = false;
    }

    public void Initialize(Transform lc, int hull, int turret, string Nhull, string Nturret)
    {
        Debug.Log("Initialize called with parameters: " + lc + ", " + hull + ", " + turret + ", " + Nhull + ", " + Nturret);
        location = lc;
        locationScale = location.localScale;
        hullMod = "M" + hull.ToString();
        turretMod = "M" + turret.ToString();
        hullName = Nhull;
        turretName = Nturret;
        Setterer();
        Debug.Log($"Setterer name {hullName} {turretName}");
    }
    public void Setterer()
    {
        Debug.Log($" {hullName} {turretName} ");
        Invoke("StatsSetterHull", 0.5f);
        Invoke("StatsSetterTurret", 0.5f);
    }

}
