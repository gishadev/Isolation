using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("General")]
    public int health = 10;

    public Transform PlayerTrans { private set; get; }
    public NavMeshAgent Agent { private set; get; }

    void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        PlayerTrans = FindObjectOfType<PlayerController>().transform;
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
