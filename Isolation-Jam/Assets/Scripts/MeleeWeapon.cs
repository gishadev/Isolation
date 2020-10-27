using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.CompareTag("Enemy"))
                Destroy(other.gameObject);
        }
    }
}
