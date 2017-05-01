using UnityEngine;
using System.Collections;

namespace onegam_1411 {
    public class KillZone : MonoBehaviour {

        public void OnTriggerEnter2D(Collider2D other) {
            // should only be balloons down here
            Balloon b = other.gameObject.GetComponent<Balloon>();
            if(b == null) {
                Debug.LogError("No balloon on killzone GameObject " + other.gameObject.name);
                return;
            }
            GameDriver.Instance.BalloonMissed();
            b.PopBad();
        }
    }
}