using UnityEngine;

public class CollapseManager : MonoBehaviour
{
    #region Singleton
    public static CollapseManager Instance { private set; get; }
    #endregion

    [Header("Collapsing")]
    public float collapseSpeed = 1f;
    public WorldEdge[] worldEdges;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        for (int i = 0; i < worldEdges.Length; i++)
            if (worldEdges[i].IsCollapsing)
                worldEdges[i].Collapse(collapseSpeed);
    }
}

[System.Serializable]
public class WorldEdge
{
    public string Name;
    public Accelerator accelerator;
    public GameObject wall;

    public bool IsCollapsing { get => !accelerator.IsCharged; }

    Vector3 collapseDir
    {
        get => new Vector3(
            PlayerManager.Instance.playerSpawnpoint.position.x - wall.transform.position.x,
            0f,
            PlayerManager.Instance.playerSpawnpoint.position.z - wall.transform.position.z).normalized;
    }

    public void Collapse(float speed)
    {
        Debug.DrawRay(wall.transform.position, collapseDir * 10f, Color.blue);
        wall.transform.Translate(wall.transform.InverseTransformDirection(collapseDir) * speed * Time.deltaTime);
    }
}

