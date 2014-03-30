using UnityEngine;
using System.Collections;

public class SwayPlayerSwing : MonoBehaviour {

    public SwingArm arm;

    [HideInInspector]
    public bool IsSwinging {
        get {  return joint.enabled; }
    }

    [HideInInspector]
    public Rigidbody2D SwingTarget {
        get { return joint.connectedBody; }
    }
    private float swingAccel = .5f;
    private float minAccel = .05f;
    private float maxVelocityBase = 5f;
    private float maxVelocityScale = 1f;

    private float ropeMax = 4;

    private float minLen = 1.5f;
    private float maxLen = 5f;
    private float lenSpeed = 1f;
    private float lenThreshold = .5f;

    private int ropePointMask;
    private BoxCollider2D box;
    private DistanceJoint2D joint;
    private CharacterController2D cc;
    private SwayPlayer player;

    public void Start() {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        cc = GetComponent<CharacterController2D>();
        box = GetComponent<BoxCollider2D>();
        player = GetComponent<SwayPlayer>();
        ropePointMask = 1 << LayerMask.NameToLayer("RopePoint");
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

    public void StartSwing() {
        Rigidbody2D ropePoint = GetBestRopePoint();
        if (ropePoint == null) { // no ropePoints
            return;
        }
        joint.connectedBody = ropePoint;
        
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
        arm.StartSwing(ropePoint);
    }

    public void EndSwing() {
        cc.velocity = rigidbody2D.velocity;
        cc.enabled = true;
        rigidbody2D.isKinematic = true;
        box.enabled = false;
        joint.enabled = false;
        joint.connectedBody = null;
        arm.EndSwing();

        // always face the direction we're now moving
        if((cc.velocity.x > 0 && transform.localScale.x < 0) // move L, face R
            || (cc.velocity.x < 0 && transform.localScale.x > 0)) { // move R, face L
            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z);
        }
    }

    private Rigidbody2D GetBestRopePoint() {
        /// figure out the "optimal" line we want the anchor point
        /// to lie upon: 
        /// * if we're moving upward or back, just use 45 degrees 
        /// * if we're moving downward, then use a line perpendicular
        /// to our velocity.  If we're moving down fast, we'll try
        /// to find a point far ahead, that way we maintain velocity once
        /// the rope starts constraining us.
        Vector2 optimalDir;
        if (cc.velocity == Vector3.zero || cc.velocity.y >= 0) {
            optimalDir = new Vector2(player.FacingDir, 1);
        } else {
            optimalDir = new Vector2(-cc.velocity.y, cc.velocity.x);
        }

        //= (cc.velocity.x < 0) ? new Vector2(-1,1) : Vector2.one;
        Ray2D optimal = new Ray2D(transform.position, optimalDir);

        // iterate over all the rope points within a bounding box
        // above and in front of us, and keep the point that's 
        // closest to our optimal line, while still being within ropeMax
        Collider2D bestPoint = null;
        float bestDist = Mathf.Infinity;
        Vector3 pos = transform.position;
        Vector2 box1 = new Vector2(pos.x - ropeMax, pos.y);
        Vector2 box2 = new Vector3(pos.x + ropeMax, pos.y + ropeMax);

        foreach(Collider2D col in Physics2D.OverlapAreaAll(box1, box2, ropePointMask)) {
            if ((col.transform.position - transform.position).magnitude >= ropeMax) {
                continue;
            }
            float minDist = GetMinDistance(optimal, col);
            if (minDist < bestDist) {
                bestDist = minDist;
                bestPoint = col;
            }
        }
        return bestPoint ? bestPoint.rigidbody2D : null;
    }

    private static float GetMinDistance(Ray2D ray, Collider2D col) {
        // find the place where the ray and the ray's normal
        // at the point intersect
        Vector2 colPos = col.transform.position;
        Vector2 pointRay1 = ray.origin, pointRay2 = ray.origin + ray.direction,
             pointCol1 = colPos, pointCol2 = colPos + new Vector2(-ray.direction.y, ray.direction.x);
        Vector2 intersect = LineIntersectionPoint(pointRay1, pointRay2, pointCol1, pointCol2);
        //Debug.Log(colPos.ToString() + ": origin=" + ray.origin + ", intersect=" + intersect + ", dist=" + (intersect - colPos).magnitude);
        return (intersect - colPos).magnitude;
    }

    private static Vector2 LineIntersectionPoint(Vector2 ps1, Vector2 pe1, Vector2 ps2, Vector2 pe2)
    {
      // Get A,B,C of first line - points : ps1 to pe1
      float A1 = pe1.y-ps1.y;
      float B1 = ps1.x-pe1.x;
      float C1 = A1*ps1.x+B1*ps1.y;

      // Get A,B,C of second line - points : ps2 to pe2
      float A2 = pe2.y-ps2.y;
      float B2 = ps2.x-pe2.x;
      float C2 = A2*ps2.x+B2*ps2.y;

      // Get delta and check if the lines are parallel
      float delta = A1*B2 - A2*B1;
      if(delta == 0)
         throw new System.Exception("Lines are parallel");

      //Debug.Log("(" + ps1 + "," + pe1 + ") (" + ps2 + "),(" + pe2 + ")=" + new Vector2((B2 * C1 - B1 * C2) / delta, (A1 * C2 - A2 * C1) / delta));
      // now return the Vector2 intersection point
      return new Vector2(
          (B2*C1 - B1*C2)/delta,
          (A1*C2 - A2*C1)/delta
      );
    }

}
