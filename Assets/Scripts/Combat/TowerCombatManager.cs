using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class TowerCombatManager : CombatManager
    {
        protected override void Dead()
        {
            Debug.Log("I'm a Tower and I'm dead.");
            base.Dead();
        }


    }
}
