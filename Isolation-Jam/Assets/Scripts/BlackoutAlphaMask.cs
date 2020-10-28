using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlackoutAlphaMask : MonoBehaviour
{
    GameObject[] Walls
    {
        get => CollapseManager.Instance.worldEdges.Select(x => x.wall).ToArray();
    }

    MeshRenderer mr;

    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        // Обновляем маску.
        List<Vector3> corners = GetNearestCorners();
        //Vector3 min = new Vector3(Mathf.Infinity, 0f, Mathf.Infinity);
        //Vector3 max = new Vector3(Mathf.NegativeInfinity, 0f, Mathf.NegativeInfinity);

        //for (int i = 0; i < corners.Count; i++)
        //{
        //    if (corners[i].x < min.x && corners[i].z < min.z)
        //    {
        //        min = corners[i];
        //        continue;
        //    }

        //    if (corners[i].x > max.x && corners[i].z > max.z)
        //        max = corners[i];
        //}

        mr.material.SetFloat("_MinX", corners.Select(f=>f.x).Min());
        mr.material.SetFloat("_MaxX", corners.Select(f => f.x).Max() - 1f);
        mr.material.SetFloat("_MinZ", corners.Select(f => f.z).Min());
        mr.material.SetFloat("_MaxZ", corners.Select(f => f.z).Max() - 1f);
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
