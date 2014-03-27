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

    private bool didJump = false, didFire = false;

    public void Start() {
        gravity = rigidbody2D.gravityScale * -9.8f;
        swing = GetComponent<SwayPlayerSwing>();
        cc = GetComponent<CharacterController2D>();
    }

    public void Update() {
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
                swing.ActivateSwing();
            }
        } else {
            if (didFire) {
                swing.ActivateSwing();
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

            if ( (hSpeed > 0 && transform.localScale.x < 0f)
                    || (hSpeed < 0 && transform.localScale.x > 0f) ) {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
		    float smoothedMovementFactor = cc.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		    velocity.x = Mathf.Lerp( velocity.x, hSpeed * runSpeed, Time.fixedDeltaTime * smoothedMovementFactor );

            if (cc.isGrounded && didJump) {
                velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            }

		    // apply gravity
		    velocity.y += gravity * Time.fixedDeltaTime;
            cc.move(velocity * Time.deltaTime);
        }
    }
}
