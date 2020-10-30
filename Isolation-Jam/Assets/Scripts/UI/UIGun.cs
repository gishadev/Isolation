using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGun : MonoBehaviour
{
    public TMP_Text gunNameText;
    public TMP_Text ammoText;
    public Image gunImg;

    public void ResetGun(GunData gunData)
    {
        gunNameText.text = gunData.name;
        //gunImg.sprite = gunData.name;
    }

    public void UpdateAmmoCount(int count)
    {
        ammoText.text = count.ToString();
    }
}
