using UnityEngine;
using UnityEngine.Rendering;

public class AutoAim : MonoBehaviour
{
    [Header("Auto Aim Stats")]
    private float range = 100f;
    private float minAngle = -12f;
    private float maxAngle = 9f;
    private float closestEnemy;
    private Vector3 directionToEnemy;
    private Vector3 finalToTarget;
    private bool isThereEnemy = false;
    Collider bestHit;

    [Header("References")]
    private Transform muzzle;
    private LayerMask layerMask;

    private void Start()
    {
        muzzle = transform.Find("muzzle");
        layerMask = LayerMask.GetMask("Enemy");
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

        Collider[] hits = Physics.OverlapSphere(muzzle.position, range, layerMask);

        foreach (Collider hit in hits)
        {
            Vector3 closestPoint = hit.ClosestPoint(muzzle.position);
            Vector3 toTarget = closestPoint - muzzle.position;
            float distanceToTarget = toTarget.magnitude;

            Vector3 flatForward = Vector3.ProjectOnPlane(muzzle.forward, Vector3.up).normalized;
            Vector3 flatToTarget = Vector3.ProjectOnPlane(toTarget, Vector3.up).normalized;
            float yaw = Vector3.SignedAngle(flatForward, flatToTarget, Vector3.up);
            if (Mathf.Abs(yaw) > 30f) continue;

            if (distanceToTarget < closestEnemy)
            {
                closestEnemy = distanceToTarget;
                directionToEnemy = toTarget.normalized;
                finalToTarget = toTarget;
                bestHit = hit;
                isThereEnemy = true;
            }
        }
        if (isThereEnemy)
        {
            if (finalToTarget.sqrMagnitude > 0.0001f)
            {
                Ray ray = new Ray(muzzle.position, finalToTarget.normalized);
                RaycastHit rayHit;

                if (bestHit.Raycast(ray, out rayHit, closestEnemy))
                {
                    Vector3 boundsCenter = bestHit.bounds.center;
                    Vector3 boundsExtents = bestHit.bounds.extents;

                    Vector3 localHit = bestHit.transform.InverseTransformPoint(rayHit.point);
                    Vector3 localCenter = bestHit.transform.InverseTransformPoint(boundsCenter);

                    if (Mathf.Abs(localHit.x - localCenter.x) <= boundsExtents.x &&
                        Mathf.Abs(localHit.z - localCenter.z) <= boundsExtents.z)
                    {
                        Vector3 pitchForward = Vector3.ProjectOnPlane(muzzle.forward, muzzle.right).normalized;
                        Vector3 pitchToTarget = Vector3.ProjectOnPlane(finalToTarget, muzzle.right).normalized;

                        float pitch = Vector3.SignedAngle(pitchForward, pitchToTarget, muzzle.right);

                        if (pitch >= minAngle && pitch <= maxAngle)
                        {
                            Debug.Log("Target can be hit!");
                        }
                        else
                        {
                            Debug.Log("Target out of vertical range!");
                        }
                    }
                    else
                    {
                        Debug.Log($" hit point is outside XZ bounds.");
                    }
                }


                else
                {
                    Debug.Log("Raycast did not hit the target.");
                }
            }
            else
            {
                Debug.LogWarning("finalToTarget is zero vector. Cannot create ray.");
            }
        }
        else
        {
            Debug.Log("No enemy.");
        }
        isThereEnemy = false;

    }
    private void OnDrawGizmosSelected()
    {
        if (muzzle == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(muzzle.position, range);

        Gizmos.color = Color.white;
        Gizmos.DrawRay(muzzle.position, directionToEnemy * closestEnemy);
        ;
    }


}