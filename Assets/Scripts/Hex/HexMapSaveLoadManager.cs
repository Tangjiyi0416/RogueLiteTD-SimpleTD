using Hex;
using UnityEngine;
using System.Collections.Generic;
public class HexMapSaveLoadManager : MonoBehaviour
{
    HexMapSystem hexMapSystem;
    static HexMapSaveLoadManager instance = null;
    public static HexMapSaveLoadManager Instance
    {
        get { return instance ?? (instance = FindObjectOfType(typeof(HexMapSaveLoadManager)) as HexMapSaveLoadManager); }
    }
    private void Awake()
    {
        hexMapSystem = HexMapSystem.Instance;
        if (instance == null) instance = this as HexMapSaveLoadManager;
        if (instance == this) DontDestroyOnLoad(this);
        else DestroyImmediate(this);
    }

    public void SaveCurrentMap()
    {
        SaveLoadManager.Save(new HexMapSaveData(hexMapSystem.map), "map.dat");
    }
    public void LoadSavedMap()
    {
        HexMapSaveData saveData = SaveLoadManager.Load<HexMapSaveData>("map.dat");
        List<HexTileData> data = saveData.data;
        Dictionary<(int, int), HexTile> map = new Dictionary<(int, int), HexTile>();
        foreach (var tileData in data)
        {
            int q = tileData.q, r = tileData.r;
            map.Add(
                (q, r)
                , Instantiate(
                    Resources.Load($"HexTiles/{tileData.originalPrefabName}") as GameObject
                    , hexMapSystem.GetWorldCoordFromAxialCoord(q, r)
                    , Quaternion.identity
                    , gameObject.transform
                ).GetComponent<HexTile>()
            );
            map[(q, r)].LoadData(tileData);

        }
        hexMapSystem.map = map;

    }
}
[System.Serializable]
public class HexMapSaveData : SaveData
{
    public List<HexTileData> data = new List<HexTileData>();
    public HexMapSaveData(Dictionary<(int, int), HexTile> map)
    {
        foreach (var tile in map)
        {
            data.Add(tile.Value.data);
        }
    }
}
