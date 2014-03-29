using UnityEngine;
using System.Collections;

public class SwayPlayer : MonoBehaviour {

    // movement config
    private float gravity; // drived from rigidbody2d
    private float runSpeed = 4f;
    private float groundDamping = 20f; // how fast do we change direction? higher means faster
    private float inAirDamping = 5f;
    private float jumpHeight = 3f;

    private SwayPlayerSwing swing;

    private CharacterController2D cc;

    public int FacingDir {
        get { return (int)Mathf.Sign(transform.localScale.x);  }
    }
    /// <summary>
    /// When we let go of a rope, we maintain velocity until we hit something,
    /// unlike normally having horizontal velocity quickly drop to zero with
    /// neutral input.
    /// </summary>
    private bool isRopeFlying = false;

    private bool didJump = false, didFire = false;
    //private bool didTest1=false, didTest2=false;

    public void Start() {
        gravity = rigidbody2D.gravityScale * -9.8f;
        swing = GetComponent<SwayPlayerSwing>();
        cc = GetComponent<CharacterController2D>();
        cc.onControllerCollidedEvent += OnControllerCollided;
        cc.velocity = new Vector3(2, 0); isRopeFlying = true; // +++
    }

    public void Update() {
        //if(!didTest1) {
        //    didFire = true; didTest1 = true;
        //}
        //if(!didTest1 && transform.position.y < 5.2f) {
        //    didFire = true; didTest1 = true;
        //}
        //if(!didTest2 && transform.position.x > 3) {
        //    didFire = true; didTest2 = true;
        //}
        if (Input.GetButtonDown("Jump")) {
            didJump = true;
        }
        if (Input.GetButtonDown("Fire1")) {
            didFire = true;
        }
    }
    public void FixedUpdate() {
        HandleGrapple();
        HandleMovement();
        didJump = false;
        didFire = false;
    }

    private void HandleGrapple() {
        if (swing.IsSwinging) {
            // while swinging, either jumping or firing lets go
            if(didJump || didFire) {
                swing.EndSwing();
                isRopeFlying = true;
            }
        } else {
            if (didFire) {
                swing.StartSwing();
            }
        }
    }

    private void HandleMovement() {

        if (swing.IsSwinging) {
            // swinging handles input
            swing.UpdateSwing();
            return;
        } else {
            Vector3 velocity = cc.velocity;
            float hSpeed = Input.GetAxis("Horizontal");

            if (isRopeFlying) {
                // cancel rope flying if they input the opposite direction
                if(hSpeed != 0 && Mathf.Sign(hSpeed) != Mathf.Sign(cc.velocity.x)) {
                    isRopeFlying = false;
                }
            }
            if (!isRopeFlying) {
                if ((hSpeed > 0 && transform.localScale.x < 0f)
                        || (hSpeed < 0 && transform.localScale.x > 0f)) {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                float smoothedMovementFactor = cc.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
                velocity.x = Mathf.Lerp(velocity.x, hSpeed * runSpeed, Time.fixedDeltaTime * smoothedMovementFactor);
            }

            if (cc.isGrounded && didJump) {
                velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            }

            // apply gravity
            velocity.y += gravity * Time.fixedDeltaTime;
            cc.move(velocity * Time.deltaTime);
        }
    }

    public void OnControllerCollided(RaycastHit2D hit) {
        //Debug.Log(name + " hit " + hit.collider.name);
        isRopeFlying = false;
    }
}
