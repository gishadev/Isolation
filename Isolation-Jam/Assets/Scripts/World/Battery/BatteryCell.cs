using UnityEngine;

public class BatteryCell : MonoBehaviour
{
    public float chargePercent = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController p = PlayerManager.Instance.player;
            if (p.HoldingBattery == null)
            {
                p.HoldingBattery = this;
                transform.SetParent(p.transform);
            }
        }
    }
}
