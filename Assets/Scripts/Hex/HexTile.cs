using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hex
{
    [RequireComponent(typeof(MeshCollider))]
    public class HexTile : MonoBehaviour
    {
        public HexTileData data;

        public void Init(string baseTileName, int q, int r)
        {
            data = new HexTileData(baseTileName, q, r);
            gameObject.name = $"{q}, {r}";
            data.currentColor = data.originalColor = GetComponentInChildren<MeshRenderer>().material.color;

        }
        public void LoadData(HexTileData data)
        {
            this.data = data;
            gameObject.name = $"{data.q}, {data.r}";
            GetComponentInChildren<MeshRenderer>().material.color = data.currentColor;

        }

        MeshRenderer meshRenderer;
        void Start()
        {
            //Fetch the mesh renderer component from the GameObject
            meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        void OnMouseEnter()
        {
            // Change the color of the GameObject to red when the mouse is over GameObject
            //meshRenderer.material.color = HexMapSystem.Instance.mouseOverColor;
            meshRenderer.material.SetFloat("Boolean_1dbae06b4e654335b15f13bbc0562c0c",1f);
        }
        void OnMouseDown()
        {
            data.currentColor = HexMapSystem.Instance.seletedColor;
            meshRenderer.material.color = data.currentColor;

        }
        void OnMouseExit()
        {
            // Reset the color of the GameObject back to normal
            //meshRenderer.material.color = data.currentColor;
            meshRenderer.material.SetFloat("Boolean_1dbae06b4e654335b15f13bbc0562c0c",0f);
        }
    }

    [System.Serializable]
    public class HexTileData
    {
        public string originalPrefabName;
        public int q, r;

        //This stores the GameObject’s original color
        public Color originalColor = Color.gray;

        public Color currentColor = Color.gray;
        public HexTileData(string originalPrefabName, int q, int r)
        {
            this.originalPrefabName = originalPrefabName;
            this.q = q;
            this.r = r;
        }
    }
    public enum GroundLevel
    {
        High,Low
    }
}