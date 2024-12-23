using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TS.Player {

    public class RocklPusher : MonoBehaviour {
        public Collider2D pushCollider;
        public ContactFilter2D contactFilter;
        public float force = 100;
        private static readonly List<Collider2D> _cache = new List<Collider2D>();

        private void FixedUpdate() {
            var count = pushCollider.OverlapCollider(contactFilter, _cache);
            for (int i = 0; i < count; ++i) {
                if (_cache[i].gameObject.CompareTag("Rock")) {
                    _cache[i].attachedRigidbody.AddForce(force * pushCollider.transform.right);
                    break;
                }
            }
        }
    }
}