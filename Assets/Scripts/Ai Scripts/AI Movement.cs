    using UnityEngine;
    using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    public void MoveTowardDirection(bool backward, float speed,
        NavMeshAgent navMeshAgent, Vector3 position, Rigidbody rb, float maxTurn)
    {
        Vector3 direction;

        if (navMeshAgent.hasPath)
            direction = navMeshAgent.steeringTarget - transform.position;
        else
            direction = position - transform.position;


        direction.y = 0;
        if (direction.sqrMagnitude < 0.001f)
            return;

        direction.Normalize();

        Vector3 moveDirection = backward ? -transform.forward : transform.forward;
        float angle = Vector3.Angle(moveDirection, direction);

        Vector3 lookDirection = backward ? -direction : direction;
        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDirection);
            Quaternion newRot = Quaternion.RotateTowards(rb.rotation, targetRot, maxTurn);
            rb.MoveRotation(newRot);
        }

        float absAngle = Mathf.Abs(angle);
        float movementSpeed =
        absAngle < 30f ? speed :
        absAngle < 60f ? speed / 2f :
        absAngle < 80f ? speed / 4f : 0f;

        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        if (!Physics.Raycast(rayOrigin, moveDirection, 1.5f, LayerMask.GetMask("Props")))
        {
            Vector3 move = moveDirection * movementSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }

    }
}
