using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    public Transform target;
    public float maxXOffset, maxZOffset;
    public float smoothness = 1f;

    Vector3 offset;

    void FixedUpdate()
    {
        // Камера следует за игроком и оффсетим в сторону мыши.
        offset.x = GetMouseDistFromCenter().x * maxXOffset;
        offset.y = 0f;
        offset.z = GetMouseDistFromCenter().y * maxZOffset;

        Vector3 newPos = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * smoothness);
    }


    Vector2 GetMouseDistFromCenter()
    {
        float x = Mathf.Clamp(((Input.mousePosition.x - (Screen.width / 2f)) / Screen.width) * 2f, -1f, 1f);
        float y = Mathf.Clamp(((Input.mousePosition.y - (Screen.height / 2f)) / Screen.height) * 2f, -1f, 1f);
        return new Vector2(x, y);
    }
}
