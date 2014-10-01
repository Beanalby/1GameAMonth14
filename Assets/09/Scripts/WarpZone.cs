using UnityEngine;
using System.Collections;

namespace onegam_1409 {
    public class WarpZone : MonoBehaviour {
        public bool IsEastWest;
        public WarpZone otherZone;
        private int playerLayer;

        public void Start() {
            playerLayer = LayerMask.NameToLayer("Player");
        }

        public void OnTriggerEnter2D(Collider2D other) {
            Transform t = other.transform;
            if(other.gameObject.layer == playerLayer) {
                /// we might have the triggerHelper rather than the real trigger,
                /// grab the singleton just in case.
                t = Player.Instance.transform;
            } else if(other.gameObject.tag != "Pickup") {
                // pickups are the only other things that warp
                return;
            }

            // pop him over to the other side
            int direction = 1;
            Vector3 offset;
            if(IsEastWest) {
                offset = Vector3.right * 1.5f;
                if(transform.position.x < otherZone.transform.position.x) {
                    direction = -1;
                }
            } else {
                offset = Vector3.up;
                if(transform.position.y < otherZone.transform.position.y) {
                    direction = -1;
                    /// When we warp them up, need some extra space to account
                    /// for the fact that the pivot is in their feet, not
                    /// the center.
                    offset *= 3;
                }
            }
            offset *= direction;
            if(IsEastWest) {
                // move it up just a TINY bit to make sure it doesn't get
                // stuck in the floor.
                t.position = new Vector3(
                    otherZone.transform.position.x + offset.x,
                    t.position.y + .1f,
                    t.position.z);
            } else {
                t.position = new Vector3(
                    t.position.x,
                    otherZone.transform.position.y + offset.y,
                    t.position.z);
            }
            t.SendMessage("Warped", SendMessageOptions.DontRequireReceiver);
        }
    }
}