using UnityEngine;
public class UIHealth : MonoBehaviour
{
    public void UpdateHealthBar(float value, float maxValue)
    {
        float p = value / maxValue;
        transform.localScale = new Vector3(p, transform.localScale.y, transform.localScale.z);
    }
}
