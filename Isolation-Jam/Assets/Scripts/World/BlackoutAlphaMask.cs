using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlackoutAlphaMask : MonoBehaviour
{
    GameObject[] Walls
    {
        get => CollapseManager.Instance.worldEdges.Select(x => x.wall).ToArray();
    }

    // Corners.
    public float MIN_X { get; private set; }
    public float MAX_X { get; private set; }
    public float MIN_Z { get; private set; }
    public float MAX_Z { get; private set; }

    MeshRenderer mr;

    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        // Обновляем маску.
        List<Vector3> corners = GetNearestCorners();
        SetCorners(corners);

        mr.material.SetFloat("_MinX", MIN_X);
        mr.material.SetFloat("_MaxX", MAX_X);
        mr.material.SetFloat("_MinZ", MIN_Z);
        mr.material.SetFloat("_MaxZ", MAX_Z);
    }

    void SetCorners(List<Vector3> _corners)
    {
        MIN_X = _corners.Select(f => f.x).Min();
        MAX_X = _corners.Select(f => f.x).Max() - 1f;
        MIN_Z = _corners.Select(f => f.z).Min();
        MAX_Z = _corners.Select(f => f.z).Max() - 1f;
    }

    List<Vector3> GetNearestCorners()
    {
        Bounds[] bounds = GetWallsBounds();
        List<Vector3> result = new List<Vector3>();

        foreach (Bounds bi in bounds)
        {
            // Ищем 2 границы с которыми пересекается b.
            foreach (Bounds bj in bounds)
            {
                if (bi == bj)
                    continue;

                if (bi.Intersects(bj))
                {
                    // Создаем новые границы пересечения.
                    Vector3 center = new Vector3(bi.center.x, bi.center.y, bj.center.z);
                    Vector3 size = new Vector3(bi.size.x, 0f, bj.size.z);

                    // Убираем неправильные границы.
                    if (size.x >= bi.size.z || size.z >= bj.size.x)
                        continue;

                    Bounds intersectBounds = new Bounds(center, size);
                    result.Add(intersectBounds.center/*GetNearestToPointCorner(Vector3.zero, intersectBounds)*/);
                }
            }

        }

        return result;
    }
    Bounds[] GetWallsBounds()
    {
        Bounds[] result = new Bounds[Walls.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = Walls[i].GetComponent<MeshRenderer>().bounds;
        }

        return result;
    }
    Vector3 GetNearestToPointCorner(Vector3 point, Bounds bounds)
    {
        Vector3[] corners =
        {
            new Vector3(bounds.min.x,bounds.center.y,bounds.min.z),
            new Vector3(bounds.min.x,bounds.center.y,bounds.max.z),
            new Vector3(bounds.max.x,bounds.center.y,bounds.max.z),
            new Vector3(bounds.max.x,bounds.center.y,bounds.min.z),
        };

        Vector3 nearestCorner = Vector3.zero;
        float dist = Mathf.Infinity;
        for (int i = 0; i < corners.Length; i++)
        {
            if ((corners[i] - point).sqrMagnitude < dist)
            {
                nearestCorner = corners[i];
                dist = (nearestCorner - point).sqrMagnitude;
            }
        }

        return nearestCorner;
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            // Границы.
            Gizmos.color = Color.yellow;
            Bounds[] bounds = GetWallsBounds();
            foreach (Bounds b in bounds)
                Gizmos.DrawWireCube(b.center, b.size);

            // Точки Пересечения (углы).
            Gizmos.color = Color.cyan;
            List<Vector3> corners = GetNearestCorners();
            foreach (Vector3 c in corners)
                Gizmos.DrawSphere(c, 0.5f);
        }
    }
}
