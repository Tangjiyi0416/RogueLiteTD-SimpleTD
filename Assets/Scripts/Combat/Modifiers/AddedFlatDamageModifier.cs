using UnityEngine;

namespace CombatSystem
{
    public class AddedFlatDamageModifier : Modifier
    {
        private Phases addedBaseDamage = new Phases(0, 100, 111, 456, 45);

        //Call when added to a CombatManager
        public override void Modify(CombatManager owner)
        {
            owner.baseDamage += addedBaseDamage;
        }
        //Call when removed from a CombatManager
        public override void UnModify(CombatManager owner)
        {
            owner.baseDamage += addedBaseDamage;

        }
    }


}