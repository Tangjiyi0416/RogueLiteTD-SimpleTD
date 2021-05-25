using UnityEngine;
namespace CombatSystem
{
    public class TestSkill : Skill
    {
        new public const string DISPLAY_NAME ="Test Skill 1";
        new public static readonly string[] TAGS ={"Test only","123 123"};

        public override void Use()
        {
            throw new System.NotImplementedException();
        }
    }


}