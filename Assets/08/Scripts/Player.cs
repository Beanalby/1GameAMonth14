using UnityEngine;
using System.Collections;

namespace onegam_1408 {
    [RequireComponent(typeof(CharacterController2D))]
    public class Player : MonoBehaviour {
        private static Player _instance = null;
        public static Player Instance { get { return _instance; } }

        public ParticleSystem healthParticles;
        public Transform playerMesh;

        private float maxHealth = 40;
        private float jumpHeight = 3f;
        private float runSpeed = 4f;
        private float groundDamping = 20f; // how fast do we change direction? higher means faster
        private float inAirDamping = 5f;
        private float healRate = 10f;
        private float gravity; // drived from rigidbody2d

        private float health;
        private bool didJump = false;
        private StageCamera cam;

        private bool isHealing = false;
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
            cam = GameObject.FindObjectOfType<StageCamera>();
            transform.position = GameDriver.Instance.SpawnPoint.transform.position + new Vector3(0, 2, 0);
        }

        public void Update() {
            if(CanControl && Input.GetButtonDown("Jump") && cc.isGrounded) {
                didJump = true;
            }
            UpdateHaze();
            UpdateHealth();
        }
        public void FixedUpdate() {
            UpdateMovement();
        }

        private void UpdateHaze() {
            cam.SetHaze((maxHealth - health) / maxHealth);
        }

        private void UpdateHealth() {
            if(isHealing) {
                health = Mathf.Min(maxHealth, health + (healRate * Time.deltaTime));
                if(health >= maxHealth) {
                    healthParticles.Stop();
                }
            }
        }
        private void UpdateMovement() {
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
            if (cc.isGrounded && didJump) {
                velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
                //AudioSource.PlayClipAtPoint(JumpSound, transform.position);
                didJump = false;
                SendMessage("PlayerJumped");
            }

            velocity.y += gravity * Time.fixedDeltaTime;
            cc.move(velocity * Time.deltaTime);
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
            GameDriver.Instance.PlayerDied();
            SendMessage("PlayerDied");
        }
        public void DisableControl() {
            canControl = false;
        }
        public void OnTriggerEnter2D(Collider2D other) {
            if(other.CompareTag("Respawn")) {
                GameDriver.Instance.SetSpawnPoint(other.gameObject);
            }
        }
        // OnTriggerExit2D not working 
        // http://answers.unity3d.com/questions/575438/how-to-make-ontriggerenter2d-work.html
        // let portal tell us when we enter exit instead
        public void PortalEntered() {
            isHealing = true;
            if(health < maxHealth) {
                healthParticles.Play();
            }
       }
        public void PortalExited() {
            isHealing = false;
            healthParticles.Stop();
        }
    }
}