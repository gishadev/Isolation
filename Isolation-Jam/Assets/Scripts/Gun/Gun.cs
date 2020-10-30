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

    int ammo;

    public bool IsReadyToShoot { private set; get; } = true;

    void Start()
    {
        if (!isEndlessAmmo)
            ammo = ammoCount;
    }

    public void Shoot()
    {
        GameObject projGO = Instantiate(projectile, shotPos.position, shotPos.rotation);
        Projectile proj = projGO.GetComponent<Projectile>();

        proj.SetData(projSpeed, projDmg);

        if (!isEndlessAmmo)
            ammo--;

        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        IsReadyToShoot = false;
        yield return new WaitForSeconds(delayBtwShots);
        IsReadyToShoot = true;
    }
}
