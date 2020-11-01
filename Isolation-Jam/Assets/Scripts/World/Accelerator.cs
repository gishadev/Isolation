using System.Collections;
using UnityEngine;

public class Accelerator : MonoBehaviour, IInteractable
{
    [Header("Accelerator")]
    public float fullDischargeTime = 10f;
    public float Power { private set { power = Mathf.Clamp01(value); } get => power; }
    public bool IsCharged { get => Power > 0f; }

    float power;

    public LineRenderer lrToWall;
    public LineRenderer lrToCore;
    Material rayMat;

    void Start()
    {
        Charge(1.0f);
        MaterialsRewriter.ResetMaterials(lrToWall, out rayMat);
    }

    void FixedUpdate()
    {
        //Разрядка ускорителя.
        Power -= Time.fixedDeltaTime / fullDischargeTime;

        lrToWall.enabled = Power > 0;
        lrToCore.enabled = Power > 0;

        rayMat.SetFloat("_Energy", Power);
    }

    public void Charge(float value)
    {
        Power += value;
    }

    #region IInteractable
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
    #endregion
}