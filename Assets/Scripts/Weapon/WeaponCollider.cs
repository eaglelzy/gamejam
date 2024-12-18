using System.Collections.Generic;
using TS.Character;
using UnityEngine;

namespace TS.Weapon {

    public class WeaponCollider : MonoBehaviour {
        public Collider2D hitCollider;
        public ContactFilter2D contactFilter2D;
        private static readonly List<Collider2D> shared = new();

        public void OverlapTargets() {
            hitCollider.OverlapCollider(contactFilter2D, shared);
            foreach (var collider in shared) {
                if (collider.TryGetComponent<Health>(out var health)) {
                    health.Damage(1, gameObject);
                }
            }
        }
    }
}