using System.Linq;
using UnityEngine;

public class Core : MonoBehaviour, IInteractable
{
    public MeshRenderer crystal;
    public Material sampleEnergyMat;

    float AvgPower { get => accelerators.Sum(x => x.Power) / accelerators.Length; }

    Accelerator[] accelerators;
    Material energyMat;

    void Awake()
    {
        accelerators = FindObjectsOfType<Accelerator>();
    }

    void Start()
    {
        MaterialsRewriter.ResetMaterials(crystal, sampleEnergyMat, out energyMat);
    }

    void LateUpdate()
    {
        energyMat.SetFloat("_Energy", AvgPower);
    }

    // Заряжаем все ускорители равномерно.
    public void Interact()
    {
        PlayerController p = PlayerManager.Instance.player;
        if (p.HoldingBattery)
        {
            float v = p.HoldingBattery.chargePercent / accelerators.Length;

            for (int i = 0; i < accelerators.Length; i++)
                accelerators[i].Charge(v);

            Destroy(p.HoldingBattery.gameObject);
        }

        AudioManager.Instance.PlaySFX("Core_Charge");
    }

    public bool IsReadyForInteraction()
    {
        return PlayerManager.Instance.player.HoldingBattery != null;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Game Over.
        if (other.CompareTag("EdgeBasis"))
            GameManager.Instance.Lose();
    }
}
