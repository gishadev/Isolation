using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region PUBLIC_FIELDS
    [Header("Movement")]
    public float runSpeed = 500f;
    public float accelerationSpeed = 15f;

    [Header("Dash")]
    public KeyCode DashInput;
    public float dashSpeed = 1000f;
    public float dashDist = 5f;
    public float dashDelay = 1f;
    [Space]
    public int groundLayer = 9;

    [Header("Battery Cell")]

    [Range(0f, 1f)] public float speedDecrease = 0.25f;
    #endregion

    #region PRIVATE_FIELDS
    bool isCollision = false;
    bool isDashDelay = false;
    Vector3 movementInput = Vector3.zero;
    #endregion

    #region PROPERTIES
    public bool IsDashing { set; get; } = false;
    public BatteryCell HoldingBattery { get; set; }
    float MovementSpeed { get => IsDashing ? dashSpeed : runSpeed; }
    float MovementModifier { get => HoldingBattery != null ? 1f - speedDecrease : 1f; }
    #endregion

    #region COMPONENTS
    Rigidbody rb;
    Camera cam;
    #endregion

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    void Update()
    {
        if (!IsDashing)
            UpdateMovementInput();

        if (Input.GetKeyDown(DashInput))
        {
            if (movementInput.magnitude > 0 && !isCollision && !isDashDelay)
                StartCoroutine(Dash());
        }

        PlayerRotation();
    }

    void FixedUpdate()
    {
        rb.velocity = movementInput * MovementSpeed * MovementModifier * Time.fixedDeltaTime;
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
        IsDashing = true;

        while (Vector3.Distance(initPos, transform.position) < dashDist * MovementModifier && !isCollision)
            yield return null;

        IsDashing = false;
        StartDashDelay();
    }

    void StartDashDelay()
    {
        isDashDelay = true;
        Invoke("StopDashDelay", dashDelay);
    }

    void StopDashDelay()
    {
        isDashDelay = false;
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

    #region Collision Detection
    private void OnCollisionEnter(Collision coll)
    {
        // Если игрок коллизится не с полом => отключаем dash.
        if (coll != null)
            if (coll.gameObject.layer != groundLayer)
                isCollision = true;

        if (coll.collider.CompareTag("WorldWall"))
            Die();
    }

    private void OnCollisionExit(Collision coll)
    {
        // Игрок перестал коллизится не с полом.
        if (coll != null)
            if (coll.gameObject.layer != groundLayer)
                isCollision = false;
    }
    #endregion

    public void Die()
    {
        gameObject.SetActive(false);

        IsDashing = false;
        StopDashDelay();

        PlayerManager.Instance.TriggerRespawning();
    }
}
