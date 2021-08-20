using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnitAISystem
{
    public class KiledState : State
    {
        public KiledState(UnitAI unitAI) : base(unitAI)
        {
        }

        public override void Start()
        {
            
            Debug.Log($"{unitAI.gameObject.name}: AI awoke.");
        }

    }
}