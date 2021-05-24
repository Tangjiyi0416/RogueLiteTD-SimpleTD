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
        public Phases finalDamage;
        public Phases Resistence;
        public List<CombatModifier> combatModifiers;
        public abstract void Init();
        public abstract void UpdateFinalDamage();
        public abstract void Attack();
        public abstract void UseSkill();//strategy pattern


        public abstract void RecieveDamage();
        public abstract void RecieveEffect(CombatEffect effect);
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

    public abstract class CombatSkill{

    }
    public abstract class CombatModifier
    {
        public string modifierName;
        public List<string> tags;
        //public targeting method (strategy pattern)
        public int castTime;//in frames
        public int mainDuration;//in frames
        public int coolDown;//in frames

        public abstract void Modifier(CombatManager host);

    }

    //CombatEffect is the base class for any combat-related Effects you want to implement
    public abstract class CombatEffect
    {
        public string effectName;
        public int effectDuration;
        public int effectDurationTimer;

        public CombatManager owner;
        public CombatManager target;

        ///<param name = "duration">in frames</param>
        ///<summary>Sets owner, target, and the duration of this effect, the effect will expire when the effectDurationTimer goes zero.</summary>
        public virtual void Init(CombatManager owner, CombatManager target, int duration)
        {
            this.owner = owner;
            this.target = target;
            this.effectDurationTimer = this.effectDuration = duration;

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
        public void RecieveEffect(CombatEffect effecct);
    }
   
}