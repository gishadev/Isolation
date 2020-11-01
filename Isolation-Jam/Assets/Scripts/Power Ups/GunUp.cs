using UnityEngine;

public class GunUp : MonoBehaviour, IPowerUp
{
    public GunData[] guns;

    public void Use()
    {
        PlayerManager.Instance.GiveGun(guns[Random.Range(0, guns.Length)]);

        Destroy(gameObject);
    }
}
