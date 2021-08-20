using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MovementSystem;
namespace UnitAISystem
{
    public class MoveState : State
    {
        MovementManager movementManager;
        public MoveState(UnitAI unitAI) : base(unitAI)
        {
            movementManager = unitAI.GetComponent<MovementManager>();
        }

        public override void Start()
        {

            Debug.Log($"{unitAI.gameObject.name}: Start Moving.");
        }
        public override void Update()
        {
            
            movementManager.Move();
        }

    }
}