using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnitAISystem
{
    public class StartState : State
    {
        public StartState(UnitAI unitAI) : base(unitAI)
        {
        }

        public override void Start()
        {

            Debug.Log($"{unitAI.gameObject.name}: AI awoke.");
            unitAI.SetState(unitAI.moveState);

        }

    }
}