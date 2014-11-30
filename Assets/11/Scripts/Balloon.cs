using UnityEngine;
using System.Collections;

namespace onegam_1411 {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Balloon : MonoBehaviour {

        public Color color;

        private Vector2 puntVelocity = new Vector2(10, 30);

        public void Start() {
            // set our material's balloon color
            Material mat = GetComponentInChildren<MeshRenderer>().material;
            mat.SetColor("_Color", color);
            mat.SetColor("_Emission", color * .1f);
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

        public void OnCollisionEnter2D(Collision2D coll) {
            // if we're falling, bounce off of anything we hit.
            if(rigidbody2D.isKinematic) {
                return;
            }
            Debug.Log(name + " collided with " + coll.gameObject.name + ", me=" + rigidbody2D.velocity + ", relative=" + coll.relativeVelocity);
            // if it's a balloon on the stack, push ourselves away
            if(coll.gameObject.layer == gameObject.layer) {
                if(coll.gameObject.rigidbody2D.isKinematic == true) {
                    Vector2 v = new Vector2(puntVelocity.x, rigidbody2D.velocity.y);
                    if(coll.gameObject.transform.position.x > transform.position.x) {
                        v.x = -v.x;
                    }
                    rigidbody2D.velocity = v;
                }
            }
        }
  }
}