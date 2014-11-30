using UnityEngine;
using System.Collections;

namespace onegam_1411 {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Balloon : MonoBehaviour {

        public Color color;

        public void Start() {
            // set our material's balloon color
            Material mat = GetComponentInChildren<MeshRenderer>().material;
            mat.SetColor("_Color", color);
            mat.SetColor("_Emission", color * .1f);
        }
        public void Launch() {
            rigidbody2D.isKinematic = false;
        }
    }
}