using UnityEngine;
using System.Collections;

namespace onegam_1407 {
    public class RailStraight : RailSection {

        public float length = 1f;
        private Vector3 EndPos;

        public void Start() {
            EndPos = new Vector3(0, 0, length);
        }
        public override Vector3 GetPosition(float position) {
            if(position >= EndPos.magnitude) {
                return transform.position + (transform.rotation * EndPos);
            } else {
                return Vector3.Lerp(transform.position, transform.position + (transform.rotation * EndPos),
                    position / EndPos.magnitude);
            }
        }
        public override float GetRemainder(float position) {
            return Mathf.Max(0, position - EndPos.magnitude);
        }

        public void OnDrawGizmos() {
            Gizmos.color = Color.cyan;
            Vector3 pos1 = transform.position, pos2 = transform.position + (transform.rotation * new Vector3(0, 0, length));
            pos1.y += .5f;
            pos2.y += .5f;
            Gizmos.DrawLine(pos1, pos2);
        }
    }
}