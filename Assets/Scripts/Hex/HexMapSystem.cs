using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hex
{
    public class HexMapSystem : MonoBehaviour
    {

        [SerializeField]
        private int mapHeight = 7, mapWidth = 20;
        [SerializeField]
        private float hexSize = 1f;
        [SerializeField]
        private GameObject baseTile;

        public Color seletedColor = Color.blue;//When click on the GameObject, it turns to this color (blue)
        ///<summary>The key (int, int) is the tuple of (coordinate q, coordiante r)</summary>
        //Dictionary is a special data structure that can use a "Key" to access specific item in the storage.
        //In this case, I utilize its "Key" as coordinates for our map, so that we can easily edit the targeted
        //map tile by calling "HexMap.map[(q,r)]".
        //Notice that the "(int, int)" is a "tuple", which is, yet again, a data structure, look it up on Google if you are interested.
        public Dictionary<(int, int), HexTile> map;

        static HexMapSystem instance = null;
        public static HexMapSystem Instance
        {
            get { return instance ?? (instance = FindObjectOfType(typeof(HexMapSystem)) as HexMapSystem); }
        }

        ///<param name="mapHeight">How many tiles can vertically fit .</param>
        ///<param name="mapWidth">How many tiles can horizonatlly fit.</param>
        public void CreateNewMap()
        {
            if (map != null) DestroyCurrentMap();
            map = new Dictionary<(int, int), HexTile>();

            for (int r = 0; r < mapHeight; r++)
            {
                //Because we're using Axial coordinates(rhombus) to store our "semi-kinda-nearly-close-to-rectangular map", we need 
                //to offset the coordinate q to cancel out(offset) the distortion.
                //And we done that by making additional offset every two row(r).
                //"shfit right" is faster than "divided by 2" while keeping the same outcome.
                //Beware the "-" sign, if left without parentheses,it will change the r variable to negative number before shifting,
                //result in weird edge on your map.
                int offset = -(r >> 1);
                for (int q = offset; q < mapWidth + offset; q++)
                {
                    map.Add((q, r), Instantiate(baseTile, GetWorldCoordFromAxialCoord(q, r), Quaternion.identity, gameObject.transform).GetComponent<HexTile>());
                    map[(q, r)].Init(baseTile.name, q, r);
                }
            }
        }

        public void DestroyCurrentMap()
        {
            if (map == null) { Debug.Log("There is no map, nomad."); return; }
            foreach (var hex in map)
            {
                GameObject.DestroyImmediate(hex.Value.gameObject);
            }
            map.Clear();
            map = null;

            Debug.Log("Old Map Destroyed.");
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

        public void SaveCurrentMap()
        {
            SaveLoadManager.Save(new HexMapSaveData(map), "map.dat");
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
                        , GetWorldCoordFromAxialCoord(q, r)
                        , Quaternion.identity
                        , gameObject.transform
                    ).GetComponent<HexTile>()
                );
                map[(q, r)].LoadData(tileData);

            }
            this.map = map;

        }


        const float sqrt3 = 1.7320508f;
        const float inverSqrt3 = 0.5773502f;
        //Methods that convert coordinates to world or to map.
        public Vector3 GetWorldCoordFromAxialCoord(int mapQ, int mapR)
        {
            return new Vector3((mapQ + mapR / 2f) * sqrt3 * hexSize, 0, mapR * 1.5f * hexSize);
        }
        public (int, int) GetAxialCoordFromWorldCoord(float worldX, float worldZ)
        {
            return HexRounding(((worldX * inverSqrt3 - 1f / 3 * worldZ) / hexSize, 2f / 3 * worldZ / hexSize));
        }
        public Vector3 GetWorldCoordFromOffsetCoord(int col, int row)
        {
            return new Vector3(hexSize * sqrt3 * col + ((row & 1) >> 1), hexSize * 1.5f * row);
        }
        public (int, int) GetOffsetCoordFromWorldCoord(float worldX, float worldZ)
        {
            var axial = GetAxialCoordFromWorldCoord(worldX, worldZ);
            return (axial.Item1 + (axial.Item2 >> 1), axial.Item2);
        }

        private (int, int) HexRounding((float, float) co)
        {
            //this xyz refers to "cube coordinates", not world coordinates!
            int x = Mathf.RoundToInt(co.Item1);
            int y = Mathf.RoundToInt(co.Item2);
            int z = Mathf.RoundToInt(-co.Item1 - co.Item2);

            float diffX = Mathf.Abs(x - co.Item1);
            float diffY = Mathf.Abs(y - co.Item2);
            float diffZ = Mathf.Abs(z + co.Item1 + co.Item2);

            if (diffX > diffY && diffX > diffZ) x = -y - z;
            else if (diffY > diffZ) y = -x - z;
            return (x, y);
        }

        void OnGUI()
        {
            for (int r = 0; r < mapHeight; r++)
            {
                int offset = -(r >> 1);
                for (int q = offset; q < mapWidth + offset; q++)
                {
                    drawString($"{q}, {r}", GetWorldCoordFromAxialCoord(q, r) + Vector3.up * 0.15f, Color.black);
                }
            }


        }
        static public void drawString(string text, Vector3 worldPos, Color? colour = null)
        {
            var restoreColor = GUI.color;

            if (colour.HasValue) GUI.color = colour.Value;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
            {
                //    GUI.color = restoreColor;
                return;
            }
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
            //Debug.Log(screenPos.y);
            GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + Screen.height, size.x, size.y), text);
            GUI.color = restoreColor;
        }

        Plane m_Plane;//A ghost plane for raycasting.
                      //Vector3 m_DistanceFromCamera;//Make some space between plane(overlaps the map) and Camera
                      //public float m_DistanceY;//same above

        protected virtual void Awake()
        {
            if (instance == null) instance = this as HexMapSystem;
            if (instance == this) DontDestroyOnLoad(this);
            else DestroyImmediate(this);
        }

        void Start()
        {

            Camera.main.transform.position = new Vector3((mapWidth - 1) * hexSize * 1.7320508f / 2, +10, (mapHeight - 1) * hexSize - 10);
            m_Plane = new Plane(Vector3.up, Vector3.up * 0.15f);
        }
        void Update()
        {
            //Detect when there is a mouse click
            if (Input.GetMouseButtonDown(0))
            {
                //Create a ray from the Mouse click position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //Initialise the enter variable
                float enter = 0.0f;

                if (m_Plane.Raycast(ray, out enter))
                {
                    //Get the point that is clicked
                    Vector3 hitPoint = ray.GetPoint(enter);
                    (int, int) axialCoord = GetAxialCoordFromWorldCoord(hitPoint.x, hitPoint.z);
                    Debug.Log($"Axial coordinate: {axialCoord}, Offset coordinate: {GetOffsetCoordFromWorldCoord(hitPoint.x, hitPoint.z)}");
                }
            }
        }

        protected virtual void OnDestroy()
        {
            instance = null;
            if (map != null) DestroyCurrentMap();
        }
    }


}