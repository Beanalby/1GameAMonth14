using UnityEngine;
using System.Collections;

namespace onegam_1408 {
    public class DeathBox : MonoBehaviour {
        private int layerPlayer;
        public void Start() {
            layerPlayer = LayerMask.NameToLayer("Player");
        }
        public void OnTriggerEnter2D(Collider2D other) {
            if(other.gameObject.layer == layerPlayer) {
                Player.Instance.Die();
            }
        }
    }
}