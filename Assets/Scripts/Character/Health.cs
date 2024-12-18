using System;
using UnityEngine;
using UnityEngine.Events;

namespace TS.Character {

    public class Health : MonoBehaviour {
        public byte hp;
        public UnityEvent onDied;
        [NonSerialized] public bool isDead;

        protected void Update() {
            if (isDead) return;
            if (hp is 0) {
                onDied?.Invoke();
                isDead = true;
            }
        }
    }
}