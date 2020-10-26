using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region PUBLIC_FIELDS
    [Header("Movement")]
    public float runSpeed = 500f;
    public float accelerationSpeed = 15f;
    [Header("Dash")]
    public bool isDashing = false;
    public float dashSpeed = 1000f;
    public float dashDist = 5f;
    #endregion

    #region PRIVATE_FIELDS
    Vector3 movementInput = Vector3.zero;
    int nonPlayerLayerMask;
    #endregion

    #region PROPERTIES
    float MovementSpeed { get; set; }
    #endregion

    #region COMPONENTS
    Rigidbody rb;
    Camera cam;
    #endregion

    #region METHODS
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    void Start()
    {
        MovementSpeed = runSpeed;

        nonPlayerLayerMask = 1 << gameObject.layer;
        nonPlayerLayerMask = ~nonPlayerLayerMask;
    }

    void Update()
    {
        if (!isDashing)
            UpdateMovementInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (movementInput.magnitude > 0)
                StartCoroutine(Dash());
        }

        PlayerRotation();
    }

    void FixedUpdate()
    {
        rb.velocity = movementInput * MovementSpeed * Time.fixedDeltaTime;
    }

    #region Movement
    // Кастомный Input.GetAxis.
    void UpdateMovementInput()
    {
        float rawH = Input.GetAxisRaw("Horizontal");
        float rawV = Input.GetAxisRaw("Vertical");

        movementInput.x = Mathf.Lerp(movementInput.x, rawH, Time.deltaTime * accelerationSpeed);
        movementInput.z = Mathf.Lerp(movementInput.z, rawV, Time.deltaTime * accelerationSpeed);

        movementInput = Vector3.ClampMagnitude(movementInput, 1f);
    }

    IEnumerator Dash()
    {
        Vector3 initPos = transform.position;
        movementInput = movementInput.normalized;
        MovementSpeed = dashSpeed;
        isDashing = true;

        while (Vector3.Distance(initPos, transform.position) < dashDist && !IsObstacleOnDashDir(movementInput))
            yield return null;

        MovementSpeed = runSpeed;
        isDashing = false;
    }
    

    // Можно сделать через OnCollisionEnter
    bool IsObstacleOnDashDir(Vector3 direction)
    {
        Ray ray = new Ray(transform.position, direction);
        Debug.DrawRay(ray.origin, ray.direction, Color.blue);
        return Physics.Raycast(ray, 1f, nonPlayerLayerMask);
    }
    #endregion

    #region Rotation
    void PlayerRotation()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            Vector3 dir = (hitInfo.point - transform.position).normalized;
            float rotY = Mathf.Atan2(dir.z, -dir.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.Euler(Vector3.up * rotY);
            transform.rotation = rotation;
        }        
    }
    #endregion

    #endregion
}
