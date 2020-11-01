using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask whatIsSolid;
    public float lifeTime;

    float projSpeed;
    int projDmg;

    void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
    }

    void Update()
    {
        Vector3 direction = transform.InverseTransformDirection(-transform.forward);
        direction.y = 0f;

        transform.Translate(direction.normalized * projSpeed * Time.deltaTime);

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, -transform.forward, out hitInfo, 0.3f, whatIsSolid))
        {
            if (hitInfo.collider.CompareTag("Player"))
                hitInfo.collider.GetComponent<PlayerController>().AddHealth(-projDmg);
            else if (hitInfo.collider.CompareTag("Enemy"))
                hitInfo.collider.GetComponent<Enemy>().TakeDamage(projDmg);

            DestroyProjectile();
        }
        Debug.DrawRay(transform.position, -transform.right * 10f, Color.red);
    }

    public void SetData(float _projSpeed, int _projDmg)
    {
        projSpeed = _projSpeed;
        projDmg = _projDmg;
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

}
