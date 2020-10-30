using UnityEngine;

public class Core : MonoBehaviour, IInteractable
{
    Accelerator[] accelerators;

    void Start()
    {
        accelerators = FindObjectsOfType<Accelerator>();
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
