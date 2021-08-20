using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MapSystem
{
    public class MapManager : MonoBehaviour
    {
        public float hexWidth = 1f;

        //Dictionary is a special data structure that can use a "Key" to access specific item in the storage.
        //In this case, I utilize its "Key" as coordinates for our map, so that we can easily edit the targeted
        //map tile by calling "HexMap.map[(Vector2Int)mapPosition]".
        public Dictionary<Vector2Int, MapTile> map;

        static MapManager instance = null;
        public static MapManager Instance
        {
            get { return instance ?? (instance = FindObjectOfType<MapManager>()); }
        }

        /// <param name="mapHeight">How many tiles can vertically fit .</param>
        /// <param name="mapWidth">How many tiles can horizonatlly fit.</param>

        // public void CreateNewMap()
        // {
        //     if (map != null) DestroyCurrentMap();
        //     map = new Dictionary<Vector2Int, HexTile>();

        //     for (int r = 0; r < mapHeight; r++)
        //     {
        //         //Because we're using Axial coordinates(rhombus) to store our "semi-kinda-nearly-close-to-rectangular map", we need 
        //         //to offset the coordinate q to cancel out(offset) the distortion.
        //         //And we done that by making additional offset every two row(r).
        //         //"shfit right" is faster than "divided by 2" while keeping the same outcome.
        //         //Beware the "-" sign, if left without parentheses,it will change the r variable to negative number before shifting,
        //         //result in weird edge on your map.
        //         int offset = -(r >> 1);
        //         for (int q = offset; q < mapWidth + offset; q++)
        //         {
        //             map.Add((q, r), Instantiate(baseTile, GetWorldCoordFromAxialCoord(q, r), Quaternion.identity, gameObject.transform).GetComponent<HexTile>());
        //             map[(q, r)].Init(baseTile.name, q, r);
        //         }
        //     }
        // }

        // public void DestroyCurrentMap()
        // {
        //     if (map == null && transform.childCount == 0) { Debug.Log("There is no map, nomad."); return; }

        //     while (transform.childCount > 0)
        //     {
        //         DestroyImmediate(transform.GetChild(0).gameObject);
        //     }

        //     map?.Clear();
        //     map = null;
        //     Debug.Log("Old Map Destroyed.");
        // }
        public void UpdateMapFromChildren()
        {
            if (map == null) map = new Dictionary<Vector2Int, MapTile>();
            else map.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent<MapTile>(out var mapTile))
                {
                    map.Add(mapTile.MapPosition, mapTile);
                }
            }
        }
        #region Coordinate Coverting Methods
        const float sqrt3 = 1.7320508f;
        const float inverSqrt3 = 0.5773502f;
        //Methods that convert coordinates to world or to map.
        public static Vector3 GetWorldPositionFromMapPosition(Vector2Int mapPos, float hexWidth)
        {
            return new Vector3((mapPos.x + mapPos.y * 0.5f) * hexWidth, 0, mapPos.y * 0.5f * hexWidth * sqrt3);
        }
        public Vector3 GetWorldPositionFromMapPosition(Vector2Int mapPos)
        {
            return new Vector3((mapPos.x + mapPos.y * 0.5f) * hexWidth, 0, mapPos.y * 0.5f * hexWidth * sqrt3);
        }
        public static Vector2Int GetMapPositionFromWorldPosition(Vector3 worldPos, float hexWidth)
        {
            return PositionRounding(new Vector2((worldPos.x - worldPos.z * inverSqrt3) / hexWidth, 2 * inverSqrt3 * worldPos.z * hexWidth));
        }
        public Vector2Int GetMapPositionFromWorldPosition(Vector3 worldPos)
        {
            return PositionRounding(new Vector2((worldPos.x - worldPos.z * inverSqrt3) / hexWidth, 2 * inverSqrt3 * worldPos.z * hexWidth));
        }
        // public static Vector3 GetWorldCoordFromOffsetCoord(int col, int row, float hexSize)
        // {
        //     return new Vector3(hexSize * sqrt3 * col + ((row & 1) >> 1), hexSize * 1.5f * row);
        // }
        // public Vector3 GetWorldCoordFromOffsetCoord(int col, int row)
        // {
        //     return new Vector3(hexSize * sqrt3 * col + ((row & 1) >> 1), hexSize * 1.5f * row);
        // }
        // public static Vector2Int GetOffsetCoordFromWorldCoord(float worldX, float worldZ, float hexSize)
        // {
        //     var axial = GetAxialCoordFromWorldCoord(worldX, worldZ, hexSize);
        //     return new Vector2Int(axial.x + (axial.x >> 1), axial.y);
        // }
        // public Vector2Int GetOffsetCoordFromWorldCoord(float worldX, float worldZ)
        // {
        //     var axial = GetAxialCoordFromWorldCoord(worldX, worldZ, hexSize);
        //     return new Vector2Int(axial.x + (axial.x >> 1), axial.y);
        // }

        private static Vector2Int PositionRounding(Vector2 co)
        {
            //this xyz refers to "cube coordinates", not world coordinates!
            int x = Mathf.RoundToInt(co.x);
            int y = Mathf.RoundToInt(co.y);
            int z = Mathf.RoundToInt(-co.x - co.y);

            float diffX = Mathf.Abs(x - co.x);
            float diffY = Mathf.Abs(y - co.y);
            float diffZ = Mathf.Abs(z + co.x + co.y);

            if (diffX > diffY && diffX > diffZ) x = -y - z;
            else if (diffY > diffZ) y = -x - z;
            return new Vector2Int(x, y);
        }
        #endregion

        #region Map Utilities
        public List<MapTile> GetNeighborsByMapPosition(Vector2Int mapPos)
        {
            List<MapTile> neighbors = new List<MapTile>();
            int[,] neighborOffset = { { 0, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 }, { -1, 0 }, { -1, 1 } };
            for (int i = 0; i < 6; i++)
            {
                map.TryGetValue(new Vector2Int(mapPos.x + neighborOffset[i, 0], mapPos.y + neighborOffset[i, 1]), out var tile);
                if (tile != null) neighbors.Add(tile);
            }
            return neighbors;
        }
        public int GetDistanceByMapPosition(Vector2Int mapPos1,Vector2Int mapPos2)
        {
            return (Mathf.Abs(mapPos1.x - mapPos2.x) + Mathf.Abs(mapPos1.x + mapPos1.y - mapPos2.x - mapPos2.y) + Mathf.Abs(mapPos1.y - mapPos2.y)) / 2;
        }
        #endregion
        protected virtual void Awake()
        {
            if (instance == null) instance = this as MapManager;
            else DestroyImmediate(this);
            UpdateMapFromChildren();
        }

        // Plane m_Plane;//A ghost plane for raycasting.
        //Vector3 m_DistanceFromCamera;//Make some space between plane(overlaps the map) and Camera
        //public float m_DistanceY;//same above

        // void Start()
        // {
        //     //Camera.main.transform.position = new Vector3((mapWidth - 1) * hexSize * 1.7320508f / 2, +10, (mapHeight - 1) * hexSize - 10);
        //     m_Plane = new Plane(Vector3.up, Vector3.up * 0.15f);
        // }
        // void Update()
        // {
        //     //Detect when there is a mouse click
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         //Create a ray from the Mouse click position
        //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //         //Initialise the enter variable
        //         float enter = 0.0f;

        //         if (m_Plane.Raycast(ray, out enter))
        //         {
        //             //Get the point that is clicked
        //             Vector3 hitPoint = ray.GetPoint(enter);
        //             Vector2Int axialCoord = GetAxialCoordFromWorldCoord(hitPoint.x, hitPoint.z);
        //             Debug.Log($"Axial coordinate: {axialCoord}, Offset coordinate: {GetOffsetCoordFromWorldCoord(hitPoint.x, hitPoint.z)}");
        //             Debug.Log($"NeighborCount: {GetNeighborsByAxialCoord(axialCoord.Item1, axialCoord.Item2).Count}");
        //         }
        //     }
        // }
    }


}