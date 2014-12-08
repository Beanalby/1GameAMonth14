using UnityEngine;
using System.Collections;

namespace onegam_1412 {
    public class PukeCollider : MonoBehaviour {
        private Puker puker;
        public void Start() {
            puker = transform.parent.GetComponent<Puker>();
        }

        public void OnTriggerEnter2D(Collider2D other) {
            SendMessageUpwards("PukeHit", other);
        }
    }
}