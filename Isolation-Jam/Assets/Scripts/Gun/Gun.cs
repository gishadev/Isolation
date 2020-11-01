using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform shotPos;
    [HideInInspector] public GunData gunData;

    public int Ammo
    {
        get => ammo;
        set => ammo = Mathf.Clamp(value, 0, gunData.ammoCount);
    }
    int ammo;

    public bool IsReadyToShoot { private set; get; } = true;

    void Start()
    {
        Ammo = gunData.ammoCount;
    }

    public void Shoot()
    {
        GameObject projGO = Instantiate(gunData.projectile, shotPos.position, shotPos.rotation);
        Projectile proj = projGO.GetComponent<Projectile>();

        proj.SetData(gunData.projSpeed, gunData.projDmg);

        if (!gunData.isEndlessAmmo)
        {
            Ammo--;
            UIManager.Instance.gun.UpdateAmmoCount(Ammo);

            if (Ammo <= 0)
                PlayerManager.Instance.GiveGun(PlayerManager.Instance.defaultGun);
        }


        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        IsReadyToShoot = false;
        yield return new WaitForSeconds(gunData.delayBtwShots);
        IsReadyToShoot = true;
    }
}
