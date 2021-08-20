using System;
using System.Collections.Generic;
using UnityEngine;
using MapSystem;
namespace MovementSystem
{
    [CreateAssetMenu(fileName ="New Path",menuName ="Test/PathData")]
    public class PathData : ScriptableObject
    {
        public List<Node> waypoints;
    }
    [Serializable]
    public class Node
    {
        public Vector2Int mapPosition;
        public Vector3 WorldPosition
        {
            get => MapManager.Instance.GetWorldPositionFromMapPosition(mapPosition);
        }
        public float waitTime;

        public Node(Vector2Int mapPosition, float waitTime = 0f)
        {
            this.mapPosition = mapPosition;
            this.waitTime = waitTime;
        }
        public Node(Node node){
            this.mapPosition = node.mapPosition;
            this.waitTime = node.waitTime;
        }

    }

}