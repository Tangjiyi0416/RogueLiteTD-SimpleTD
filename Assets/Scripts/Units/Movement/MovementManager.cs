using System;
using System.Collections.Generic;
using PriorityQueue;
using UnityEngine;
using MapSystem;
namespace MovementSystem
{

    public class MovementManager : MonoBehaviour
    {
        public PathData pathData;//Test only, remove after Wave Spawner setup
        [SerializeField] private Queue<Node> waypoints;
        private Queue<Node> path;
        Node nextPosition;
        public float moveSpeed;
        //first time trying to use event handler, if it works great, I'll repalce those in combat system with these
        public event EventHandler OnWaypointReached;
        public event EventHandler OnEndReached;

        //Test only, remove after Wave Spawner setup
        private void Start()
        {
            Initialize(pathData);
        }

        public void Initialize(PathData pathData)
        {
            this.waypoints = new Queue<Node>(pathData.waypoints);
            GeneratePathTowardNextWaypoint();
            nextPosition = path.Dequeue();
        }
        private float wait = 0;
        public void Move()
        {
            if (wait > 0)
            {
                Debug.Log(wait);
                wait -= GameManager.Instance.gameDeltaTime;
                return;
            }
            //check if arrived at nextPosition
            if (transform.position == nextPosition.WorldPosition)
            {
                wait = nextPosition.waitTime;
                if (path.Count == 0)
                {
                    //check if it reached the end
                    if (waypoints.Count == 0)
                    {
                        EndReached();
                        return;
                    }
                    else
                    {
                        WaypointReached();
                        GeneratePathTowardNextWaypoint();
                    }
                }
                else
                {
                    nextPosition = path.Dequeue();
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, nextPosition.WorldPosition, moveSpeed * GameManager.Instance.gameDeltaTime);
        }

        private void WaypointReached()
        {
            Debug.Log($"Waypoint reached, {waypoints.Count} to go.");
            OnWaypointReached?.Invoke(this, EventArgs.Empty);
        }

        private void EndReached()
        {
            Debug.Log("End");
            OnEndReached?.Invoke(this, EventArgs.Empty);
        }
        public void GeneratePathTowardNextWaypoint()
        {
            //A* Pathfinding here we gooooooooooooo
            if (MapManager.Instance.map == null || MapManager.Instance.map.Count == 0) return;
            bool pathFound = false;
            //some data structure to store the pathing data(guess thier meaning by name)
            //pathfinding requires actuall map data, so the process is based on MapTile, but in the end
            //saved as a queue of Node for MovementSystem to work on.
            SimplePriorityQueue<MapTile, int> frontier = null;
            Dictionary<MapTile, MapTile> from = null;
            Dictionary<MapTile, int> costSoFar = null;
            MapTile current = null;
            Node nextWaypoint = null;
            //Try to find a path toward the nextWaypoint, try next-nextWaypoint if faild, unitl there's no waypoints left.
            while (!pathFound && waypoints.Count > 0)
            {
                nextWaypoint = waypoints.Dequeue();
                //destination to path
                //some data structure to store the pathing data(guess thier meaning by name)
                frontier = new SimplePriorityQueue<MapTile, int>();
                from = new Dictionary<MapTile, MapTile>();
                costSoFar = new Dictionary<MapTile, int>();
                //get current map position as starting point
                frontier.Enqueue(MapManager.Instance.map[MapManager.Instance.GetMapPositionFromWorldPosition(transform.position)], 0);
                from.Add(frontier.First, null);
                costSoFar.Add(frontier.First, 0);

                //the current position that's being test
                while (!(frontier.Count == 0))
                {
                    current = frontier.Dequeue();
                    Debug.Log($"Pathing {gameObject.name} is testing {current.MapPosition}");
                    if (current.MapPosition == nextWaypoint.mapPosition)
                    {
                        pathFound = true;
                        break;
                    }
                    foreach (var next in MapManager.Instance.GetNeighborsByMapPosition(current.MapPosition))
                    {
                        if (next.isObstacle) continue;//TODO: make mask for obstacle detection for each kind of units
                        int newCost = costSoFar[current] + 1;
                        if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                        {
                            costSoFar[next] = newCost;
                            frontier.Enqueue(next, newCost + MapManager.Instance.GetDistanceByMapPosition(nextWaypoint.mapPosition, next.MapPosition));
                            if (from.ContainsKey(next))
                                from[next] = current;
                            else
                                from.Add(next, current);
                        }
                    }
                }
            }

            if (!pathFound)
            {
                Debug.LogError($"{gameObject.name} can't find any path :( Check if its PathData was setup correctly.");
                path = null;
                return;
            }
            Debug.Log($"{gameObject.name} path found!");
            List<Node> revrsePath = new List<Node>();
            revrsePath.Add(nextWaypoint);
            while (from.ContainsKey(current) && from[current] != null)
            {
                revrsePath.Add(new Node(from[current].MapPosition));
                current = from[current];
            }
            revrsePath.Reverse();
            path = new Queue<Node>(revrsePath);
        }
    }

}