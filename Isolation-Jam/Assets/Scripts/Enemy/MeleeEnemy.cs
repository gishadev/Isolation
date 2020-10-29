using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Melee")]
    public int damage = 15;
    public float knockbackForce = 1f;
    public float attackDelay = 1.5f;
    public float rayLength = 1f;
    [Space]
    public LayerMask playerLayer;

    float delay;

    void Start()
    {
        delay = attackDelay;
    }

    void Update()
    {
        // Враг движется к игроку и, останавливаясь на мин. дистанции, атакует его.
        if (ForwardRaycast())
        {
            if (delay < 0)
            {
                Attack();
                delay = attackDelay;
            }
        }
        else
        {
            Agent.SetDestination(PlayerTrans.position);
        }


        if (delay > 0)
            delay -= Time.deltaTime;

        if (Agent.velocity.normalized.magnitude == 0f)
            LockPointOfView(PlayerTrans.position);
    }

    void Attack()
    {
        PlayerManager.Instance.player.AddHealth(-damage);
        Vector3 dir = (PlayerManager.Instance.player.transform.position - transform.position).normalized;
        PlayerManager.Instance.player.AddKnockback(dir * knockbackForce);
    }

    void LockPointOfView(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        float rotY = Mathf.Atan2(dir.z, -dir.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(Vector3.up * rotY);
        transform.rotation = rotation;
    }

    bool ForwardRaycast()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, rayLength, playerLayer))
            return true;

        Debug.DrawRay(transform.position, ray.direction * rayLength, Color.red, 0.1f);

        return false;
    }
}
