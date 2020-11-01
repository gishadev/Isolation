using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Melee")]
    public int damage = 15;
    public float knockbackForce = 1f;
    public float attackRadius = 1f;
    [Space]
    public LayerMask playerLayer;

    void Update()
    {
        if (Player.gameObject.activeInHierarchy)
        {
            // Враг движется к игроку и, останавливаясь на мин. дистанции, атакует его.
            if (IsInPlayer())
                Attack();
            else
                Agent.SetDestination(Player.transform.position);

            if (Agent.velocity.normalized.magnitude == 0f)
                LockPointOfView(Player.transform.position);
        }
        else
            Agent.SetDestination(LocalSpawnpoint.Position);
    }

    void Attack()
    {
        PlayerManager.Instance.player.AddHealth(-damage);
        Vector3 dir = (PlayerManager.Instance.player.transform.position - transform.position).normalized;
        PlayerManager.Instance.player.AddKnockback(dir * knockbackForce);

        Destroy(gameObject);
    }

    void LockPointOfView(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        float rotY = Mathf.Atan2(dir.z, -dir.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(Vector3.up * rotY);
        transform.rotation = rotation;
    }

    bool IsInPlayer()
    {
        return Physics.CheckSphere(transform.position, attackRadius, playerLayer) && !PlayerManager.Instance.player.IsDashing;
    }
}
