using UnityEngine;
using System.Collections;

namespace onegam_1407 {
    public class RailStraight : RailSection {

        public Vector3 EndPos;
        public override Vector3 GetPosition(float position) {
            if(position >= EndPos.magnitude) {
                return transform.position + EndPos;
            } else {
                return Vector3.Lerp(transform.position, transform.position + EndPos,
                    position / EndPos.magnitude);
            }
        }
        public override float GetRemainder(float position) {
            return Mathf.Max(0, position - EndPos.magnitude);
        }

        public void OnDrawGizmos() {
            Gizmos.color = Color.cyan;
            Vector3 pos1 = transform.position, pos2 = transform.position + EndPos;
            pos1.y += .5f;
            pos2.y += .5f;
            Gizmos.DrawLine(pos1, pos2);
        }
    }
}