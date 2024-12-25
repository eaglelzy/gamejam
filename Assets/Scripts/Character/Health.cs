using System;
using UnityEngine;
using UnityEngine.Events;

namespace TS.Character
{
    /// <summary>
    /// 能被攻击的都继承生命组件
    /// </summary>
    public class Health : MonoBehaviour
    {
        //数据都用float，避免要做类型转换
        //public byte hp;

        [Tooltip("当前生命值")]
        public float CurrentHealth;

        [Tooltip("最大生命值")]
        public float MaximumHealth = 1;

        public UnityEvent OnDied;
        [NonSerialized] public bool IsDead;

        public UnityAction HealthChangedAction;


        protected virtual void Awake()
        {
            Initialization();
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void Initialization()
        {
            //开始的生命值为最大生命值
            SetHealth(MaximumHealth);
        }

        /// <summary>
        /// 收到伤害
        /// </summary>
        /// <param name="damage">伤害量</param>
        public virtual void Damage(float damage, GameObject attacker)
        {
            CurrentHealth -= damage;

            HealthChangedAction?.Invoke();

            if (CurrentHealth < 0)
            {
                OnDied?.Invoke();
            }
        }

        /// <summary>
        /// 改变生命值，封装成方法
        /// </summary>
        /// <param name="newValue">新的生命值</param>
        public virtual void SetHealth(float newValue)
        {
            CurrentHealth = newValue;     
            HealthChangedAction?.Invoke();
        }
    }
}