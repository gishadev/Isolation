using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject[] prefabs;
    public Spawnpoint[] spawnpoints;
    [Space]
    public float spawnDelay;
    public int maxEnemyCount = 15;

    List<Enemy> enemies = new List<Enemy>();

    void Start()
    {
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        while (true)
        {
            if (enemies.Count < maxEnemyCount)
            {
                Spawnpoint s = spawnpoints[Random.Range(0, spawnpoints.Length)];

                if (s.IsActive)
                {
                    // Spawning.
                    Enemy e = Instantiate(
                        prefabs[Random.Range(0, prefabs.Length)],
                        s.Position,
                        Quaternion.identity).GetComponent<Enemy>();

                    enemies.Add(e);

                }
                else
                {
                    // Restarting Coroutine.
                    StartCoroutine(Spawning());
                    break;
                }
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
