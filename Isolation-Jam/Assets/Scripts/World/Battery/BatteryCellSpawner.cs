using System.Collections;
using UnityEngine;

public class BatteryCellSpawner : MonoBehaviour
{
    public GameObject batteryCellPrefab;
    public float spawnDelay = 15f;

    BatteryCell spawnedBattery;
    bool IsHold { get => spawnedBattery != GameManager.Instance.player.HoldingBattery; }

    void Start()
    {
        StartCoroutine(SpawnBattery());
    }

    IEnumerator SpawnBattery()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);
            spawnedBattery = Instantiate(batteryCellPrefab, transform.position + Vector3.up * 2f, Quaternion.identity).GetComponent<BatteryCell>();
            yield return new WaitWhile(() => IsHold);
        }
    }
}
