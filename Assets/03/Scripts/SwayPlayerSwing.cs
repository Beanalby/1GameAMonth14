using UnityEngine;
using System.Collections;

public class SwayPlayerSwing : MonoBehaviour {

    public SwingArm arm;
    public AudioClip armOut, armIn;

    [HideInInspector]
    public bool IsSwinging {
        get {  return joint.enabled; }
    }
    public Vector2 SwingPoint {
        get { return joint.anchor; }
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
    private float lenSpeed = 3f;

    private int ropePointMask;
    private BoxCollider2D box;
    private DistanceJoint2D joint;
    private CharacterController2D cc;
    private SwayPlayer player;
    private SpriteRenderer armL;
    private int allButPlayerMask;

    public void Start() {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        cc = GetComponent<CharacterController2D>();
        box = GetComponent<BoxCollider2D>();
        player = GetComponent<SwayPlayer>();
        ropePointMask = 1 << LayerMask.NameToLayer("RopePoint");
        armL = transform.Find("robot_armL").GetComponent<SpriteRenderer>();
        allButPlayerMask = ~(1 << LayerMask.NameToLayer("Player"));
    }

    // not automatically called every frame, invoked by
    // SwayPlayer as needed
    public void FixedUpdate() {
        if(IsSwinging) {
            HandleSwing();
            HandleLengthen();
        }
    }

    private void HandleLengthen() {
        Vector3 us = transform.position
            + new Vector3(joint.anchor.x, joint.anchor.y, 0);
        Vector3 jointBase = joint.connectedBody.transform.position
            + new Vector3(joint.connectedAnchor.x, joint.connectedAnchor.y, 0);
        float curDistance = (jointBase - us).magnitude;

        float newDistance = -1;

        float vert = Input.GetAxisRaw("Vertical");
        if (vert > 0) {
            newDistance = Mathf.Max(minLen, curDistance - lenSpeed * vert * Time.deltaTime);
        } else if (vert < 0) {
            newDistance = Mathf.Min(maxLen, curDistance + lenSpeed * -vert * Time.deltaTime);
        }
        if (newDistance != -1) {
            // see if we'd hit anything by doing the lengthening
            Vector2 newPos = new Ray(jointBase, us - jointBase).GetPoint(newDistance);
            newPos += -joint.anchor;
            Vector2 pos1 = box.offset - box.size / 2 + newPos;
            Vector2 pos2 = box.offset + box.size / 2 + newPos;
            Collider2D obj = Physics2D.OverlapArea(pos1, pos2, allButPlayerMask);
            if(obj == null) {
                joint.distance = newDistance;
                // adjust our position manually if we're not moving,
                // changing DistanceJoint2D doesn't move us if we're still
                if(GetComponent<Rigidbody2D>().velocity.magnitude == 0) {
                    transform.position = newPos;
                }
            }
        }
    }

    private void HandleSwing() {

        Vector2 delta = (swingAccel * Time.deltaTime) * GetComponent<Rigidbody2D>().velocity;
        if (delta.magnitude == 0) {
            delta = Vector2.right * minAccel;
        } else if (delta.magnitude < minAccel) {
            delta *= (minAccel / delta.magnitude);
        }

        // max speed scales upwards as the distance increases
        float maxVelocity = maxVelocityBase + maxVelocityScale * (joint.distance - minLen);
        if (Input.GetAxisRaw("Horizontal") == 1) {
            if (GetComponent<Rigidbody2D>().velocity.x < 0) {
                GetComponent<Rigidbody2D>().velocity -= delta;
            } else {
                if(GetComponent<Rigidbody2D>().velocity.magnitude < maxVelocity) {
                    GetComponent<Rigidbody2D>().velocity += delta;
                }
            }
        }
        if (Input.GetAxisRaw("Horizontal") == -1) {
            if (GetComponent<Rigidbody2D>().velocity.x > 0) {
                GetComponent<Rigidbody2D>().velocity -= delta;
            } else {
                if (GetComponent<Rigidbody2D>().velocity.magnitude < maxVelocity) {
                    GetComponent<Rigidbody2D>().velocity += delta;
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
        
        GetComponent<Rigidbody2D>().velocity = cc.velocity;
        cc.enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = false;
        box.enabled = true;
        joint.enabled = true;
        arm.StartSwing(ropePoint);
        armL.enabled = false;
        
        Vector3 src = transform.position
            + new Vector3(joint.anchor.x, joint.anchor.y, 0);
        Vector3 dest = joint.connectedBody.transform.position
            + new Vector3(joint.connectedAnchor.x, joint.connectedAnchor.y, 0);
        joint.distance = (dest - src).magnitude;

        AudioSource.PlayClipAtPoint(armOut, transform.position);
    }

    public void EndSwing() {
        cc.velocity = GetComponent<Rigidbody2D>().velocity;
        cc.enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = true;
        box.enabled = false;
        joint.enabled = false;
        joint.connectedBody = null;
        arm.EndSwing();
        armL.enabled = true;

        // always face the direction we're now moving
        if((cc.velocity.x > 0 && transform.localScale.x < 0) // move L, face R
            || (cc.velocity.x < 0 && transform.localScale.x > 0)) { // move R, face L
            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z);
        }
        AudioSource.PlayClipAtPoint(armIn, transform.position);
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

        Ray2D optimal = new Ray2D(transform.position, optimalDir);

        // iterate over all the rope points within a bounding box
        // above and in front of us, and keep the point that's 
        // closest to our optimal line, while still being within ropeMax
        Collider2D bestPoint = null;
        float bestDist = Mathf.Infinity;
        Vector2 box1 = new Vector2(transform.position.x, transform.position.y);
        Vector2 box2 = box1 + new Vector2(ropeMax * player.FacingDir, ropeMax);

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
        return bestPoint ? bestPoint.GetComponent<Rigidbody2D>() : null;
    }

    private static float GetMinDistance(Ray2D ray, Collider2D col) {
        // find the place where the ray and the ray's normal
        // at the point intersect
        Vector2 colPos = col.transform.position;
        Vector2 pointRay1 = ray.origin, pointRay2 = ray.origin + ray.direction,
             pointCol1 = colPos, pointCol2 = colPos + new Vector2(-ray.direction.y, ray.direction.x);
        Vector2 intersect = LineIntersectionPoint(pointRay1, pointRay2, pointCol1, pointCol2);
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
