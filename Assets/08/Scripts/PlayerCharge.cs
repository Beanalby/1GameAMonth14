using UnityEngine;
using System.Collections;

namespace onegam_1408 {
    [RequireComponent(typeof(Player))]
    public class PlayerCharge : MonoBehaviour {

        private CharacterController2D cc;
        private Chargable chargeTarget;
        Player player;
        
        public void Start() {
            cc = GetComponent<CharacterController2D>();
            player = GetComponent<Player>();
        }

        public void Update() {
            UpdateCharge();
        }

        private void UpdateCharge() {
            if(chargeTarget == null && player.CanControl) {
                if(Input.GetAxisRaw("Vertical") < -.8f && cc.isGrounded) {
                    TryCharge();
                }
            } else {
                if(chargeTarget != null && (chargeTarget.IsFullyCharged || Input.GetAxisRaw("Vertical") > -.8f)) {
                    StopCharge();
                }
            }
        }

        private void TryCharge() {
            // see if there's a charge nearby
            foreach(Collider2D obj in Physics2D.OverlapCircleAll(transform.position, .1f)) {
                if(obj.CompareTag("Chargable")) {
                    StartCharge(obj.gameObject);
                    return;
                }
            }
        }

        private void StartCharge(GameObject obj) {
            chargeTarget = obj.GetComponent<Chargable>();
            if(!chargeTarget.CanStartCharging) {
                chargeTarget = null;
                return;
            }
            chargeTarget.StartCharge();
        }
        private void StopCharge() {
            if(chargeTarget != null) {
                chargeTarget.StopCharge();
            }
            chargeTarget = null;
        }

        public void PlayerDied() {
            StopCharge();
        }
    }
}