using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class CreepCombatManager : CombatManager
    {
        public override void Attack()
        {
            throw new NotImplementedException();
        }
        public override void ReceiveHit(Hit hit)
        {
            base.ReceiveHit(hit);
            //Debug.Log($"{hit.origin} {hit.target}");
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


    }
}
