using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [SerializeField] private Transform MapParent;
    [SerializeField] private Tilemap tilemap;
    public Vector3Int mapSizeVector { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    private void Start()
    {
        UpdateMapSize();
    }

    private void UpdateMapSize()
    {
        tilemap.CompressBounds();
        BoundsInt cellBounds = tilemap.cellBounds;
        mapSizeVector = cellBounds.size;
    }
}