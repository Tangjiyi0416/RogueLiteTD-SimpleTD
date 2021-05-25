using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hex;
namespace CombatSystem
{
    public class TestTargetingMethod : TargetingMethod
    {
        HexMapSystem hexMapSystem;

        private List<CombatManager> targets;
        public override void Init()
        {
            hexMapSystem = HexMapSystem.Instance;
            targets = new List<CombatManager>();
            
        }
        public override List<CombatManager> GetTargets(int targetNumber)
        {
            if (targets.Count < targetNumber)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Input.GetMouseButton(0) && Physics.Raycast(ray, out hit))
                {
                    targets.Add(hit.transform.GetComponent<CreepCombatManager>());
                }
                
            }
            return targets;
        }
    }
}
