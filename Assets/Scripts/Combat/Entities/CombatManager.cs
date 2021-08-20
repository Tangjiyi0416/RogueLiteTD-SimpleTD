using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CombatSystem
{
    #region Delegates
    public delegate void OnHitEvent(Hit hit);
    public delegate void WhenHitEvent(Hit hit);
    public delegate void UpdateStatusEffectEvent();
    public delegate void UpdateEvent();
    #endregion
    //Every tower and creep have one and only one CombatManager, it handles all combat related stuff except moving.
    //CombatManager fuctions as a container with some extra ability, so it doesn't "generate" stats like life, defence, baseDamage, etc. on its own.
    //When a tower/creep spawns, all those data needs to be assgined to the corresponding fields in CombatManager(Maybe a default modifier?).
    public abstract class CombatManager : MonoBehaviour//Don't use Unity Callback functions in this class, use them in its children.
    {
        public CombatData combatData;
        protected List<Modifier> modifiers = new List<Modifier>();
        protected List<StatusEffect> statusEffects = new List<StatusEffect>();
        protected List<Skill> skills = new List<Skill>();
        protected Attack currentAttack;

        public event OnHitEvent onHitEvent;
        public event WhenHitEvent whenHitEvent;
        public event UpdateStatusEffectEvent updateStatusEffectEvent;
        public event UpdateEvent updateEvent;
        public virtual void Initialize(CombatData combatData, List<Modifier> modifiers, List<Skill> skills)
        {
            this.combatData = combatData;
            foreach (var m in modifiers) ReceiveModifier(m);
            foreach (var s in skills) ReceiveSkill(s);

        }
        protected virtual void Update()
        {
            TriggerUpdateStatusEffectEvent();
            updateEvent?.Invoke();
        }
        #region Hit
        public void TriggerOnHitEvent(Hit hit)
        {
            onHitEvent?.Invoke(hit);
        }

        public void TriggerWhenHitEvent(Hit hit)
        {
            whenHitEvent?.Invoke(hit);
        }
        public virtual void ReceiveHit(Hit hit)
        {
            hit.origin.TriggerOnHitEvent(hit);
            int damage = 0;
            for (int i = 0; i < DamageType.damageTypeCount; i++)
            {
                damage += (int)(hit.damage[i].FinalValue * (1f + combatData.Resistence[i] / 100f));
            }
            combatData.life -= damage;
            TriggerWhenHitEvent(hit);
            Debug.Log($"{gameObject.name} received {damage} point damage from {hit.origin.name}");
            if (IsDead()) Die();
        }
        #endregion
        #region Testing stuff
        public virtual void ReceiveHeal(int number)
        {
            combatData.life += number;
            combatData.life = combatData.life > combatData.maxLife.FinalValue ? combatData.maxLife.FinalValue : combatData.life;
        }
        public virtual void LifeGain()
        {
            ReceiveModifier(new ExampleLifeGainOnHitModifier());
        }
        public virtual void LifeLose()
        {
            RemoveModifier("example_life_gain_on_hit_modifier");
        }

        #endregion
        #region Status Effect
        public virtual void ReceiveStatusEffect(StatusEffect effect)
        {
            foreach (StatusEffect e in statusEffects)
            {
                if (e.NAME == effect.NAME)
                {
                    e.durationTimer = e.duration;
                    return;
                }
            }
            effect.OnAdded();
            statusEffects.Add(effect);
        }
        protected virtual void TriggerUpdateStatusEffectEvent()
        {
            updateStatusEffectEvent?.Invoke();
        }
        public virtual void RemoveStatusEffect(string effectName)
        {
            foreach (StatusEffect e in statusEffects)
            {
                if (e.NAME == effectName)
                {
                    e.OnRemoved();
                    statusEffects.Remove(e);
                    return;
                }
            }
        }
        #endregion
        #region Modifier
        public virtual void ReceiveModifier(Modifier modifier)
        {

            modifier.OnAdded(this);
            modifiers.Add(modifier);
            Debug.Log($"{gameObject.name} get {modifier.DISPLAY_NAME}");

        }
        public virtual void RemoveModifier(string modifierName)
        {
            foreach (Modifier m in modifiers)
            {
                if (m.NAME == modifierName)
                {
                    m.OnRemoved(this);
                    modifiers.Remove(m);
                    Debug.Log($"{gameObject.name} lose {m.DISPLAY_NAME}");
                    return;
                }
            }

        }
        public virtual void RemoveModifier(Modifier modifier)
        {
            modifier.OnRemoved(this);
            modifiers.Remove(modifier);
            Debug.Log($"{gameObject.name} lose {modifier.DISPLAY_NAME}");
        }
        #endregion
        #region Skill
        public virtual void UseSkill(int skillIndex)
        {
            skills[skillIndex].Use();
        }
        public virtual void ReceiveSkill(Skill skill)
        {
            skill.Initialize();
            skills.Add(skill);
            Debug.Log($"{gameObject.name} get {skill.DisplayName}");

        }

        public virtual void RemoveSkill(int skillIndex)
        {
            Debug.Log($"{gameObject.name} lose {skills[skillIndex].DisplayName}");
            skills.RemoveAt(skillIndex);

        }
        #endregion
        #region Attack
        public virtual void ReceiveAttack(Attack attack)
        {
            attack.OnAdded();
            currentAttack = attack;
            Debug.Log($"{gameObject.name} get {attack.DisplayName}");

        }
        #endregion
        public bool IsDead()
        {
            return combatData.life <= 0 ? true : false;
        }

        protected virtual void Die()
        {

            GameObject.Destroy(gameObject);
        }

        #region Utility Fuctions
        #endregion
    }

    [Serializable]
    public class ScalableStat
    {
        [SerializeField]
        private int baseValue;
        [SerializeField]
        private float increment;
        [SerializeField]
        private float multiplier;
        public int FinalValue { get => (int)(baseValue * (1f + increment) * (1f + multiplier)); }

        /// <param name="baseValue"></param>
        /// <param name="increment">This paramater will be added by 1 in Finalvalue, so you only need to pass the float part of target value.</param>
        /// <param name="multiplier">This paramater will be added by 1 in Finalvalue, so you only need to pass the float part of target value.</param>
        public ScalableStat(int baseValue = 0, float increment = 0f, float multiplier = 0f)
        {
            this.baseValue = baseValue;
            this.increment = increment;
            this.multiplier = multiplier;
        }

        public ScalableStat(ScalableStat stat)
        {
            baseValue = stat.baseValue;
            increment = stat.increment;
            multiplier = stat.multiplier;
        }

        /// <summary>
        /// baseValue will be added by num
        /// </summary>
        /// <param name="num"></param>
        public void ScaleBaseValue(int num)
        {
            baseValue += num;
        }
        /// <summary>
        /// increment will be added by num
        /// </summary>
        /// <param name="num"></param>
        public void ScaleIncrement(float num)
        {
            increment += num;
        }
        /// <summary>
        /// multiplier will be multiplied by num
        /// </summary>
        /// <param name="num"></param>
        public void ScaleMultiplier(float num)
        {
            multiplier *= 1f + num;
        }
    }
    /* Abandoned damage calculating class
    [Serializable]
    public class Phases //五行
    {
        [SerializeField]
        private int[] arr;
        public int Metal { get => arr[0]; set => arr[0] = value; }
        public int Wood { get => arr[1]; set => arr[1] = value; }
        public int Water { get => arr[2]; set => arr[2] = value; }
        public int Fire { get => arr[3]; set => arr[3] = value; }
        public int Earth { get => arr[4]; set => arr[4] = value; }
        public int Total { get => arr.Sum(); }
        public Phases(int n)
        {
            arr = new int[] { n, n, n, n, n };
        }
        public Phases(int[] phases)
        {
            if (phases.Length != 5) throw new ArgumentOutOfRangeException("Can only take int[5]");
            this.arr = phases;
        }
        public Phases(int metal, int wood, int water, int fire, int earth)
        {
            arr = new int[] { metal, wood, water, fire, earth };
        }

        public static Phases Clamp(Phases phases, int min, int max)
        {
            int[] arr = new int[5];
            for (int i = 0; i < 5; i++)
            {
                if (phases.arr[i] > max) arr[i] = max;
                else if (phases.arr[i] < min) arr[i] = min;
                else arr[i] = phases.arr[i];
            }
            return new Phases(arr);
        }

        public static Phases operator +(Phases a) => a;
        public static Phases operator -(Phases a) => new Phases(new int[] { -a.arr[0], -a.arr[1], -a.arr[2], -a.arr[3], -a.arr[4] });
        public static Phases operator +(Phases a, Phases b)
            => new Phases(new int[] { a.arr[0] + b.arr[0], a.arr[1] + b.arr[1], a.arr[2] + b.arr[2], a.arr[3] + b.arr[3], a.arr[4] + b.arr[4] });
        public static Phases operator -(Phases a, Phases b)
            => new Phases(new int[] { a.arr[0] - b.arr[0], a.arr[1] - b.arr[1], a.arr[2] - b.arr[2], a.arr[3] - b.arr[3], a.arr[4] - b.arr[4] });
        public static Phases operator *(Phases a, Phases b)
            => new Phases(new int[] { a.arr[0] * b.arr[0], a.arr[1] * b.arr[1], a.arr[2] * b.arr[2], a.arr[3] * b.arr[3], a.arr[4] * b.arr[4] });
        public static Phases operator /(Phases a, Phases b)
        {
            if (b.arr[0] == 0 || b.arr[1] == 0 || b.arr[2] == 0 || b.arr[3] == 0 || b.arr[4] == 0) throw new DivideByZeroException();

            return new Phases(new int[] { a.arr[0] / b.arr[0], a.arr[1] / b.arr[1], a.arr[2] / b.arr[2], a.arr[3] / b.arr[3], a.arr[4] / b.arr[4] });
        }
    }
    */
    public class Hit
    {
        public ScalableStat[] damage;
        public CombatManager origin;
        public CombatManager target;

        public Hit(ScalableStat[] damage, CombatManager origin, CombatManager target)
        {
            this.damage = new ScalableStat[DamageType.damageTypeCount];
            for (int i = 0; i < DamageType.damageTypeCount; i++)
            {
                this.damage[i] = new ScalableStat(damage[i]);

            }
            this.origin = origin;
            this.target = target;
        }
    }

    [Serializable]
    public class CombatData
    {
        public int life;
        public ScalableStat maxLife;
        public ScalableStat defence;
        public ScalableStat attackSpeed;//in seconds
        public ScalableStat skillCooldownSpeed;//in seconds
        public ScalableStat[] damage;//use 6 because there is 5 phases and 1 true damage
        public ScalableStat[] rawResistence;

        public int[] Resistence
        {
            get
            {
                int[] res = new int[DamageType.damageTypeCount];
                for (int i = 0; i < DamageType.damageTypeCount; i++)
                {
                    res[i] = Mathf.Clamp(rawResistence[i].FinalValue, -75, 75);
                }
                return res;
            }
        }

        public CombatData()
        {
            maxLife = new ScalableStat(50);
            life = maxLife.FinalValue;
            defence = new ScalableStat();
            attackSpeed = new ScalableStat(1);
            skillCooldownSpeed = new ScalableStat(1);
            damage = new ScalableStat[DamageType.damageTypeCount];
            rawResistence = new ScalableStat[DamageType.damageTypeCount];
        }
        public CombatData(int life, ScalableStat maxLife, ScalableStat defence, ScalableStat attackSpeed, ScalableStat skillCooldownSpeed, ScalableStat[] damage, ScalableStat[] rawResistence)
        {
            this.life = life;
            this.maxLife = maxLife;
            this.defence = defence;
            this.attackSpeed = attackSpeed;
            this.skillCooldownSpeed = skillCooldownSpeed;
            this.damage = damage;
            this.rawResistence = rawResistence;
        }
    }
    public static class DamageType
    {
        public enum Types
        {
            True, Metal, Wood, Water, Fire, Earth
        }

        public static int damageTypeCount = Enum.GetNames(typeof(Types)).Length;
    }
}