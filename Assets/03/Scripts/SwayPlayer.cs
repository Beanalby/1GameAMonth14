using UnityEngine;
using System.Collections;

public class SwayPlayer : MonoBehaviour {

    private float moveSpeed = 10f;

    private const float groundDist = .02f;
    private int groundMask;
    private BoxCollider2D box;
    private SwayPlayerSwing swing;
    private float groundOffV=0, groundOffH=0;

    private bool isGrounded {
        get {
            Vector2 basePos = new Vector2(transform.position.x,
                transform.position.y) + box.center;
            Vector2 posR = basePos + new Vector2(groundOffH, -groundOffV);
            Vector2 posL = basePos + new Vector2(-groundOffH, -groundOffV);
            return Physics2D.Raycast(posR, -Vector2.up, groundDist, groundMask)
                || Physics2D.Raycast(posL, -Vector2.up, groundDist, groundMask);
        }
    }
    
    public void Start() {
        groundMask = 1 << LayerMask.NameToLayer("Ground");
        swing = GetComponent<SwayPlayerSwing>();
        box = GetComponent<BoxCollider2D>();
        groundOffV = box.size.y / 2 + groundDist / 2;
        groundOffH = box.size.x / 2;
    }

    public void Update() {
        rigidbody2D.gravityScale = isGrounded ? 0 : 1;
        HandleGrapple();
        HandleMovement();
        Debug.Log("Grounded=" + isGrounded);
    }

    private void HandleGrapple() {
        if (Input.GetButtonDown("Jump")) {
            swing.ActivateSwing();
        }
    }

    private void HandleMovement() {
        if (swing.IsSwinging) {
            // swinging handles input
            swing.UpdateSwing();
            return;
        }

        float force = Input.GetAxis("Horizontal") * moveSpeed;
        
        rigidbody2D.velocity = new Vector2(force, rigidbody2D.velocity.y);
    }

    //public void OnDrawGizmos() {
    //    if (groundOffV == 0) {
    //        return;
    //    }
    //    Gizmos.color = Color.cyan;
    //    Vector3 posR = transform.position
    //        + new Vector3(groundOffH, -groundOffV);
    //    Vector3 posL = transform.position
    //        + new Vector3(-groundOffH, -groundOffV);
    //    Gizmos.DrawLine(posR, posR + new Vector3(0, -groundDist, 0));
    //    Gizmos.DrawLine(posL, posL + new Vector3(0, -groundDist, 0));
    //}
}
