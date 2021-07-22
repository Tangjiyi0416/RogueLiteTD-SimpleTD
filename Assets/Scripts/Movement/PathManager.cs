using System;
using System.Collections.Generic;
using UnityEngine;
using Hex;
using PriorityQueue;
namespace MovementSystem
{

    public class PathManager : MonoBehaviour
    {
        public List<Node> GeneratePathFromWaypoints()
        {
            List<Node> finalPath = new List<Node>();
            int i = 0;
            Transform child;
            child = transform.GetChild(i);
            if (child.tag == "Waypoint")
            {
                finalPath.Add(new Node(child.position));
            }
            for (i++; i < transform.childCount; i++)
            {
                child = transform.GetChild(i);

                if (child.tag == "Waypoint")
                {
                    List<Node> path = Pathing(finalPath[finalPath.Count - 1].worldPos, child.position);
                    if (path != null)
                        finalPath.AddRange(path);
                }

            }
            return finalPath;
        }
        //A* Pathfinding here we gooooooooooooo
        private List<Node> Pathing(Vector3 startPos, Vector3 targetPos)
        {
            bool pathFound = false;
            List<Node> path = new List<Node>();
            path.Add(new Node(targetPos));
            SimplePriorityQueue<HexTile, int> frontier = new SimplePriorityQueue<HexTile, int>();
            Dictionary<HexTile, HexTile> from = new Dictionary<HexTile, HexTile>();
            Dictionary<HexTile, int> costSoFar = new Dictionary<HexTile, int>();

            frontier.Enqueue(HexMapSystem.Instance.map[HexMapSystem.Instance.GetAxialCoordFromWorldCoord(startPos.x, startPos.z)], 0);
            from.Add(frontier.First, null);
            costSoFar.Add(frontier.First, 0);

            HexTile current = null;
            while (!(frontier.Count == 0))
            {
                current = frontier.Dequeue();
                Debug.Log($"{current.data.q}, {current.data.r}");
                if ((current.data.q, current.data.r) == (path[0].q, path[0].r))
                {
                    pathFound = true;
                    break;
                }
                foreach (var next in HexMapSystem.Instance.GetNeighborsByAxialCoord(current.data.q, current.data.r))
                {
                    if (next.data.isObstacle) continue;
                    int newCost = costSoFar[current] + 1;
                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        frontier.Enqueue(next, newCost + HexMapSystem.Instance.GetDistanceByAxialCoord(path[0].q, path[0].r, next.data.q, next.data.r));
                        if (from.ContainsKey(next))
                            from[next] = current;
                        else
                            from.Add(next, current);
                    }
                }

            }
            if (!pathFound)
            {
                return null;
            }
            while (from.ContainsKey(current) && from[current] != null)
            {
                path.Add(new Node(current.data.q, current.data.r));
                current = from[current];
            }
            path.Reverse();
            return path;
        }
    }
    [Serializable]
    public class Node
    {
        public int q, r;

        public Node(int q, int r)
        {
            this.q = q;
            this.r = r;
        }
        public Node(Vector3 worldPos)
        {
            (q, r) = HexMapSystem.Instance.GetAxialCoordFromWorldCoord(worldPos.x, worldPos.z);
        }

        public Vector3 worldPos { get => HexMapSystem.Instance.GetWorldCoordFromAxialCoord(q, r); }
    }
}