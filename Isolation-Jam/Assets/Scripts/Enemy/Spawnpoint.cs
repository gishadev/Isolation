using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    public Vector3 Position { get => transform.position; }
    public bool IsActive
    {
        get => Position.x > corners.MIN_X || Position.x < corners.MAX_X
            || Position.z > corners.MIN_Z || Position.z < corners.MAX_Z;
    }

    BlackoutAlphaMask corners;

    void Awake()
    {
        corners = FindObjectOfType<BlackoutAlphaMask>();
    }
}