using UnityEngine;
using System.Collections;

namespace onegam_1409 {
    [RequireComponent(typeof(CharacterController2D))]
    public class Player : MonoBehaviour {
        private static Player _instance = null;
        public static Player Instance { get { return _instance; } }

        public Transform playerMesh;

        private float maxHealth = 100;
        private float jumpHeight = 3f;
        private float runSpeed = 8f;
        private float groundDamping = 20f; // how fast do we change direction? higher means faster
        private float inAirDamping = 5f;
        private float gravity; // drived from rigidbody2d

        private float health;
        private bool didJump = false;
        private bool useCC = true;

        private CharacterController2D cc;
        private bool isDead = false;
        private bool canControl = true;
        public bool CanControl { get { return canControl && !isDead; } }

        public void Awake() {
            if(_instance != null) {
                Debug.LogError("Can't have multiple players in a scene");
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        public void Start() {
            health = maxHealth;
            cc = GetComponent<CharacterController2D>();
            gravity = rigidbody2D.gravityScale * -9.8f;
            cc.velocity = new Vector3(0, 6.5f, 0);
        }

        public void Update() {
            if(CanControl && Input.GetButtonDown("Jump") && cc.isGrounded) {
                didJump = true;
            }
        }

        public void FixedUpdate() {
            UpdateMovement();
        }

        private void UpdateMovement() {
            if(useCC) {
                Vector3 velocity = cc.velocity;
                float hSpeed = 0;
                if(CanControl) {
                    hSpeed = Input.GetAxis("Horizontal");
                }
                if((hSpeed > 0 && playerMesh.localScale.x < 0f) || (hSpeed < 0 && playerMesh.localScale.x > 0f)) {
                    playerMesh.localScale = new Vector3(-playerMesh.localScale.x, playerMesh.localScale.y, playerMesh.localScale.z);
                }
                float smoothedMovementFactor = cc.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
                velocity.x = Mathf.Lerp(velocity.x, hSpeed * runSpeed, Time.fixedDeltaTime * smoothedMovementFactor);
                if(cc.isGrounded && didJump) {
                    velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
                    //AudioSource.PlayClipAtPoint(JumpSound, transform.position);
                    didJump = false;
                    SendMessage("PlayerJumped");
                }

                velocity.y += gravity * Time.fixedDeltaTime;
                cc.move(velocity * Time.deltaTime);
            }
        }

        public void GotHit(int damage) {
            if(isDead) {
                return;
            }
            health = Mathf.Max(0, health - damage);
            SendMessage("PlayerHit");
            if(health <= 0) {
                Die();
            }
        }
        public void Die() {
            health = 0;
            isDead = true;
            //GameDriver.Instance.PlayerDied();
            SendMessage("PlayerDied");
        }
        public void DisableControl() {
            canControl = false;
        }

        public void EdgeGrabbed(Vector3 edgePosition) {
            StartCoroutine(_edgeGrabbed(edgePosition));
        }

        private IEnumerator _edgeGrabbed(Vector3 edgePosition) {
            /// if our vertical velocity is over 6.5, we'll make it
            /// over the ledge on our own
            if(cc.velocity.y < 6.5f) {
                cc.velocity = Vector3.zero;
                transform.position = edgePosition;
                useCC = false;
                SendMessage("PlayerClimb");
                yield return new WaitForSeconds(1.05f);
                useCC = true;
            }
        }
    }
}