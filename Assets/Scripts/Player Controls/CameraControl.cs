using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    #region Variables

    [Header("Hull/Turret")]
    GameObject hull;
    GameObject turret;

    [Header("Camera settings")]
    [SerializeField] float maxPitch = 15f;
    [SerializeField] float minPitch = 2f;
    private float rise = 6f;

    [SerializeField] float Radius = 13f;
    private float maxRange = 25f;
    private float minRange = 5f;

 
    private float angle;
    private Vector3 offset;


    [Header("Input")]
    float riseInput;
    float rangeInput;

    #endregion

    void Update()
    {
        float currentInput = 0f;
        float currentRangeInput = 0f;

        angle = turret.transform.eulerAngles.y * Mathf.Deg2Rad;
      
        currentInput = Mathf.Lerp(currentInput, riseInput, Time.time);
        rise += currentInput*4 * Time.deltaTime;
        rise = Mathf.Clamp(rise, minPitch, maxPitch);

        currentRangeInput = Mathf.Lerp(currentRangeInput, rangeInput, Time.time);
        Radius += currentRangeInput * 4 * Time.deltaTime;
        Radius = Mathf.Clamp(Radius, minRange, maxRange);

        offset = new Vector3(Mathf.Sin(angle) * Radius, -rise, Mathf.Cos(angle) * Radius);
        transform.position = hull.transform.position + offset*-1;
        transform.LookAt(hull.transform);
    }

    public void OnCameraControl(InputValue value)
    {
        riseInput = value.Get<float>();
    }
    public void OnCameraRange(InputValue value)
    {
        rangeInput = value.Get<float>();
    }

    public void Initialize(GameObject hullGO, GameObject turretGO)
    {
        hull = hullGO;
        turret = turretGO;
    }
}
