using UnityEngine;
using System.Collections;

namespace onegam_1411 {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Balloon : MonoBehaviour {

        public void Launch() {
            rigidbody2D.isKinematic = false;
        }
    }
}