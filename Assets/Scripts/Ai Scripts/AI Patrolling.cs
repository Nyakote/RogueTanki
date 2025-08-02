using UnityEngine;
using static UnityEditor.FilePathAttribute;
using UnityEngine.AI;

public class AiPatrolling : MonoBehaviour
{
    Vector3 randomPosition;
    float walkToPointRange = 50f;

    float angle;
    float maxTurn;

    float moveStartTime;
    float maxTimeToReach;
    float cooldownStartTime;
    float cooldownTime;


    public bool walkToPos;
    public bool finishedMoving;
    AIMovement aIMovement;
    AITurretRotation aITurretRotation;
    

    public void PatrollingMovement(float turnSpeed, float accelFactor,float stoppingFactor, float speed, 
                                  Rigidbody rb, NavMeshAgent navMeshAgent, TurretControlBase turretBase, 
                              Transform Turret, string hullName, string turretName, float rotationSpeed)
    {

        aIMovement = GetComponent<AIMovement>();
        aITurretRotation = GetComponent<AITurretRotation>();

        aITurretRotation.TurretRotation(Turret, hullName, turretName, rotationSpeed, 
            turretBase,null);

        if (!finishedMoving)
        {
            if (!walkToPos)
            {
                SearchForPoint(speed, navMeshAgent);
            }

            if (walkToPos)
            {
                if (Time.time - moveStartTime > maxTimeToReach)
                {
                    walkToPos = false;
                    finishedMoving = true;
                    cooldownStartTime = Time.time;
                    MovingCooldown();
                    return;
                }
                angle = Vector3.SignedAngle(transform.forward, randomPosition - transform.position,Vector3.up);
                maxTurn = turnSpeed * Time.fixedDeltaTime;

                accelFactor = Mathf.MoveTowards(accelFactor, 1f, Time.fixedDeltaTime);

                aIMovement.MoveTowardDirection(Mathf.Abs(angle) > 90, speed,
                    navMeshAgent, randomPosition, rb, maxTurn);


                float distance = Vector3.Distance(transform.position, randomPosition);
                if (distance < 1f)
                {
                    walkToPos = false;
                    finishedMoving = true;
                    cooldownStartTime = Time.time;
                    MovingCooldown();
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
            }
            MovingCooldown();
        }
    }

    

    void SearchForPoint(float speed, NavMeshAgent navMeshAgent)
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
            navMeshAgent.SetDestination(randomPosition);
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
    }
}
