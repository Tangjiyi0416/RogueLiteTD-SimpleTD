using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CombatSystem
{
    public abstract class CombatManager : MonoBehaviour, ICanRecieveDamage, ICanRecieveEffect
    {
        public int life;
        public int defence;
        public Phases baseDamage;
        public Phases modifiedDamage;
        public Phases Resistence;
        public List<CombatModifier> combatModifiers;
        public Skill currentSkill;
        public abstract void UpdateModifiedDamage();
        public abstract void Attack();
        public abstract void UseSkill();//strategy pattern


        public abstract void RecieveDamage();
        public abstract void RecieveEffect(StatusEffect effect);
    }
    //五行
    public class Phases
    {
        public int[] phases = new int[5];

    }
    public enum PhaseType
    {
        Metal, Wood, Water, Fire, Earth
    }

    public abstract class Skill
    {
        public const string DISPLAY_NAME="UNNAMED";
        public static readonly string[] TAGS = {};
        public const int CAST_TIME = 0;//in frames
        public const int PRIMARY_DURATION = 0;//in frames
        public const int COOLDOWN = 0;//in frames

        public abstract void Use();
    }
    public abstract class CombatModifier
    {
        public const string DISPLAY_NAME = "UNNAMED";
        public static readonly string[] TAGS = {};
        //public targeting method (strategy pattern)


        public abstract void Modify(CombatManager host);

    }

    //CombatEffect is the base class for any combat-related Effects you want to implement
    public abstract class StatusEffect
    {
        public const string DISPLAY_NAME="UNNAMED";
        public const int DEFAULT_DURATION=0;

        public int duration;
        public int durationTimer;

        private CombatManager owner;
        private CombatManager target;

        ///<param name = "duration">in frames</param>
        ///<summary>Sets owner, target, and the duration of this effect, the effect will expire when the effectDurationTimer goes zero.</summary>
        public virtual void Init(CombatManager owner, CombatManager target, int duration)
        {
            this.owner = owner;
            this.target = target;
            this.durationTimer = this.duration = duration;

        }
        public abstract void Effect();
    }
    //Anything implemented this interface can be damaged. How they handled the RecieveDamage() method is completely up to you.
    public interface ICanRecieveDamage
    {
        public void RecieveDamage();
    }
    //Anything implemented this interface can get combat effects from any sources. How they handled the RecieveEffect() method is completely up to you.
    public interface ICanRecieveEffect
    {
        public void RecieveEffect(StatusEffect effecct);
    }

}