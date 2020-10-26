using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region PUBLIC_FIELDS
    [Header("Movements")]
    public float moveSpeed = 15f;
    public float accFactor = 1f;
    #endregion

    #region PRIVATE_FIELDS
    Vector3 moveInput;
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
        moveInput = GetMovementInput();
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed * Time.fixedDeltaTime;
    }

    #region Movement
    Vector3 GetMovementInput()
    {
        float h = Mathf.Clamp(Input.GetAxis("Horizontal") * accFactor, -1f, 1f);
        float v = Mathf.Clamp(Input.GetAxis("Vertical") * accFactor, -1f, 1f);

        return new Vector3(h, 0f, v).normalized;
    }
    #endregion

    #endregion
}
