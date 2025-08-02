using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.FilePathAttribute;

public class AiMoveCloser : MonoBehaviour
{
    AIMovement aIMovement;
    AITurretRotation aITurretRotation;
    private float maxTurn;

    public void MoveCloser(float turnSpeed, float accelFactor, float stoppingFactor, float speed,
                                  Rigidbody rb, NavMeshAgent location, TurretControlBase turretBase,
                              Transform Turret, string hullName, string turretName, float rotationSpeed,
                                                                                       Transform player)
    {
        aIMovement = GetComponent<AIMovement>();
        aITurretRotation = GetComponent<AITurretRotation>();
        if (location == null || player == null) return;

        Vector3 targetPos = player.position;
        Vector3 toTarget = targetPos - transform.position;
        float angleToTarget = Vector3.SignedAngle(transform.forward, toTarget, Vector3.up);
        maxTurn = turnSpeed * Time.fixedDeltaTime;
        location.SetDestination(targetPos);

        aIMovement.MoveTowardDirection(Mathf.Abs(angleToTarget) > 90, speed, location, targetPos, rb, maxTurn);
        
        aITurretRotation.TurretRotation(Turret, hullName, turretName, rotationSpeed,
            turretBase, angleToTarget);
       
    }
}
