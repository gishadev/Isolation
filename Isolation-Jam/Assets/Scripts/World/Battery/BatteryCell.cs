using UnityEngine;

public class BatteryCell : MonoBehaviour
{
    public float chargePercent = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController p = GameManager.Instance.player;
            p.HoldingBattery = this;
            transform.SetParent(p.transform);
        }
    }
}
