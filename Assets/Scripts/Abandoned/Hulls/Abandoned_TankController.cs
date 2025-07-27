/*using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class TankController : MonoBehaviour
{
    #region Variables
    [Header("Hull Stats")]
    [SerializeField] string hullName;
    [SerializeField] string hullMod;
    [SerializeField] GroundChecker rightTrack;
    [SerializeField] GroundChecker leftTrack;
    // private float hp;

    [Header("Hull Movement")]
    public float speed;
    public float turnSpeed;
    public float power;
    public float turnPower;
    public float coeficient;
    public float timeToMaxSpeed;
    private float velocitySmoothRef;
    private float breakSmoothRef;
    private float turnVelocityRef;


    [Header("temp")]
    public float inputMovement;
    public float inputTurning;

    [Header("Ground Check bools")]
    public bool isFullyGrounded = false;
    public bool isLeftTrackFullyGrounded = false;
    public bool isRightTrackFullyGrounded = false;
    public bool isLeftTrackOneGrounded = false;
    public bool isRightTrackOneGrounded = false;
    public bool isThereAtLeastOneTrackGrounded = false;
    public float originalPower;

    [Header("Importing scripts")]
    private Rigidbody rb;
    private TankStatsLoader loader;

  
    #endregion
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        loader = Object.FindFirstObjectByType<TankStatsLoader>();
        StatsSetter();
        coeficient = speed / power * 3;
    }

    void Update()
    {
        inputMovement = Input.GetAxis("Vertical");
        inputTurning = Input.GetAxis("Horizontal");

    }

    void FixedUpdate()
    {
        IsGrounded();
        PowerCalculations();
        if (inputMovement != 0)
        {
                HullAcceleration();
        }
        else
        {
            HullBreaking();
        }
        if (inputTurning != 0)
        {
            if (isFullyGrounded)
            {
                HullTurning();
            }
        }

    }

    private void StatsSetter()
    {
        HullMod hullStats = loader.GetStats(hullName, hullMod);
        if (hullStats != null)
        {  
            rb.mass = hullStats.Weight;
            speed = hullStats.Speed*2;
            turnSpeed = NormalizeTurnSpeed(hullStats.TurnSpeed, rb.mass);
            power = hullStats.Power;
            turnPower = power / 2;
            rb.maxAngularVelocity = turnSpeed;
            rb.maxLinearVelocity = speed;
            originalPower = power;
            // Debug.Log($"Hull stats set: Speed={speed}, TurnSpeed={turnSpeed}, Weight={rb.mass}, Power={power}, TurnPower={turnPower} for {hullName} {hullMod}");
        }
        else
        {
            Debug.LogError("Hull stats not found for TankHull Level1");
        }

    }

    private float NormalizeTurnSpeed(float originalDeg, float weight)
    {
        float originalRad = originalDeg * Mathf.Deg2Rad;
        float normalized = Mathf.Pow(originalRad, 0.9f) * Mathf.Sqrt(1000f / weight);
        return Mathf.Clamp(normalized, 0.3f, 2.5f);
    }

    private void HullAcceleration()
    {
        timeToMaxSpeed = speed / power * coeficient;
        float targetSpeed = inputMovement * speed;
        float currentSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
        float speedDifference = targetSpeed - currentSpeed;
        float velocity = Vector3.Dot(rb.linearVelocity, transform.forward);
        float target = inputMovement * speed;
        float smoothSpeed = Mathf.SmoothDamp(velocity, target, ref velocitySmoothRef, timeToMaxSpeed);
        float delta = smoothSpeed - velocity;
        rb.AddForce(transform.forward * delta, ForceMode.VelocityChange);

    }

    private void HullBreaking()
    {
        float currentSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
        float smoothSpeed = Mathf.SmoothDamp(currentSpeed, 0f, ref breakSmoothRef, 0.3f);
        float delta = smoothSpeed - currentSpeed;
        rb.AddForce(transform.forward * delta, ForceMode.VelocityChange);
    }

    private void HullTurning()
    {
        float currentTurnSpeed = Vector3.Dot(rb.angularVelocity, Vector3.up);
        float targetTurnSpeed = inputTurning * turnSpeed;

        float smoothedTurnSpeed = Mathf.SmoothDamp(currentTurnSpeed, targetTurnSpeed, ref turnVelocityRef, 0.2f);
        float delta = smoothedTurnSpeed - currentTurnSpeed;

        rb.AddTorque(Vector3.up * delta * turnPower, ForceMode.VelocityChange);
    }

    private void IsGrounded()
    {
        isLeftTrackFullyGrounded = (leftTrack.RearCheck ? 1 : 0) + (leftTrack.MidCheck ? 1 : 0) + (leftTrack.FrontCheck ? 1 : 0) >= 2;
        isRightTrackFullyGrounded = (rightTrack.RearCheck ? 1 : 0) + (rightTrack.MidCheck ? 1 : 0) + (rightTrack.FrontCheck ? 1 : 0) >= 2;

        isLeftTrackOneGrounded = (leftTrack.RearCheck ? 1 : 0) + (leftTrack.MidCheck ? 1 : 0) + (leftTrack.FrontCheck ? 1 : 0) >= 1;
        isRightTrackOneGrounded = (rightTrack.RearCheck ? 1 : 0) + (rightTrack.MidCheck ? 1 : 0) + (rightTrack.FrontCheck ? 1 : 0) >= 1;

        isThereAtLeastOneTrackGrounded = isLeftTrackOneGrounded || isRightTrackOneGrounded;
        isFullyGrounded = isLeftTrackFullyGrounded && isRightTrackFullyGrounded;
    }
    private void PowerCalculations()
    {
        if (isFullyGrounded)
        {
            power = originalPower;
        }
        else if (isThereAtLeastOneTrackGrounded)
        {
            power = originalPower * 0.5f;
        }
        else
        {
            power = 0f;
        }

    }
}

*/