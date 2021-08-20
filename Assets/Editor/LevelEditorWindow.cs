
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using MapSystem;
using MovementSystem;
[Serializable]
public class LevelEditorWindow : EditorWindow
{
    Transform mapOrigin;
    Dictionary<Vector2Int, MapTile> map;

    private enum Tab
    {
        Map = 0,
        Path = 1
    }
    [SerializeField] private Tab currentTab = Tab.Map;
    private enum Mode
    {
        View = 0,
        Paint = 1,
        Erase = 2
    }
    [SerializeField] private Mode currentMode = Mode.View;

    [SerializeField] private bool showMapToolbar = false;
    [SerializeField] private static bool showMapCoordniate = false;
    [SerializeField] private bool showPathToolBar;

    // [SerializeField] private int _mapHeight = 7;
    // [SerializeField] private int _mapWidth = 20;
    private const float _mapTileWidth = 1f;
    [SerializeField] private GameObject _baseTile;
    [MenuItem("Window/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorWindow>("Level Editor");
    }


    private SerializedObject serializedObject;
    // private SerializedProperty baseTile;
    // private SerializedProperty mapHeight;
    // private SerializedProperty mapWidth;

    private void OnEnable()
    {
        var data = EditorPrefs.GetString("LevelEditorWindow", JsonUtility.ToJson(this, false));
        JsonUtility.FromJsonOverwrite(data, this);
        serializedObject = new SerializedObject(this);
        // baseTile = serializedObject.FindProperty("_baseTile");
        // mapHeight = serializedObject.FindProperty("_mapHeight");
        // mapWidth = serializedObject.FindProperty("_mapWidth");

        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;
        mapOrigin = FindObjectOfType<MapManager>()?.transform ?? new GameObject("Map", typeof(MapManager)).transform;

        RefreshPalette();
    }
    private void OnDisable()
    {
        EditorPrefs.SetString("LevelEditorWindow", JsonUtility.ToJson(this, false));
        SceneView.duringSceneGui -= OnSceneGUI;
    }
    void OnFocus()
    {
        mapOrigin = FindObjectOfType<MapManager>()?.transform ?? new GameObject("Map", typeof(MapManager)).transform;
        RefreshPalette();
        UpdateMap();
    }

    private List<GameObject> palette = new List<GameObject>();
    private List<GUIContent> paletteIcons = new List<GUIContent>();
    private Vector2 palettePos = Vector2.zero;
    [NonSerialized] private int paletteIndex = 0;
    private const string paletteResourcePath = "Assets/Prefabs/MapTiles";

    #region  UI
    void OnGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();

        #region Palette
        EditorGUILayout.BeginVertical();
        palettePos = EditorGUILayout.BeginScrollView(palettePos);
        GUIStyle itemStyle = new GUIStyle(GUI.skin.button);
        itemStyle.fixedHeight = 110;
        itemStyle.fixedWidth = 80;
        itemStyle.padding = new RectOffset(10, 10, 10, 10);
        paletteIndex = GUILayout.SelectionGrid(paletteIndex, paletteIcons.ToArray(), 12, itemStyle);
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        #endregion

        #region Tools Panel
        EditorGUILayout.BeginVertical(GUILayout.Width(400f));

        currentTab = (Tab)GUILayout.Toolbar(
           (int)currentTab,
           Enum.GetNames(typeof(Tab)),
           GUILayout.Height(20));
        switch (currentTab)
        {
            case Tab.Map:
                DrawMapToolTab();
                break;
            case Tab.Path:
                DrawPathToolTab();
                break;
            default:
                Debug.LogError("Map Editor Tab Reference Lost.");
                break;
        }

        EditorGUILayout.EndVertical();
        #endregion

        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

    void OnSceneGUI(SceneView sceneview)
    {
        sceneview.Repaint();
        if (!showMapToolbar) { return; }

        //Draw Tool Bar
        Handles.BeginGUI();
        var ToolBarRect = new Rect((SceneView.lastActiveSceneView.camera.pixelRect.width / 6), 10, (SceneView.lastActiveSceneView.camera.pixelRect.width * 4 / 6), 40);
        GUILayout.BeginArea(ToolBarRect);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        currentMode = (Mode)GUILayout.Toolbar(
           (int)currentMode,
           Enum.GetNames(typeof(Mode)),
           GUILayout.Height(ToolBarRect.height));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        Handles.EndGUI();

        Vector3 cellCenter = GetTargetPosition(); // Refactoring, I moved some code in this function
        DisplayVisualHelp(cellCenter);
        switch (currentMode)
        {
            case Mode.View:
                return;
            case Mode.Paint:

                PlaceTile(cellCenter);
                break;
            case Mode.Erase:
                EraseTile(cellCenter);
                break;
        }
    }

    private void DrawMapToolTab()
    {
        var centerlabel = new GUIStyle(GUI.skin.label);
        centerlabel.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("Scene View", centerlabel);

        EditorGUILayout.BeginHorizontal();
        showMapToolbar = GUILayout.Toggle(showMapToolbar, "Show Map Toolbar");
        showMapCoordniate = GUILayout.Toggle(showMapCoordniate, "Show Map Coordinate");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Map Generator", centerlabel);
        // EditorGUILayout.ObjectField(baseTile, typeof(GameObject), new GUIContent("Tile Prefab"));
        // EditorGUILayout.PropertyField(mapHeight, new GUIContent("Map Height"));
        // EditorGUILayout.PropertyField(mapWidth, new GUIContent("Map Width"));

        // if (GUILayout.Button("Spawn New Map"))
        // {
        //     CreateNewMap();
        // }
        if (GUILayout.Button("Erase Whole Map") && EditorUtility.DisplayDialog("Erase whole Map?", "Are you sure you want to erase the whole map?\nThis action cannot be undone.", "Yes", "Cancel"))
        {
            EraseWholeMap();
        }
    }

    private void DrawPathToolTab()
    {
        showPathToolBar = GUILayout.Toggle(showPathToolBar, "Show Path Toolbar");
        if (GUILayout.Button("Save Current Path As PathData"))
        {
            PathData asset = ScriptableObject.CreateInstance<PathData>();

            string savePath = EditorUtility.SaveFilePanelInProject("Save PathData", $"New{typeof(PathData).Name}", "asset","Choose where to save the PathData.");
            if (savePath == "") return;
            
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(savePath);

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }


    private void DisplayVisualHelp(Vector3 center)
    {
        // Vertices
        Vector3[] lines = {
        center+new Vector3(0,0,_mapTileWidth*0.5f)
        , center+new Vector3(0.5f,0,_mapTileWidth*0.25f)

        , center+new Vector3(0.5f,0,_mapTileWidth*0.25f)
        , center+new Vector3(0.5f,0,-_mapTileWidth*0.25f)

        , center+new Vector3(0.5f,0,-_mapTileWidth*0.25f)
        , center+new Vector3(0,0,-_mapTileWidth*0.5f)

        , center+new Vector3(0,0,-_mapTileWidth*0.5f)
        , center+new Vector3(-0.5f,0,-_mapTileWidth*0.25f)

        , center+new Vector3(-0.5f,0,-_mapTileWidth*0.25f)
        , center+new Vector3(-0.5f,0,_mapTileWidth*0.25f)

        , center+new Vector3(-0.5f,0,_mapTileWidth*0.25f)
        , center+new Vector3(0,0,_mapTileWidth*0.5f)
        };

        // Rendering
        Handles.color = Color.green;
        Handles.DrawLines(lines);
    }
    void RefreshPalette()
    {
        palette.Clear();
        System.IO.Directory.CreateDirectory(paletteResourcePath);
        //Load everything that's prefab
        string[] prefabFiles = System.IO.Directory.GetFiles(paletteResourcePath, "*.prefab");
        foreach (string prefabFile in prefabFiles)
            palette.Add(AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject);

        paletteIcons.Clear();
        foreach (GameObject prefab in palette)
            // Get a preview for the prefab
            paletteIcons.Add(new GUIContent(AssetPreview.GetAssetPreview(prefab)));


    }
    #endregion

    #region MapEditing
    private Vector3 GetTargetPosition()
    {
        // Get the mouse position in world space such as z = 0
        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.y / guiRay.direction.y);
        // Get the corresponding cell on our virtual grid
        Vector2Int mapPos = MapManager.GetMapPositionFromWorldPosition(mousePosition, _mapTileWidth);
        Vector3 worldPos = MapManager.GetWorldPositionFromMapPosition(mapPos, _mapTileWidth);

        return worldPos;
    }
    private void PlaceTile(Vector3 worldPos)
    {
        // Filter the left click so that we can't select objects in the scene
        if (Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(0); // Consume the event
        }

        // We have a prefab selected and we are clicking in the scene view with the left button
        if (paletteIndex < palette.Count && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Vector2Int mapPos = MapManager.GetMapPositionFromWorldPosition(worldPos, _mapTileWidth);

            //If tile already occupied, then replace it with the new one.
            if (map.ContainsKey(mapPos))
            {
                // Undo.DestroyObjectImmediate(map[qr].gameObject);
                DestroyImmediate(map[mapPos].gameObject);
                map.Remove(mapPos);
            }

            // Create the prefab instance while keeping the prefab link
            GameObject tile = PrefabUtility.InstantiatePrefab(palette[paletteIndex]) as GameObject;
            tile.transform.position = worldPos;
            tile.transform.SetParent(mapOrigin);
            MapTile mapTile = tile.GetComponent<MapTile>();
            mapTile.Init(mapPos);
            map.Add(mapPos, mapTile);
            // Allow the use of Undo (Ctrl+Z, Ctrl+Y).
            // Undo.RegisterCreatedObjectUndo(tile, "");
        }
    }
    private void EraseTile(Vector3 worldPos)
    {
        // Filter the left click so that we can't select objects in the scene
        if (Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(0); // Consume the event
        }

        // We have a prefab selected and we are clicking in the scene view with the left button
        if (paletteIndex < palette.Count && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Vector2Int mapPos = MapManager.GetMapPositionFromWorldPosition(worldPos, _mapTileWidth);

            if (map.ContainsKey(mapPos))
            {
                // Undo.DestroyObjectImmediate(map[qr].gameObject);
                DestroyImmediate(map[mapPos].gameObject);
                map.Remove(mapPos);
            }
        }
    }

    #endregion

    private void UpdateMap()
    {
        if (map == null) map = new Dictionary<Vector2Int, MapTile>();
        else map.Clear();
        for (int i = 0; i < mapOrigin.childCount; i++)
        {
            if (mapOrigin.GetChild(i).TryGetComponent<MapTile>(out var mapTile))
            {
                mapTile.Init(MapManager.GetMapPositionFromWorldPosition(mapTile.transform.position, _mapTileWidth));
                map.Add(mapTile.MapPosition, mapTile);
                EditorUtility.SetDirty(mapTile);
            }
        }
    }
    private void EraseWholeMap()
    {
        DestroyImmediate(mapOrigin.gameObject);
        map.Clear();
        mapOrigin = new GameObject("Map", typeof(MapManager)).transform;
    }

    [DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.InSelectionHierarchy)]
    static void DrawHexTileCoord(MapTile tile, GizmoType gizmoType)
    {
        if (!showMapCoordniate) return;
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.normal.textColor = Color.black;
        Handles.Label(tile.transform.position + Vector3.left * 0.3f, $"({tile.MapPosition.x}, {tile.MapPosition.y})", labelStyle);
    }
    // public void CreateNewMap()
    // {
    //     if (hexMapManager.transform.childCount != 0)
    //     {
    //         Debug.Log("Please Destroy Current Map First.");
    //         return;
    //     }
    //     for (int r = 0; r < _mapHeight; r++)
    //     {
    //         //Because we're using Axial coordinates(rhombus) to store our "semi-kinda-nearly-close-to-rectangular map", we need 
    //         //to offset the coordinate q to cancel out(offset) the distortion.
    //         //And we done that by making additional offset every two row(r).
    //         //"shfit right" is faster than "divided by 2" while keeping the same outcome.
    //         //Beware the "-" sign, if left without parentheses,it will change the r variable to negative number before shifting,
    //         //result in weird edge on your map.
    //         int offset = -(r >> 1);
    //         for (int q = offset; q < _mapWidth + offset; q++)
    //         {
    //             GameObject tile = PrefabUtility.InstantiatePrefab(_baseTile) as GameObject;
    //             tile.transform.position = HexMapManager.GetWorldCoordFromAxialCoord(q, r, _hexWidth);
    //             tile.transform.SetParent(hexMapManager.transform);
    //             tile.GetComponent<HexTile>().Init(q, r);
    //         }
    //     }
    //     hexMapManager.mapHeight = _mapHeight;
    //     hexMapManager.mapWidth = _mapWidth;
    //     hexMapManager.hexWidth = _hexWidth;
    // }

    // public void DestroyCurrentMap()
    // {
    //     if (hexMapManager.transform.childCount == 0) { Debug.Log("There is no map, nomad."); return; }

    //     while (hexMapManager.transform.childCount > 0)
    //     {
    //         DestroyImmediate(hexMapManager.transform.GetChild(0).gameObject);
    //     }
    //     Debug.Log("Old Map Destroyed.");
    // }

}
