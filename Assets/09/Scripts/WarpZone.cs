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
            if(other.gameObject.layer != playerLayer) {
                return;
            }
            /// we might have the triggerHelper rather than the real trigger,
            /// grab the singleton just in case.
            Transform player = Player.Instance.transform;

            // pop him over to the other side
            int direction = 1;
            Vector3 offset;
            if(IsEastWest) {
                offset = Vector3.right;
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
                player.position = new Vector3(
                    otherZone.transform.position.x + offset.x,
                    player.position.y,
                    player.position.z);
            } else {
                player.position = new Vector3(
                    player.position.x,
                    otherZone.transform.position.y + offset.y,
                    player.position.z);
            }
        }
    }
}