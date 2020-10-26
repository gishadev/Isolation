using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region PUBLIC_FIELDS
    [Header("Movement")]
    public Transform target;
    public float maxXOffset, maxZOffset;
    public float smoothness = 1f;
    #endregion

    #region PRIVATE_FIELDS
    Vector3 offset;
    #endregion

    #region METHODS

    void Update()
    {
        // Камера следует за игроком и оффсетим в сторону мыши.
        offset.x = Mathf.Lerp(offset.x, GetMouseDistFromCenter().x * maxXOffset, Time.deltaTime * smoothness);
        offset.y = 0f;
        offset.z = Mathf.Lerp(offset.z, GetMouseDistFromCenter().y * maxZOffset, Time.deltaTime * smoothness);

        transform.position = target.position + offset;
    }


    Vector2 GetMouseDistFromCenter()
    {
        float x = Mathf.Clamp(((Input.mousePosition.x - (Screen.width / 2f)) / Screen.width) * 2f, -1f, 1f);
        float y = Mathf.Clamp(((Input.mousePosition.y - (Screen.height / 2f)) / Screen.height) * 2f, -1f, 1f);
        return new Vector2(x, y);
    }

    #endregion
}
