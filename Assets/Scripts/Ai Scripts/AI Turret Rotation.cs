using UnityEngine;

public class AITurretRotation : MonoBehaviour
{
    private bool canRotate;
    private bool rotating;
    private float startRotTime;
    private float timeBetweenRot = 3f;
    private float inTurretYaw;

    public void TurretRotation(Transform Turret, string hullName, string turretName, float rotationSpeed, 
        TurretControlBase turretBase, float? targetYaw)
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
            rotating = true;
            if (targetYaw == null) { inTurretYaw = UnityEngine.Random.Range(-180f, 180f); }
            else { inTurretYaw = targetYaw.Value; }
            startRotTime = Time.time;
        }

        Quaternion currentRotation = Turret.transform.localRotation;
        Quaternion desiredRotation = Quaternion.Euler(0f, inTurretYaw, 0f);

        Turret.transform.localRotation = Quaternion.RotateTowards(currentRotation, desiredRotation, rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(currentRotation, desiredRotation) < 0.5f)
        {
            canRotate = false;
            rotating = false;

        }
    }
    private void TurretCooldown()
    {
        if (Time.time - startRotTime >= timeBetweenRot)
        {
            canRotate = true;
        }
    }
    
}
