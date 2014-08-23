using UnityEngine;
using System.Collections;

namespace onegam_1408 {
    [RequireComponent(typeof(CharacterController2D))]
    public class Player : MonoBehaviour {

        private float jumpHeight = 3f;
        private float runSpeed = 4f;
        private float groundDamping = 20f; // how fast do we change direction? higher means faster
        private float inAirDamping = 5f;
        private float gravity; // drived from rigidbody2d

        private bool didJump = false;

        private CharacterController2D cc;

        public void Start() {
            cc = GetComponent<CharacterController2D>();
            gravity = rigidbody2D.gravityScale * -9.8f;
        }

        public void Update() {
            if(Input.GetButtonDown("Jump") && cc.isGrounded) {
                didJump = true;
            }
        }

        public void FixedUpdate() {
            UpdateMovement();
        }
        private void UpdateMovement() {
            Vector3 velocity = cc.velocity;
            float hSpeed = Input.GetAxis("Horizontal");
            if((hSpeed > 0 && transform.localScale.x < 0f) || (hSpeed < 0 && transform.localScale.x > 0f)) {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            float smoothedMovementFactor = cc.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
            velocity.x = Mathf.Lerp(velocity.x, hSpeed * runSpeed, Time.fixedDeltaTime * smoothedMovementFactor);

            if (cc.isGrounded && didJump) {
                velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
                //AudioSource.PlayClipAtPoint(JumpSound, transform.position);
                didJump = false;
            }

            // apply gravity
            velocity.y += gravity * Time.fixedDeltaTime;
            cc.move(velocity * Time.deltaTime);
        }

    }
}