using UnityEngine;
using System.Collections;

namespace onegam_1412 {
    public class KillZone: MonoBehaviour {
        public void OnTriggerEnter2D(Collider2D other) {
            Debug.Log("Testing " + other.gameObject.name);
            if (other.gameObject.layer == LayerMask.NameToLayer("Attackable")) {
                Debug.Log("Removing " + other.gameObject.name);
                other.SendMessage("RemoveTarget");
            }
        }
    }
}