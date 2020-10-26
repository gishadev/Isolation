using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region PUBLIC_FIELDS
    [Header("Movements")]
    public float moveSpeed = 15f;
    public float accFactor = 1f;
    #endregion

    #region PRIVATE_FIELDS
    Vector3 moveInput = Vector3.zero;
    #endregion

    #region COMPONENTS
    Rigidbody rb;
    #endregion

    #region METHODS
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdateMoveInput();
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed * Time.fixedDeltaTime;
    }

    #region Movement
    // Кастомный Input.GetAxis.
    void UpdateMoveInput()
    {
        float rawH = Input.GetAxisRaw("Horizontal");
        float rawV = Input.GetAxisRaw("Vertical");

        moveInput.x = Mathf.Lerp(moveInput.x, rawH, Time.deltaTime * accFactor);
        moveInput.z = Mathf.Lerp(moveInput.z, rawV, Time.deltaTime * accFactor);

        moveInput = Vector3.ClampMagnitude(moveInput, 1f);
    }
    #endregion

    #endregion
}
