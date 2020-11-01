using System.Collections;
using UnityEngine;

public class BatteryCellSpawner : MonoBehaviour
{
    [Header("Battery Spawning")]
    public GameObject batteryCellPrefab;
    public float spawnDelay = 15f;
    public Material sampleEnergyMat;

    bool IsHold { get => spawnedBattery != PlayerManager.Instance.player.HoldingBattery; }

    Material energyMat;
    BatteryCell spawnedBattery;

    MeshRenderer mr;

    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        MaterialsRewriter.ResetMaterials(mr, sampleEnergyMat, out energyMat);
        StartCoroutine(SpawnBattery());
    }

    IEnumerator SpawnBattery()
    {
        while (true)
        {
            yield return StartCoroutine(Timer(spawnDelay));
            spawnedBattery = Instantiate(batteryCellPrefab, transform.position + Vector3.up * 2f, Quaternion.identity).GetComponent<BatteryCell>();
            yield return new WaitWhile(() => IsHold);
        }
    }

    IEnumerator Timer(float maxTime)
    {
        float time = 0f;

        while (true)
        {
            time += Time.deltaTime;
            float progress = Mathf.Clamp01(time / maxTime);
            energyMat.SetFloat("_Progress", progress);

            if (progress >= 1f)
                break;

            yield return null;
        }
    }
}
