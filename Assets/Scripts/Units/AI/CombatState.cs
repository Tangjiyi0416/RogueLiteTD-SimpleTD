using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatSystem;
namespace UnitAISystem
{
    public class CombatState : State
    {
        UnitCombatManager unitCombatManager;
        public CombatState(UnitAI unitAI) : base(unitAI)
        {
            unitCombatManager = unitAI.GetComponent<UnitCombatManager>();
        }

        public override void Start()
        {

            Debug.Log($"{unitAI.gameObject.name}: Start Combat.");

        }

    }
}