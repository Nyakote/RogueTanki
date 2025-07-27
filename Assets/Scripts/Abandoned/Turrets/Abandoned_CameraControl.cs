using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    #region Variables

    [Header("Hull/Turret")]
    [SerializeField] Transform hull;
    [SerializeField] Transform turret;

    [Header("Camera settings")]
    [SerializeField] float maxPitch = 15f;
    [SerializeField] float minPitch = 2f;
    [SerializeField] float Radius = 10f;
    private float angle;
    private Vector3 offset;
    private float rise = 2f;

    [Header("Input")]
    float riseInput;

    #endregion
    void Start()
    {
       
    }

    void Update()
    {
        GetInput();
        float currentInput = 0f;

        angle = turret.eulerAngles.y * Mathf.Deg2Rad;
      
        currentInput = Mathf.Lerp(currentInput, riseInput, Time.time);
        rise += currentInput*4 * Time.deltaTime;
        rise = Mathf.Clamp(rise, minPitch, maxPitch);

        offset = new Vector3(Mathf.Sin(angle) * Radius, -rise, Mathf.Cos(angle) * Radius);
        transform.position = hull.position + offset*-1;
        transform.LookAt(hull);
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.Q)){ riseInput = 1.0f; }
        else if(Input.GetKey(KeyCode.E)) { riseInput = -1.0f; }
        else { riseInput = 0.0f; }
    }
}
