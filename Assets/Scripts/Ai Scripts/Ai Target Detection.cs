using UnityEngine;
using UnityEngine.UIElements;

public class AiTargetDetection : MonoBehaviour
{
    #region Variables

    [Header("AI variables")]
    [SerializeField] float sightRange;
    [SerializeField] float attackRange;
    [SerializeField] float closeRange;
    public bool isInSightRange = false;
    public bool isInAttackRange = false;

    [SerializeField] float sightOfChecking = 30f;

    #endregion

    public void SerchingForPlayer(Transform muzzlePos)
    {
        //check if in sight range
        Collider[] hits = Physics.OverlapSphere(muzzlePos.position, sightRange, LayerMask.GetMask("Player"));
        foreach (var hit in hits) 
        {
            Vector3 direction = hit.transform.position - muzzlePos.position;
            float angle = Vector3.SignedAngle(muzzlePos.forward, direction, Vector3.up);

            if (Mathf.Abs(angle) < sightOfChecking)     //checks if target is in front of turret
            {
                float distanceToPlayer = Vector3.Distance(muzzlePos.position, hit.transform.position);
                if (distanceToPlayer < attackRange)
                {
                    isInSightRange = true;
                    isInAttackRange = true;
                    return;
                }
                else isInAttackRange = false;           //if not in attack range = false

                //if in front of a turret = true
                isInSightRange = true;
                return;
            }        
        }

        //check if player too close (AKA if too close from behind = isAtackable)
        Collider[] closeHits = Physics.OverlapSphere(muzzlePos.position, closeRange, LayerMask.GetMask("Player"));
        if (closeHits.Length > 0)
        {
            isInSightRange = true;
            isInAttackRange = true;
        }

        // if there no player isInAttackRange/isInSightRange and itsNotTooClose = false
        else { isInAttackRange = false; isInSightRange = false; }
    }
}
