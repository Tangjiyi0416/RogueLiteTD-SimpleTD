using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MovementSystem
{

    public class MovementManager : MonoBehaviour
    {
        [SerializeField]
        private List<Node> path;
        bool movementPaused;
        public float moveSpeed;
        public void Initialize(List<Node> path)
        {
            this.path = path;
        }
        int progress = 0;
        private void Start() {
            path = GameObject.Find("Path").GetComponent<PathManager>().GeneratePathFromWaypoints();
        }
        // Update is called once per frame
        void Update()
        {
            if (!movementPaused)
            {
                if (transform.position == path[progress].worldPos) progress++;
                if (progress == path.Count)
                {
                    movementPaused = true;
                    EndReached();
                    return;
                }
                transform.position = Vector3.MoveTowards(transform.position, path[progress].worldPos, moveSpeed * GameManager.instance.gameDeltaTime);
                //check if arrive at target path node
                //if do:
                //set target to next path node if there's still nodes left
                //else:
                //move in direction to the target path node by the moveSpeed
            }
        }

        void EndReached()
        {
            Debug.Log("End");
        }
    }

}