using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region PUBLIC_FIELDS
    [Header("General")]
    public int maxHealth = 100;

    [HideInInspector] public Gun currentGun;

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
    #endregion

    #region PRIVATE_FIELDS
    int currentHealth;

    bool isCollision = false;
    bool isDashDelay = false;
    Vector3 movementInput = Vector3.zero;
    Vector3 lookDir;
    #endregion

    #region PROPERTIES
    PlayerManager manager { get => PlayerManager.Instance; }

    public int Health
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            UIManager.Instance.health.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    public bool IsDashing { set; get; } = false;

    // Аккумулятор, который держит игрок.
    public BatteryCell HoldingBattery
    {
        get => _holdingBattery;
        set
        {
            _holdingBattery = value;
            ShowGun(value == null);
        }
    }
    BatteryCell _holdingBattery;
    // Значение скорости при различных типах передвижения.
    float MovementSpeed { get => IsDashing ? dashSpeed : runSpeed; }
    // "Замедлитель" игрока, когда он несёт аккумулятор.
    float MovementModifier { get => HoldingBattery != null ? 1f - HoldingBattery.speedDecrease : 1f; }
    #endregion

    #region COMPONENTS
    Rigidbody rb;
    Camera cam;
    PlayerAnimations pAnimations;
    #endregion

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pAnimations = GetComponent<PlayerAnimations>();
        cam = Camera.main;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        rb.velocity = movementInput * MovementSpeed * MovementModifier * Time.fixedDeltaTime;
    }

    void Update()
    {
        // Передвижение.
        if (!IsDashing)
        {
            UpdateMovementInput();
            pAnimations.UpdateMovementAnimationVel(lookDir, movementInput);
        }

        // Поворот игрока.
        PlayerRotation();

        // Рывок.
        if (Input.GetKeyDown(DashInput))
        {
            if (movementInput.normalized.magnitude > 0 && !isCollision && !isDashDelay)
                StartCoroutine(Dash());
        }

        // Стрельба.
        if (HoldingBattery == null)
        {
            if (!currentGun.isAutomatic)
            {
                if (Input.GetMouseButtonDown(0) && currentGun.IsReadyToShoot)
                    currentGun.Shoot();
            }

            else
            {
                if (Input.GetMouseButton(0) && currentGun.IsReadyToShoot)
                    currentGun.Shoot();
            }
        }
    }

    void LateUpdate()
    {
        // Взаимодействуем.
        if (manager.SelectedInteractTarget != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
                manager.SelectedInteractTarget.Interact();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (HoldingBattery != null)
                DropBattery(transform.position);
        }
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
            lookDir = (hitInfo.point - transform.position).normalized;
            float rotY = Mathf.Atan2(lookDir.z, -lookDir.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.Euler(Vector3.up * rotY);
            transform.rotation = rotation;
        }
    }
    #endregion

    #region Health
    public void AddHealth(int value)
    {
        Health += value;

        if (Health <= 0)
            Die();
    }

    public void AddKnockback(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }

    public void Die()
    {
        Health = 0;

        gameObject.SetActive(false);

        if (HoldingBattery != null)
            DropBattery(transform.position);

        IsDashing = false;
        isCollision = false;
        StopDashDelay();

        PlayerManager.Instance.TriggerRespawning();
    }
    #endregion

    #region Carry Battery
    public void DropBattery(Vector3 newPosition)
    {
        HoldingBattery.transform.SetParent(null);
        HoldingBattery.transform.position = newPosition;
        HoldingBattery = null;
    }

    public void TakeBattery(BatteryCell b)
    {
        HoldingBattery = b;
        HoldingBattery.transform.SetParent(transform);
    }
    #endregion

    #region Hide/Show Gun
    void ShowGun(bool isShow)
    {
        if (currentGun != null)
            currentGun.gameObject.SetActive(isShow);
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
}
