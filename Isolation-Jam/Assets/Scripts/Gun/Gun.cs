using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("General")]
    public Transform shotPos;
    public bool isAutomatic;
    public bool isEndlessAmmo;
    public int ammoCount;

    [Header("Projectile")]
    public GameObject projectile;
    public float projSpeed;
    public int projDmg;

    int ammo;

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
    }
}
