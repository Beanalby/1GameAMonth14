using UnityEngine;
using System.Collections;

namespace onegam_1411 {
    public class Player : MonoBehaviour {

        private float moveSpeed = 5f;

        private CharacterController2D cc;

        public bool CanControl = false;

        public void Start() {
            cc = GetComponent<CharacterController2D>();
        }

        public void Update() {
            if(CanControl) {
                cc.velocity = new Vector3(moveSpeed * Input.GetAxis("Horizontal"), 0, 0);
            }
        }
        public void FixedUpdate() {
            if(CanControl) {
                cc.move(cc.velocity * Time.deltaTime);
            }
        }
    }
}