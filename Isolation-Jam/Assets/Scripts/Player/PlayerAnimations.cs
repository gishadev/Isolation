using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();    
    }

    public void UpdateMovementAnimationVel(Vector3 lookDir, Vector3 movementInput)
    {
        // Movement Animation.
        Vector3 d = GetMovementDir(lookDir, movementInput);

        if (movementInput.z < 0)
            animator.SetFloat("X", -d.x);
        else
            animator.SetFloat("X", d.x);

        animator.SetFloat("Z", d.z);
    }

    // 0 - 2 (Empty, Pistol, Rifle)
    public void UpdateUpperState(int state)
    {
        animator.SetInteger("UpperState", state);
    }

    Vector3 GetMovementDir(Vector3 lookDir, Vector3 moveVel)
    {
        float v = Mathf.Clamp(Vector3.Dot(moveVel, lookDir), -1f, 1f);

        Vector3 lookPerp = new Vector3(lookDir.z, 0f, -lookDir.x);
        float h = Mathf.Clamp(Vector3.Dot(moveVel, lookPerp), -1f, 1f);

        return new Vector3(h, 0f, v);
    }
}
