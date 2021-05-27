using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CombatSystem
{
    public abstract class CombatManager : MonoBehaviour, ICanRecieveHit, ICanRecieveEffect
    {
        public int life = 0;
        public int maxLife = 0;
        public int defence = 0;
        public Phases baseDamage = new Phases(0, 0, 0, 0, 0);
        public Phases totalDamageIncrement = new Phases(1);
        public Phases totalDamageMultiplier = new Phases(1);
        public Phases Resistence = new Phases(0);
        protected List<Modifier> modifiers;
        protected List<StatusEffect> statusEffects;
        public Skill currentSkill;
        public abstract void UpdateModifiedDamage();
        public abstract void Attack();
        public abstract void UseSkill();//strategy pattern

        public delegate void OnHitEffects(Hit hit);
        public event OnHitEffects onHitEffects;
        public delegate void WhenHitEffects(Hit hit);
        public event WhenHitEffects whenHitEffects;

        public void TriggerOnHitEffect(Hit hit)
        {
            onHitEffects?.Invoke(hit);
        }
        public void TriggerWhenHitEffects(Hit hit)
        {
            whenHitEffects?.Invoke(hit);
        }
        public virtual void AddModifier(Modifier modifier)
        {
            modifier.Modify(this);
        }
        public abstract void RecieveHit(Hit hit);
        public abstract void RecieveEffect(StatusEffect effect);
        public bool IsDead()
        {
            return life <= 0 ? true : false;
        }

        protected virtual void Dead()
        {

            GameObject.Destroy(gameObject);
        }


    }
    //五行
    public class Phases
    {
        int metal, wood, water, fire, earth;
        public int Total { get => metal + wood + water + fire + earth; }
        public Phases(int n)
        {
            this.metal = this.wood = this.water = this.fire = this.earth = n;
        }
        public Phases(int metal, int wood, int water, int fire, int earth)
        {
            this.metal = metal;
            this.wood = wood;
            this.water = water;
            this.fire = fire;
            this.earth = earth;
        }

        public static Phases operator +(Phases a) => a;
        public static Phases operator -(Phases a) => new Phases(-a.metal, -a.wood, -a.water, -a.fire, -a.earth);
        public static Phases operator +(Phases a, Phases b)
            => new Phases(a.metal + b.metal, a.wood + b.wood, a.water + b.water, a.fire + b.fire, a.earth + b.earth);
        public static Phases operator -(Phases a, Phases b)
            => a + (-b);
        public static Phases operator *(Phases a, Phases b)
            => new Phases(a.metal * b.metal, a.wood * b.wood, a.water * b.water, a.fire * b.fire, a.earth * b.earth);
        public static Phases operator /(Phases a, Phases b)
        {
            if (b.metal == 0 || b.wood == 0 || b.water == 0 || b.fire == 0 || b.earth == 0) throw new DivideByZeroException();

            return new Phases(a.metal / b.metal, a.wood / b.wood, a.water / b.water, a.fire / b.fire, a.earth / b.earth);
        }
    }
    public enum PhaseType
    {
        Metal, Wood, Water, Fire, Earth
    }

    public class Hit
    {
        public Phases baseDamage;
        public Phases totalDamageIncrease;
        public Phases totalDamageMore;
        public CombatManager origin;

        public Hit(Phases baseDamage, Phases totalDamageIncrease, Phases totalDamageMore, CombatManager owner)
        {
            this.baseDamage = baseDamage;
            this.totalDamageIncrease = totalDamageIncrease;
            this.totalDamageMore = totalDamageMore;
            this.origin = owner;
        }
    }

    public abstract class Skill
    {
        public readonly string DISPLAY_NAME = "UNNAMED";
        public static readonly string[] TAGS = { };
        public readonly int CAST_TIME = 0;//in frames
        public readonly int PRIMARY_DURATION = 0;//in frames
        public readonly int COOLDOWN = 0;//in frames

        public abstract void Use();
    }

    public abstract class Modifier
    {
        public readonly string DISPLAY_NAME = "UNNAMED";
        public static readonly string[] TAGS = { };
        ///<summary>Call when added to a CombatManager.</summary>
        public abstract void Modify(CombatManager owner);
        ///<summary>Call when removed from a CombatManager.</summary>
        public abstract void UnModify(CombatManager owner);
    }

    //CombatEffect is the base class for any combat-related Effects you want to implement
    public abstract class StatusEffect
    {
        public readonly string DISPLAY_NAME = "UNNAMED";
        public readonly int DEFAULT_DURATION = 0;

        public int duration;
        public int durationTimer;

        protected CombatManager origin;
        protected CombatManager target;

        ///<param name = "duration">in frames</param>
        ///<summary>Sets owner, target, and the duration of this effect, the effect will expire when the effectDurationTimer goes zero.</summary>
        public StatusEffect(CombatManager origin, CombatManager target, int duration)
        {
            this.origin = origin;
            this.target = target;
            this.durationTimer = this.duration = duration;

        }
        public abstract void Effect();
    }
    //Anything implemented this interface can be damaged. How they handled the RecieveDamage() method is completely up to you.
    public interface ICanRecieveHit
    {
        public void RecieveHit(Hit hit);
    }
    public interface ICanRecieveEffect
    {
        public void RecieveEffect(StatusEffect effect);
    }


}