
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Hex;
public class HexMapEditor : EditorWindow
{
    [MenuItem("Window/HexMapEditor")]
    public static void ShowWindow()
    {
        GetWindow<HexMapEditor>("Hex Map Editor by Tang ji-yi");
    }

    [SerializeField]
    private GameObject _tilePrefab;

    private SerializedObject obj;
    private SerializedProperty tilePrefab;

    private void OnEnable()
    {
        obj = new SerializedObject(this);
        tilePrefab = obj.FindProperty("_tilePrefab");
    }

    void OnGUI()
    {
        EditorGUILayout.ObjectField(tilePrefab, typeof(GameObject), new GUIContent("Tile Prefab"));
        if (GUILayout.Button("Spawn New Map"))
        {
            HexMapSystem.Instance.CreateNewMap();
        }
        if (GUILayout.Button("Destroy current Map"))
        {
            HexMapSystem.Instance.DestroyCurrentMap();
        }
        if (GUILayout.Button("Save current Map"))
        {
            HexMapSaveLoadManager.Instance.SaveCurrentMap();
        }
        if (GUILayout.Button("Load saved Map"))
        {
            if (HexMapSystem.Instance.map != null) Debug.Log("Pls Destroy current map first.");
            else HexMapSaveLoadManager.Instance.LoadSavedMap();
        }
    }
}
