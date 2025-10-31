using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private const float playerOffsetY = 1.5f;
    private const float mapOffset = 7.5f;
    [SerializeField] private GameObject[] _mapParts;
    [SerializeField] private GameObject _floorPrefab;
    [SerializeField] private GameObject _spawnNormal;
    [SerializeField] private GameObject[] _spawnA_Walls;
    [SerializeField] private GameObject[] _nearWalls;
    [SerializeField] private GameObject[] _bigWalls;
    [SerializeField] private GameObject[] _hintObjects;
    [SerializeField] private MessageWindow _messageWindow;
    [SerializeField] private TargetCountUIController _uiController;

    [SerializeField] private Transform _parent;
    private Vector3 _newPosition;
    private string[] _readLines = default!;
    private string[] _readEventLines = default!;
    private string _path = Path.Combine(Application.streamingAssetsPath, "map01.csv");
    private string _eventPath = Path.Combine(Application.streamingAssetsPath, "eventMap01.csv");

    private int _mapWidth = 10;
    private int _mapHeight = 10;

    private int _width { get; set; }
    private int _height { get; set; }

    private int _widthEvent { get; set; }
    private int _heightEvent { get; set; }

    private List<NavMeshSurface> _allSurfaces = new List<NavMeshSurface>();

    private void Awake()
    {
        ReadData();
        ReadEventData();
        StartCoroutine(InitMap());
    }

    private void Start()
    {
        GameModeManager.SetGameMode(GameModeManager.GameMode.Explore);
    }

    private void ReadData()
    {
        try
        {
            _readLines = File.ReadAllLines(_path);
        }
        catch (Exception ex)
        {
            Debug.Log($"{_path} 読み込みエラー: {ex.Message}");
        }

        if (_readLines.Last().Trim() == "") _readLines = _readLines.Take(_readLines.Length - 1).ToArray();//最終行が空行なら削除

        _height = _readLines.Length;
        _width = _readLines[0].Split(',').Length;
    }

    private void ReadEventData()
    {
        try
        {
            _readEventLines = File.ReadAllLines(_eventPath);
        }
        catch (Exception ex)
        {
            Debug.Log($"{_eventPath} 読み込みエラー: {ex.Message}");
        }

        if (_readEventLines.Last().Trim() == "") _readEventLines = _readEventLines.Take(_readEventLines.Length - 1).ToArray();

        _heightEvent = _readEventLines.Length;
        _widthEvent = _readEventLines[0].Split(',').Length;
    }

    IEnumerator InitMap()
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
                }
                else
                {
                    Debug.Log("map.CSVデータに不備があります");
                }
            }
        }

        var floor = Instantiate(_floorPrefab, new Vector3(60,0,60), Quaternion.identity, _parent);

        // NavMeshSurfaceを収集
        var surface = floor.GetComponent<NavMeshSurface>();
        //if (surface != null) _allSurfaces.Add(surface);

        InitEvent();

        var scale = Vector3.one * 0.75f;
        scale.y /= 3.0f;
        _parent.transform.localScale = scale;
        _parent.transform.position = _newPosition;

        // すべてのSurfaceを一括ベイク
        surface.BuildNavMesh();

        yield return null;
    }

    private void InitEvent()
    {
        for (int x = 0; x < _widthEvent; x++)
        {
            var cells = _readEventLines[x].Split(',');

            for (int y = 0; y < _heightEvent; y++)
            {
                var pos = new Vector3(x * _mapWidth, 0, y * _mapHeight);

                if (cells[y] == "00") { } // 何もしない
                else if (cells[y] == "SS")
                {
                    _newPosition = new Vector3(-x * mapOffset, 0, -y * mapOffset - playerOffsetY);
                    var hint = Instantiate(_hintObjects[0], pos, Quaternion.identity, _parent);
                    hint.GetComponent<HintMessage>().SetCurrentMessage(0, _messageWindow);
                }
                else if (cells[y] == "N0")
                {
                    Instantiate(_spawnNormal, pos, Quaternion.identity, _parent);
                }
                else if (cells[y].StartsWith("W"))
                {
                    int.TryParse(cells[y].Substring(1, 1), out int code);
                    Instantiate(_nearWalls[code - 1], pos, Quaternion.identity, _parent);
                }
                else if (cells[y] == "A1")
                {
                    var aw = Instantiate(_spawnA_Walls[0], pos, Quaternion.identity, _parent);
                    aw.GetComponentInChildren<SpawnerAnnihilate>().SetUiController(_uiController);
                }
                else if (cells[y].StartsWith("U"))
                {
                    var hint = Instantiate(_hintObjects[0], pos, Quaternion.identity, _parent);
                    int.TryParse(cells[y].Substring(1, 1), out int code);
                    hint.GetComponent<HintMessage>().SetCurrentMessage(code - 1, _messageWindow);
                }
                else if (cells[y].StartsWith("D"))
                {
                    var hint = Instantiate(_hintObjects[1], pos, Quaternion.identity, _parent);
                    int.TryParse(cells[y].Substring(1, 1), out int code);
                    hint.GetComponent<HintMessage>().SetCurrentMessage(code - 1, _messageWindow);
                }
                else if (cells[y].StartsWith("L"))
                {
                    var hint = Instantiate(_hintObjects[2], pos, Quaternion.identity, _parent);
                    int.TryParse(cells[y].Substring(1, 1), out int code);
                    hint.GetComponent<HintMessage>().SetCurrentMessage(code - 1, _messageWindow);
                }
                else if (cells[y].StartsWith("R"))
                {
                    var hint = Instantiate(_hintObjects[3], pos, Quaternion.identity, _parent);
                    int.TryParse(cells[y].Substring(1, 1), out int code);
                    hint.GetComponent<HintMessage>().SetCurrentMessage(code - 1, _messageWindow);
                }
                else if (cells[y].StartsWith("B"))
                {
                    int.TryParse(cells[y].Substring(1, 1), out int code);
                    Instantiate(_bigWalls[code - 1], pos, Quaternion.identity, _parent);
                }
                else
                {
                    Debug.Log("eventMap.CSVデータに不備があります");
                }
            }
        }
    }
}
