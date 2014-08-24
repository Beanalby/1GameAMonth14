using UnityEngine;
using System.Collections;

namespace onegam_1408 {
    public class PlayerCharge : MonoBehaviour {

        private CharacterController2D cc;
        private GameObject chargeTarget;

        public void Start() {
            cc = GetComponent<CharacterController2D>();
        }

        public void Update() {
            if(chargeTarget == null && Input.GetAxisRaw("Vertical") < -.8f && cc.isGrounded) {
                Debug.Log("+++ Trying to charge!");
                TryCharge();
            } else {
                if(chargeTarget != null && Input.GetAxisRaw("Vertical") > -.8f) {
                    StopCharge();
                }
            }
        }

        private void TryCharge() {
            // see if there's a charge nearby
            foreach(Collider2D obj in Physics2D.OverlapCircleAll(transform.position, .1f)) {
                Debug.Log("+++ investigating " + obj.name);
                if(obj.CompareTag("Chargable")) {
                    StartCharge(obj.gameObject);
                }
            }
        }
        private void StartCharge(GameObject obj) {
            chargeTarget = obj;
            chargeTarget.SendMessage("StartCharge");
        }
        private void StopCharge() {
            chargeTarget.SendMessage("StopCharge");
            chargeTarget = null;
        }
    }
}