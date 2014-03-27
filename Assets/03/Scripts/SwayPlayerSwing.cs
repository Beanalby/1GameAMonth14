using UnityEngine;
using System.Collections;

public class SwayPlayerSwing : MonoBehaviour {

    public GameObject target;

    [HideInInspector]
    public bool IsSwinging {
        get { return joint.enabled; }
    }

    private float swingAccel = .5f;
    private float minAccel = .05f;
    private float maxVelocityBase = 5f;
    private float maxVelocityScale = 1f;

    private float minLen = 1.5f;
    private float maxLen = 5f;
    private float lenSpeed = 1f;
    private float lenThreshold = .5f;

    private BoxCollider2D box;
    private DistanceJoint2D joint;
    private CharacterController2D cc;

    public void Start() {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        cc = GetComponent<CharacterController2D>();
        box = GetComponent<BoxCollider2D>();
    }

    // not automatically called every frame, invoked by
    // SwayPlayer as needed
	public void UpdateSwing () {
        if(Input.GetKeyDown(KeyCode.X)) {
            transform.position = new Vector3(0, transform.position.y, 0);
            rigidbody2D.velocity = new Vector2(.1f, 0);
            joint.distance = 2f;
        }
        if(Input.GetKeyDown(KeyCode.C)) {
            joint.distance = 3f;
        }
        if(Input.GetKeyDown(KeyCode.V)) {
            joint.distance = 4f;
        }
        HandleSwing();
        HandleLengthen();
    }
    
    private void HandleLengthen() {
        Vector3 us = transform.position
            + new Vector3(joint.anchor.x, joint.anchor.y, 0);
        Vector3 jointBase = joint.connectedBody.transform.position
            + new Vector3(joint.connectedAnchor.x, joint.connectedAnchor.y, 0);
        // don't allow adjusting length if it's not taunt
        if (joint.distance * .99f >= (us - jointBase).magnitude) {
            return;
        }

        bool moved = false;
        if (Input.GetAxis("Vertical") > lenThreshold) {
            joint.distance = Mathf.Max(minLen, joint.distance - lenSpeed * Time.deltaTime);
            moved = true;
        } else if (Input.GetAxis("Vertical") < -lenThreshold) {
            joint.distance = Mathf.Min(maxLen, joint.distance + lenSpeed * Time.deltaTime);
            moved = true;
        }
        if (moved) {
            // automatically adjust our position for the new length
            Ray ray = new Ray(jointBase, us - jointBase);
            transform.position = ray.GetPoint(joint.distance);
        }
    }

    private void HandleSwing() {
        Vector3 src = transform.position
            + new Vector3(joint.anchor.x, joint.anchor.y, 0);
        Vector3 dest = joint.connectedBody.transform.position
            + new Vector3(joint.connectedAnchor.x, joint.connectedAnchor.y, 0);

        // Don't let input affect the rope's speed unless it's taunt
        if (joint.distance * .99f >= (dest - src).magnitude) {
            return;
        }

        Vector2 delta = (swingAccel * Time.deltaTime) * rigidbody2D.velocity;
        if (delta.magnitude == 0) {
            delta = Vector2.right * minAccel;
        } else if (delta.magnitude < minAccel) {
            delta *= (minAccel / delta.magnitude);
        }

        // max speed scales upwards as the distance increases
        float maxVelocity = maxVelocityBase + maxVelocityScale * (joint.distance - minLen);
        if (Input.GetAxisRaw("Horizontal") == 1) {
            if (rigidbody2D.velocity.x < 0) {
                rigidbody2D.velocity -= delta;
            } else {
                if(rigidbody2D.velocity.magnitude < maxVelocity) {
                    rigidbody2D.velocity += delta;
                }
            }
        }
        if (Input.GetAxisRaw("Horizontal") == -1) {
            if (rigidbody2D.velocity.x > 0) {
                rigidbody2D.velocity -= delta;
            } else {
                if (rigidbody2D.velocity.magnitude < maxVelocity) {
                    rigidbody2D.velocity += delta;
                }
            }
        }
	}

    public void ActivateSwing() {
        if (IsSwinging) {
            EndSwing();
        } else {
            StartSwing();
        }
    }

    private void StartSwing() {
        rigidbody2D.velocity = cc.velocity;
        cc.enabled = false;
        rigidbody2D.isKinematic = false;
        box.enabled = true;
        joint.enabled = true;
        Vector3 src = transform.position
            + new Vector3(joint.anchor.x, joint.anchor.y, 0);
        Vector3 dest = joint.connectedBody.transform.position
            + new Vector3(joint.connectedAnchor.x, joint.connectedAnchor.y, 0);
        joint.distance = (dest - src).magnitude;
    }
    private void EndSwing() {
        cc.velocity = rigidbody2D.velocity;
        cc.enabled = true;
        rigidbody2D.isKinematic = true;
        box.enabled = false;
        joint.enabled = false;
    }
}
