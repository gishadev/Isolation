using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Animator animator;

    int upperState = 0;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void LateUpdate()
    {
        animator.SetInteger("UpperState", upperState);
    }

    // Обновление анимации бега на основе BlendTree.
    public void UpdateMovementAnimationVel(Vector3 lookDir, Vector3 movementInput)
    {
        Vector3 d = GetMovementDir(lookDir, movementInput);

        if (movementInput.z < 0)
            animator.SetFloat("X", -d.x);
        else
            animator.SetFloat("X", d.x);

        animator.SetFloat("Z", d.z);
    }

    // Просчёт направления для анимации бега.
    Vector3 GetMovementDir(Vector3 lookDir, Vector3 moveVel)
    {
        float v = Mathf.Clamp(Vector3.Dot(moveVel, lookDir), -1f, 1f);

        Vector3 lookPerp = new Vector3(lookDir.z, 0f, -lookDir.x);
        float h = Mathf.Clamp(Vector3.Dot(moveVel, lookPerp), -1f, 1f);

        return new Vector3(h, 0f, v);
    }


    // 0 - 2 (Empty, Pistol, Rifle)
    public void UpdateUpperState(int state)
    {
        upperState = state;
    }
}
