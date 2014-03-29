using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DistanceJoint2D))]
public class JointGizmo : MonoBehaviour {

    public void OnDrawGizmos() {
        DistanceJoint2D joint = GetComponent<DistanceJoint2D>();
        if (!joint.enabled || joint.connectedBody == null) {
            return;
        }
        Vector3 src = transform.position
            + new Vector3(joint.anchor.x, joint.anchor.y, 0);
        Vector3 dest = joint.connectedBody.transform.position
            + new Vector3(joint.connectedAnchor.x, joint.connectedAnchor.y, 0);
        //Debug.Log((dest - src).magnitude.ToString(".000000"));
        if (joint.distance*.99f <= (dest - src).magnitude) {
            Gizmos.color = Color.red;
        } else {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawLine(src, dest);
    }
}
