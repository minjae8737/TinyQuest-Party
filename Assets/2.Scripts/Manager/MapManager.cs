using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [SerializeField] private Transform MapParent;
    [SerializeField] private Tilemap curTilemap;

    [SerializeField] private List<Tilemap> tilemaps;
    
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
        ChangeMap(0);
    }

    private void UpdateMapSize()
    {
        curTilemap.CompressBounds();
        BoundsInt cellBounds = curTilemap.cellBounds;
        mapSizeVector = cellBounds.size;
    }

    public void ChangeMap(int mapIdx)
    {
        for (int i = 0; i < tilemaps.Count; i++)
        {
            bool active = i == mapIdx;
            tilemaps[i].gameObject.SetActive(active);
        }

        curTilemap = tilemaps[mapIdx];
        UpdateMapSize();
    }
}