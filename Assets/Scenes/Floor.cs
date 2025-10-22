using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private const float floorSize = 10f;

    public List<Vector3> GetEdgePoints()
    {
        Vector3 size = new Vector3(floorSize * transform.localScale.x, 0f, floorSize * transform.localScale.z);
        Vector3 center = transform.position;

        List<Vector3> points = new List<Vector3>();
        points.Add(center + new Vector3(size.x / 2f, 0f, 0f));  // 右端
        points.Add(center - new Vector3(size.x / 2f, 0f, 0f));  // 左端
        points.Add(center + new Vector3(0f, 0f, size.z / 2f));  // 上端
        points.Add(center - new Vector3(0f, 0f, size.z / 2f));  // 下端

        return points;
    }
}
