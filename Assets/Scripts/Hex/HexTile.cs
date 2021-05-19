using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hex
{
    [RequireComponent(typeof(MeshCollider))]
    public class HexTile : MonoBehaviour
    {
        public HexTileData data;

        MeshRenderer meshRenderer;
        void Start()
        {
            //Fetch the mesh renderer component from the GameObject
            meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        void OnMouseOver()
        {
            // Change the color of the GameObject to red when the mouse is over GameObject
            meshRenderer.material.color = data.mouseOverColor;
        }
        void OnMouseDown()
        {
            data.currentColor=data.seletedColor;
        }
        void OnMouseExit()
        {
            // Reset the color of the GameObject back to normal
            meshRenderer.material.color = data.currentColor;
        }
    }

    [System.Serializable]
    public class HexTileData
    {
        public string originalPrefabName;
        public int q, r;

        //This stores the GameObject’s original color
        public Color originalColor = Color.gray;
        //When the mouse hovers over the GameObject, it turns to this color (red)
        public Color mouseOverColor = Color.red;
        //When the GameObject, it turns to this color (blue)
        public Color seletedColor = Color.blue;
        public Color currentColor = Color.gray;
        public HexTileData(string originalPrefabName, int q, int r)
        {
            this.originalPrefabName = originalPrefabName;
            this.q = q;
            this.r = r;
        }
    }
}