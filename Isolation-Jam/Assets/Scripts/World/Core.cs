using UnityEngine;

public class Core : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Game Over.
        if (other.CompareTag("EdgeBasis"))
            GameManager.Instance.Lose();
    }
}
