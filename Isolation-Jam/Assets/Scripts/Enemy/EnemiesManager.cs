using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    #region Singleton
    public static EnemiesManager Instance { private set; get; }
    #endregion

    [Header("Spawning")]
    public GameObject[] prefabs;
    public Spawnpoint[] spawnpoints;
    [Space]
    public float spawnDelay;
    public int maxEnemyCount = 15;

    public Enemy[] enemies;

    void Awake()
    {
        Instance = this;
    }

    public void OnPlay()
    {
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        while (true)
        {
            enemies = FindObjectsOfType<Enemy>();

            if (enemies.Length < maxEnemyCount)
            {
                Spawnpoint s = spawnpoints[Random.Range(0, spawnpoints.Length)];

                if (s.IsActive)
                {
                    // Spawning.
                    Enemy e = Instantiate(
                        prefabs[Random.Range(0, prefabs.Length)],
                        s.Position,
                        Quaternion.identity).GetComponent<Enemy>();

                    e.LocalSpawnpoint = s;
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
