using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("General")]
    public int health = 10;
    [Header("Drop Gun Up")]
    public GameObject gunUpPrefab;
    public float gunUpChance = 0.25f;

    public Spawnpoint LocalSpawnpoint { get; set; }
    public PlayerController Player { private set; get; }
    public NavMeshAgent Agent { private set; get; }

    void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Player = PlayerManager.Instance.player;
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        if (Random.Range(0f, 1f) < gunUpChance)
            Instantiate(gunUpPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
