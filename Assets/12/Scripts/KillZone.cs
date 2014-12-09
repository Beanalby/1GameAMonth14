using UnityEngine;
using System.Collections;

namespace onegam_1412 {
    public class KillZone: MonoBehaviour {
        public void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.layer == LayerMask.NameToLayer("Attackable")) {
                other.SendMessage("RemoveTarget");
            }
        }
    }
}