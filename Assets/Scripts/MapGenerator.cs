using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] _mapParts;
    [SerializeField] private GameObject _floorPrefab;
    [SerializeField] private Transform _parent;
    private string[] _readLines = default!;
    private string _path = @"Assets\StreamingAssets\map01.csv";

    private int _mapWidth = 10;
    private int _mapHeight = 10;

    private int _width { get; set; }
    private int _height { get; set; }

    private List<NavMeshSurface> _allSurfaces = new List<NavMeshSurface>();
    private List<Vector3> _allEdgePoints = new List<Vector3>();
    private float _threshold = 1.5f;

    private void Awake()
    {
        ReadData(_path);
        InitMap();
    }

    private void ReadData(string path)
    {
        try
        {
            _readLines = File.ReadAllLines(path);
        }
        catch (Exception ex)
        {
            Debug.Log($"{path} 読み込みエラー: {ex.Message}");
        }

        if (_readLines.Last().Trim() == "") _readLines = _readLines.Take(_readLines.Length - 1).ToArray();//最終行が空行なら削除

        _height = _readLines.Length;
        _width = _readLines[0].Split(',').Length;
    }

    private void InitMap()
    {
        for (int x = 0; x < _width; x++)
        {
            var cells = _readLines[x].Split(',');

            for (int y = 0; y < _height; y++)
            {
                if (int.TryParse(cells[y], out int num))
                {
                    var pos = new Vector3(x * _mapWidth, 0, y * _mapHeight);
                    if (num != 99)
                    {
                        var parts = Instantiate(_mapParts[num], pos, Quaternion.identity, _parent);
                    }

                    var floor = Instantiate(_floorPrefab, pos, Quaternion.identity, _parent);

                    // NavMeshSurfaceを収集
                    var surface = floor.GetComponent<NavMeshSurface>();
                    if (surface != null) _allSurfaces.Add(surface);

                    //var points = floor.GetComponent<Floor>().GetEdgePoints();
                    //_allEdgePoints.AddRange(points);
                }
                else
                {
                    Debug.Log("CSVデータに不備があります");
                }
            }
        }

        // すべてのSurfaceを一括ベイク
        foreach (var surface in _allSurfaces)
        {
            surface.BuildNavMesh();
        }

        var scale = Vector3.one;
        scale.y /= 4;
        _parent.transform.localScale = scale;

        // 一括ベイクしたので接続処理いらないかも？
        //ConnectNearbyPoints(_allEdgePoints, _threshold);
    }

    private void ConnectNearbyPoints(List<Vector3> allPoints, float threshold)
    {
        for (int i = 0; i < allPoints.Count; i++)
        {
            for (int j = i + 1; j < allPoints.Count; j++)
            {
                if (Vector3.Distance(allPoints[i], allPoints[j]) < threshold)
                {
                    GameObject linkObj = new GameObject("NavLink");
                    NavMeshLink link = linkObj.AddComponent<NavMeshLink>();
                    link.startTransform = CreateTransformAt(allPoints[i]);
                    link.endTransform = CreateTransformAt(allPoints[j]);
                    link.width = 1.0f;
                    link.UpdateLink();
                    linkObj.transform.parent = _parent;
                }
            }
        }
    }

    private Transform CreateTransformAt(Vector3 position)
    {
        GameObject pointObj = new GameObject("LinkPoint");
        pointObj.transform.position = position;
        pointObj.transform.parent = _parent;
        return pointObj.transform;
    }
}
