using UnityEngine;

public class BatteryCell : MonoBehaviour, IInteractable
{
    [Header("Battery")]
    [Range(0f, 1f)] public float speedDecrease = 0.25f;
    public float chargePercent = 0.5f;

    public void Interact()
    {
        PlayerManager.Instance.player.TakeBattery(this);
    }

    public bool IsReadyForInteraction()
    {
        return PlayerManager.Instance.player.HoldingBattery == null;
    }
}
