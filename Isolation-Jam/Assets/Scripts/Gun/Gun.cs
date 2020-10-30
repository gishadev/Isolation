using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("General")]
    public Transform shotPos;
    public bool isAutomatic;
    public bool isEndlessAmmo;
    public int ammoCount;
    public float delayBtwShots;

    [Header("Projectile")]
    public GameObject projectile;
    public float projSpeed;
    public int projDmg;

    public int Ammo 
    {
        get => ammo; 
        set
        {
            UIManager.Instance.gun.UpdateAmmoCount(value);
            ammo = Mathf.Clamp(value, 0, ammoCount);
        }
    }
    int ammo;

    public bool IsReadyToShoot { private set; get; } = true;

    void Start()
    {
        if (!isEndlessAmmo)
            Ammo = ammoCount;
    }

    public void Shoot()
    {
        GameObject projGO = Instantiate(projectile, shotPos.position, shotPos.rotation);
        Projectile proj = projGO.GetComponent<Projectile>();

        proj.SetData(projSpeed, projDmg);

        if (!isEndlessAmmo)
        {
            Ammo--;

            if (Ammo <= 0)
                PlayerManager.Instance.GiveGun(PlayerManager.Instance.defaultGun);
        }


        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        IsReadyToShoot = false;
        yield return new WaitForSeconds(delayBtwShots);
        IsReadyToShoot = true;
    }
}
