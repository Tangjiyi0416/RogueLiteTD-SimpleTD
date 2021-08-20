using System;
using System.Collections.Generic;
using UnityEngine;

namespace MapSystem
{
    [RequireComponent(typeof(MeshCollider))]
    [Serializable]
    public class MapTile : MonoBehaviour
    {
        // public MapTileData data;
        public GroundLevel groundLevel;
        [InspectorReadOnly] [SerializeField] private Vector2Int mapPosition;
        public Vector2Int MapPosition { get => mapPosition; protected set => mapPosition = value; }

        public bool isObstacle = false;
        public void Init(Vector2Int mapPosition)
        {
            this.MapPosition = mapPosition;
            gameObject.name = $"{mapPosition.x}, {mapPosition.y}";

        }

        MeshRenderer meshRenderer;
        void Start()
        {
            //Fetch the mesh renderer component from the GameObject
            meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        void OnMouseEnter()
        {
            meshRenderer.material.SetFloat("Boolean_1dbae06b4e654335b15f13bbc0562c0c", 1f);
        }
        void OnMouseExit()
        {
            meshRenderer.material.SetFloat("Boolean_1dbae06b4e654335b15f13bbc0562c0c", 0f);
        }
    }
    public enum GroundLevel
    {
        High, Low
    }
}