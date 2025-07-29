using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

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
    [SerializeField] float sightRange = 30f;
    private bool walkToPos;
    Vector3 randomPosition;
    bool finishedMoving = false;
    float cooldownTime = 3f;
    Transform player;
    NavMeshAgent location;
    LayerMask playerLayer;
    Rigidbody rb;
    HullStatsLoader HSL;
    SmokyStatsLoader TSL;
    float cooldownStartTime;
    Vector3 locationScale;
    float walkToPointRange = 100;
    LayerMask whatIsGround;
    float angleDebuffer;
    float maxTimeToReach = 10f;
    float moveStartTime;

    #endregion
    #region
    Transform muzzle;
    float range = 20;
    bool isThereEnemy = false;
    Collider bestHit;
    Vector3 directionToEnemy;
    float closestEnemy;
    Vector3 finalToTarget;
    float timeBetweenRot = 5f;
    float startRotTime = 0f;
    bool canRotate = true;
    Vector3 hullPos;
    float currentTurretYaw;
    float startLocalYaw;
    float hullYaw;
    float currentYaw;
    float targetYaw;
    bool rotating = false;
    Transform Turret;
    public TurretControlBase turretBase;
    Vector3 direction;
    float angle;
    float maxTurn;
    bool initialized =false;
    #endregion
    #region
    TurretControlBase TurretControl;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        HSL = GetComponent<HullStatsLoader>();
        TSL = GetComponentInChildren<SmokyStatsLoader>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentYaw = transform.eulerAngles.y;
        startLocalYaw = Mathf.DeltaAngle(hullYaw, currentYaw);
        closestEnemy = range;
        location = GetComponent<NavMeshAgent>();
        location.updateRotation = true;
        location.updatePosition = false;
        whatIsGround = LayerMask.GetMask("Default");
        playerLayer = LayerMask.GetMask("Player");
        hullPos = transform.position;
        hullYaw = transform.rotation.eulerAngles.y;
        

    }

    void LateUpdate()
    {
        if (location != null)
        {
            location.nextPosition = transform.position;
        }
    }
    private void FixedUpdate()
    {
        if (!initialized) return;
        if (finishedMoving) MovingCooldown();
       


        isInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!isInAttackRange && !isTargetInSight)
        {
            Patrolling();
        }

     
        if (isTargetInSight && !isInAttackRange) { MoveCloser(); }
        // if (isInAttackRange && isTargetInSight) { AttackPlayer(); }
    }
    void Patrolling()
    {
        TurretRotation();
        if (!finishedMoving)
        {
            if (!walkToPos)
            {
                SearchForPoint();
                
            }

            if (walkToPos)
            {                
                if (Time.time - moveStartTime > maxTimeToReach)
                {
                    walkToPos = false;
                    finishedMoving = true;
                    cooldownStartTime = Time.time;
                }

                float turnDirection = Mathf.Sign(angle);
                maxTurn = turnSpeed * Time.fixedDeltaTime;
                float turnAmount = Mathf.Clamp(angle, -maxTurn, maxTurn);

                accelFactor = Mathf.MoveTowards(accelFactor, 1f, Time.fixedDeltaTime);
                MoveTowardDirection(Mathf.Abs(angle) > 90, randomPosition);

                float distance = Vector3.Distance(transform.position, randomPosition);
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

    void MoveTowardDirection(bool backward, Vector3 direction)
    {
        Vector3 moveDirection;

        if (location.hasPath && location.path.corners.Length > 1)
            direction = location.path.corners[1] - transform.position;
        else
            direction = randomPosition - transform.position;

        Vector3 facingDirection = direction;
        moveDirection = backward ? -transform.forward : transform.forward;

        float desiredYaw = Quaternion.LookRotation(facingDirection).eulerAngles.y;
        float angle = Mathf.DeltaAngle(transform.eulerAngles.y, desiredYaw);

        float turnAmount = Mathf.Clamp(angle, -maxTurn, maxTurn);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turnAmount, 0f));

        float absAngle = Mathf.Abs(angle);
        Debug.Log(absAngle);
        float movementSpeed =
            absAngle < 30 ? speed :
            absAngle < 60 ? speed / 2f :
            absAngle < 90 ? speed / 4f : 0f;

       
            Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
            if (!Physics.Raycast(rayOrigin, moveDirection, 1.5f, LayerMask.GetMask("Props")))
            {
                Vector3 move = moveDirection * movementSpeed * Time.fixedDeltaTime;
                rb.MovePosition(rb.position + move);
            }
        
    }

    void SearchForPoint()
    {
        float randomX = UnityEngine.Random.Range(-walkToPointRange, walkToPointRange);
        float randomZ = UnityEngine.Random.Range(-walkToPointRange, walkToPointRange);
        randomPosition = new Vector3(randomX, 0, randomZ) + transform.position;

        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            randomPosition = hit.position;
            if (float.IsNaN(randomPosition.x) || float.IsNaN(randomPosition.y) || float.IsNaN(randomPosition.z))
            {
                Debug.LogError("Sampled NavMesh position is NaN!");
                walkToPos = false;
                return;
            }
            location.SetDestination(randomPosition);
            moveStartTime = Time.time;
            maxTimeToReach = Vector3.Distance(randomPosition, transform.position) / speed;


            walkToPos = true;
        }
        else
        {
            walkToPos = false;
        }
    }
    void MovingCooldown()
    {
        if (Time.time - cooldownStartTime >= cooldownTime)
        {
            finishedMoving = false;
            walkToPos = false;
            cooldownTime = UnityEngine.Random.Range(1, 10);
        }
        Debug.Log("Finisihed");
    }
    void AttackPlayer()
    {
        Quaternion angle = Quaternion.LookRotation(player.position - transform.position);
        // turret or body rotation logic here
    }
    void StatsSetterHull()
    {
        HullMod hullStats = HSL.GetStats(hullName, hullMod);
        int currentMod = Convert.ToInt32(hullMod.Substring(1));
        float speedBuff = 2f + 0.2f * currentMod;

        HP = hullStats.HP;
        speed = hullStats.SPD;
        turnSpeed = hullStats.ROTSPD*10;
        weight = hullStats.WEIGHT;
        power = hullStats.PWR;

        if (location != null)
        {
            location.speed = speed;
        }
    }
    public void Initialize(int hull, int turret, string Nhull, string Nturret, float radius)
    {
        
        location.radius = radius;

        hullMod = "M" + hull;
        turretMod = "M" + turret;
        hullName = Nhull;
        turretName = Nturret;
        muzzle = transform.Find($"{hullName}(Clone)/mount/{turretName}(Clone)/muzzle");
        Turret = transform.Find($"{hullName}(Clone)/mount/{turretName}(Clone)/");
        TurretControl = Turret.GetComponent<TurretControlBase>();
        turretBase = TurretControl?.GetTurretControl(turretName, turretMod);
           
        Setterer();
        initialized = true;
    }
    public void Setterer()
    {
        Invoke("StatsSetterHull", 0.5f);
        Invoke("TurretStatsSeter", 0.5f);
    }
    public void TargetDetection()
    {
        if (muzzle == null)
        {
            Debug.LogError("Muzzle not found!");
            return;
        }

        directionToEnemy = muzzle.forward;
        closestEnemy = range;
        finalToTarget = Vector3.zero;
        isThereEnemy = false;
        isTargetInSight = false;

        Collider[] hits = Physics.OverlapSphere(muzzle.position, range, playerLayer);

        foreach (Collider hit in hits)
        {
            Vector3 toTarget = hit.ClosestPoint(muzzle.position) - muzzle.position;
            float distanceToTarget = toTarget.magnitude;

            Vector3 flatForward = Vector3.ProjectOnPlane(muzzle.forward, Vector3.up).normalized;
            Vector3 flatToTarget = Vector3.ProjectOnPlane(toTarget, Vector3.up).normalized;
            float yaw = Vector3.SignedAngle(flatForward, flatToTarget, Vector3.up);

            if (Mathf.Abs(yaw) > 30f) continue;
            if (distanceToTarget >= closestEnemy) continue;

            closestEnemy = distanceToTarget;
            directionToEnemy = toTarget.normalized;
            finalToTarget = toTarget;
            bestHit = hit;
            isThereEnemy = true;
            isTargetInSight = true;
        }

        if (!isThereEnemy)
        {
            Debug.Log("No enemy.");
        }
    }

    public void TurretRotation()
    {
        if (Turret == null)
        {
            Turret = transform.Find($"{hullName}(Clone)/mount/{turretName}(Clone)/");
            if (Turret == null)
            {
                Debug.LogWarning("Turret not found at path. Skipping rotation.");
                return;
            }
        }

        if (!canRotate)
        {
            TurretCooldown();
            return;
        }

        if (!rotating)
        {
            targetYaw = UnityEngine.Random.Range(-180f, 180f);
            rotating = true;
            startRotTime = Time.time;
        }
        Quaternion currentRotation = Turret.transform.localRotation;
        Quaternion desiredRotation = Quaternion.Euler(0f, targetYaw, 0f);

        Turret.transform.localRotation = Quaternion.RotateTowards(currentRotation, desiredRotation, rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(currentRotation, desiredRotation) < 0.5f)
        {
            canRotate = false;
            rotating = false;
            TargetDetection();
        }
    }
    void RotateTurretTowardPlayer()
    {
        if (Turret == null || player == null) return;

        Vector3 toPlayer = player.position - Turret.position;
        toPlayer.y = 0f; 

        if (toPlayer.magnitude < 0.1f) return;

        Quaternion desiredRotation = Quaternion.LookRotation(toPlayer);
        Turret.rotation = Quaternion.RotateTowards(
            Turret.rotation,
            desiredRotation,
            rotationSpeed * Time.deltaTime
        );
    }
    public void MoveCloser()
    {
        if (location == null || player == null) return;

        Vector3 targetPos = player.position;
        Vector3 toTarget = targetPos - transform.position;
        float angleToTarget = Vector3.SignedAngle(transform.forward, toTarget, Vector3.up);

        location.SetDestination(targetPos);
        MoveTowardDirection(Mathf.Abs(angleToTarget) > 90, targetPos);

        RotateTurretTowardPlayer();
    }



    private void TurretCooldown()
    {
        if (Time.time - startRotTime >= timeBetweenRot)
        {
            canRotate = true;
        }
    }
    private void TurretStatsSeter()
    {
        rotationSpeed = turretBase.GetRotateSpeed()*10;
    }

}

