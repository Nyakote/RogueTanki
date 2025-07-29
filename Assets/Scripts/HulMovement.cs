using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using System;


public class HullMovement : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] string mod = "M0";
    public float HP;
    public float speed;
    public float turnSpeed;
    public float weight;
    public float power;
    float stoppingFactor = 1f;
    float turningstopFactor = 1f;
    float accelFactor = 3;
    float turnAccelFactor = 3;

    [Header("Movement Settings")]
    float lastMovementInput;
    float lastTurnInput;
    float speedieneess;
    LayerMask cantGo;
    private bool isTouchingObstacle = false;

    [Header("References")]
    private Rigidbody rb;
    private HullStatsLoader hsl;
    private float moveInputZ = 0f;
    private float turnInput = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hsl = GetComponent<HullStatsLoader>();
        cantGo = LayerMask.GetMask("Props"); 
        Invoke("StatsSetter", 0.5f);
    }

    public void OnMovement(InputValue value)
    {
        Vector3 movement = value.Get<Vector3>();
        moveInputZ = movement.z;
    }
    public void OnTurning(InputValue value)
    {
        turnInput = value.Get<float>();
    }

    private void FixedUpdate()
    {
        MovingCall();

    }

    void StatsSetter()
    {
        string name = transform.name.Split(' ')[1];

        HullMod hullStats = hsl.GetStats(name, mod);
        int currentMod = Convert.ToInt32(mod.Substring(1));
        float speedBuff = 2 + 0.2f * currentMod;

        HP = hullStats.HP;
        speed = hullStats.SPD;
        speedieneess = speed;
        turnSpeed = hullStats.ROTSPD;
        weight = hullStats.WEIGHT;
        power = hullStats.PWR;
    }
    public void HullMoving()
    {
        if (isTouchingObstacle) return;
        if (accelFactor > 1) accelFactor -= 0.05f;
        Vector3 forwardMove = transform.forward * moveInputZ * (speed / accelFactor) * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove);
        stoppingFactor = 1f;
        lastMovementInput = moveInputZ;
    }

    public void HullSlowing()
    {
        if (stoppingFactor != speed)
        {
            stoppingFactor += 0.1f;
            if (stoppingFactor > 4) stoppingFactor = speed;
            Vector3 forwardMove = lastMovementInput * transform.forward * speed / stoppingFactor * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + forwardMove);
            accelFactor = 3;
        }
    }

    public void HullTurning()
    {
        if (turnAccelFactor > 1) turnAccelFactor -= 0.1f;
        float turnAmount = turnInput * (turnSpeed / turnAccelFactor) * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turnAmount, 0f));
        turningstopFactor = 1f;
        lastTurnInput = turnInput;
    }
    public void HullTurrningSlowing()
    {
        if (turningstopFactor != turnSpeed)
        {
            turningstopFactor += 0.3f;
            if (turningstopFactor > 3) turningstopFactor = turnSpeed;
            float turnAmount = lastTurnInput * turnSpeed / turningstopFactor * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turnAmount, 0f));
        }
        turnAccelFactor = 3;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & cantGo) != 0)
        {
            isTouchingObstacle = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & cantGo) != 0)
        {
            isTouchingObstacle = false;
        }
    }


    private void MovingCall()
    {
        if (moveInputZ != 0)
        {
            HullMoving();
        }
        else
        {
            HullSlowing();
        }

        if (turnInput != 0)
        {
            HullTurning();
        }
        else
        {
            HullTurrningSlowing();
        }
    }

}
