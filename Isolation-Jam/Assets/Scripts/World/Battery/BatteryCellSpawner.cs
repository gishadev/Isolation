using System.Collections;
using UnityEngine;

public class BatteryCellSpawner : MonoBehaviour
{
    [Header("Battery Spawning")]
    public GameObject batteryCellPrefab;
    public float spawnDelay = 15f;
    public Material energyMatSample;

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
        ResetMaterials();
        StartCoroutine(SpawnBattery());
    }

    // Обновляем коллекцию материалов для динамичного изменения.
    void ResetMaterials()
    {
        Material[] newMats = new Material[mr.sharedMaterials.Length];

        for (int i = 0; i < newMats.Length; i++)
        {
            if (mr.sharedMaterials[i] == energyMatSample)
            {
                energyMat = new Material(energyMatSample);
                newMats[i] = energyMat;
                continue;
            }

            newMats[i] = mr.sharedMaterials[i];
        }

        mr.materials = newMats;
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
