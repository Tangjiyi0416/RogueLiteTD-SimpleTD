using UnityEngine;

namespace EntitySystem
{
    public class ExampleAddedFlatDamageModifier : Modifier
    {
        private int[] addedBaseDamage = new int[] {0, 8, 100, 111, 456, 5 };

        public ExampleAddedFlatDamageModifier()
         : base("example_added_flat_damage_modifier", "Example Added Flat Damage Modifier", "NO DESCRIPTION", new string[] { })
        {
        }

        //Call when added to a CombatManager
        public override void OnAdded(Entity owner)
        {
            for (int i = 0; i < 5; i++)
            {
                owner.combatData.damage[i].ScaleBaseValue(addedBaseDamage[i]);
            }
        }
        //Call when removed from a CombatManager
        public override void OnRemoved(Entity owner)
        {
            for (int i = 0; i < 5; i++)
            {
                owner.combatData.damage[i].ScaleBaseValue(-addedBaseDamage[i]);
            }
        }
    }


}