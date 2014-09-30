using UnityEngine;
using System.Collections;

namespace onegam_1409 {
    public class PlayerGrabber : MonoBehaviour {
        int ropePointLayer;

        public void Start() {
            ropePointLayer = LayerMask.NameToLayer("RopePoint");
        }
        public void OnTriggerEnter2D(Collider2D other) {
            if(other.gameObject.layer == ropePointLayer) {
                Vector3 ledgePoint = other.transform.position + (Vector3)other.GetComponent<CircleCollider2D>().center;
                SendMessageUpwards("EdgeGrabbed", ledgePoint);
            }
        }
    }
}