using UnityEngine;

public class Accelerator : MonoBehaviour, IInteractable
{
    [Header("Accelerator")]
    public float fullDischargeTime = 10f;
    public float Power { private set { power = Mathf.Clamp01(value); } get => power; }
    public bool IsCharged { get => Power > 0f; }

    float power;

    void Start()
    {
        Charge();
    }

    void FixedUpdate()
    {
        // Пассивная разрядка ускорителя.
        Power -= Time.fixedDeltaTime / fullDischargeTime;
    }

    public void Charge()
    {
        Power = 1.0f;
    }

    public void Charge(float value)
    {
        Power += value;
    }

    public void Interact()
    {
        PlayerController p = PlayerManager.Instance.player;
        if (p.HoldingBattery != null)
        {
            Charge(p.HoldingBattery.chargePercent);
            Destroy(p.HoldingBattery.gameObject);
        }
    }

    public bool IsReadyForInteraction()
    {
        return PlayerManager.Instance.player.HoldingBattery != null;
    }
}