using UnityEngine;
using System.Collections;

namespace onegam_1407 {
    public class RailCurve : RailSection {

        public bool curveRight = true;

        private float curveSign = 1;
        // totalDistance = 1/4 of the circumference of the cirle with
        // the radius 2 = 2*pi*r / 4 = (2*pi*2) / 4 = pi
        private float curveDistance = Mathf.PI;

        public void Awake() {
            base.baseAwake();
        }
        public void Start() {
            if(!curveRight) {
                curveSign = -1f;
                // flip the mesh's scale, and move its position accordingly
                GameObject meshObj = GetComponentInChildren<MeshRenderer>().gameObject;
                meshObj.transform.localScale = new Vector3(-1, 1, 1);
                Vector3 pos = meshObj.transform.localPosition;
                pos.x = -pos.x;
                meshObj.transform.localPosition = pos;
            }
            
        }

        public override Vector3 GetPosition(float position) {
            return transform.position + RotateAroundPoint(Vector3.zero,
                transform.rotation * (2*curveSign*Vector3.right),
                curveSign * (90 / curveDistance) * position);
        }
        public override float GetRemainder(float position) {
            return Mathf.Max(0, position - curveDistance);
        }
        public Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, float angle) {
            return Quaternion.Euler(0, angle, 0) * (point - pivot) + pivot;
        }

        public void OnDrawGizmos() {
            float sign = (curveRight ? 1 : -1);
            Vector3[] positions = new Vector3[] {
                new Vector3(0, 0, 0),
                new Vector3(sign * .2f, 0, .8f),
                new Vector3(sign * .6f, 0, 1.4f),
                new Vector3(sign * 1.2f, 0, 1.8f),
                new Vector3(sign * 2f, 0, 2f)
            };

            Gizmos.color = Color.cyan;
            for(int i = 0; i < positions.Length - 1; i++) {
                Vector3 pos1 = transform.position + (transform.rotation * positions[i]);
                Vector3 pos2 = transform.position + (transform.rotation * positions[i+1]);
                pos1.y += .5f;
                pos2.y += .5f;
                Gizmos.DrawLine(pos1, pos2);
            }
        }
    }
}