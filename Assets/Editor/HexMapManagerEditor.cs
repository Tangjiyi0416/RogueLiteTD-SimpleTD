using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Hex;

[CustomEditor(typeof(HexMapSystem))]
public class HexMapManagerEditor : Editor
{
    HexMapSystem hexMapManager;

    public override void OnInspectorGUI(){
        DrawDefaultInspector();

        hexMapManager = target as HexMapSystem;
        if(GUILayout.Button("Spawn New Map")){
            hexMapManager.CreateNewMap();
        }
        if(GUILayout.Button("Destroy current Map")){
            hexMapManager.DestroyCurrentMap();
        }
        if(GUILayout.Button("Save current Map")){
            hexMapManager.SaveCurrentMap();
        }
        if(GUILayout.Button("Load saved Map")){
            if(hexMapManager.map!=null) Debug.Log("Pls Destroy current map first.");
            else hexMapManager.LoadSavedMap();
        }
    }
    
}
