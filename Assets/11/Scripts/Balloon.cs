using UnityEngine;
using System.Collections;

namespace onegam_1411 {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Balloon : MonoBehaviour {

        public Color color;

        private float maxFallSpeed = -5f;

        private Vector2 puntVelocity = new Vector2(5, 12);

        public void Start() {
            // set our material's balloon color
            Material mat = GetComponentInChildren<MeshRenderer>().material;
            mat.SetColor("_Color", color);
            mat.SetColor("_Emission", color * .1f);
        }
        public void FixedUpdate() {
            CapFallSpeed();
        }

        public void Launch() {
            rigidbody2D.isKinematic = false;
        }
        public void Pop() {
            // todo: do something fancy.
            Destroy(gameObject);
        }

        public void Punt(Vector2 direction) {
            // we got punted!  give us big upward velocity and some horizontal
            Vector2 v = new Vector2(puntVelocity.x, puntVelocity.y);
            if(direction.x < 0) {
                v.x = -v.x;
            }
            rigidbody2D.velocity = v;
        }

        private void CapFallSpeed() {
            Vector3 v = rigidbody2D.velocity;
            if(v.y < maxFallSpeed) {
                v.y = maxFallSpeed;
                rigidbody2D.velocity = v;
            }
        }
    }
}