using UnityEngine;
namespace CombatSystem
{
    public class TestSkill : Skill
    {
        public TestSkill() 
        : base("permanent_alt_attack", "Fire form", "Your attack is imbued with fire.", new string[]{}, 0, 0, 20)
        {
        }

        public override void Use()
        {
            throw new System.NotImplementedException();
        }
    }


}