using UnityEngine;
using System.Collections;

namespace onegam_1407 {
    public class RailCurve : RailSection {

        public bool curveRight = true;

        private float curveSign = 1;
        // totalDistance = 1/4 of the circumference of the cirle with
        // the radius 2 = 2*pi*r / 4 = (2*pi*2) / 4 = pi
        private float curveDistance = Mathf.PI;

        // Use this for initialization
        void Start() {
            if(!curveRight) {
                curveSign = -1f;
            }
            //for(float i = 0; i <= 1; i += .25f) {
            //    Debug.Log(i + "=" + GetPosition(i * curveDistance));
            //}
            //Debug.Log("last=" + GetPosition(curveDistance));
        }

        // Update is called once per frame
        void Update() {
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