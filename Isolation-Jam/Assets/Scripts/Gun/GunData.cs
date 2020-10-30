using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Scriptable Objects/Create Gun")]
public class GunData : ScriptableObject
{
    [Header("Gun Data")]
    public GameObject prefab;

    public Vector3 offsetPosition;
    public Vector3 offsetRotation;
}
