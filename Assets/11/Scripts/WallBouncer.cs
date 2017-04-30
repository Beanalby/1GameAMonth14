using UnityEngine;
using System.Collections;

public class WallBouncer : MonoBehaviour {

    private int balloonLayer;

    public void Start() {
        balloonLayer = LayerMask.NameToLayer("Pickup");
    }

    // when balloons hit the wall, just flip around their X velocity
    public void OnCollisionEnter2D(Collision2D coll) {
        if(coll.gameObject.layer != balloonLayer) {
            return;
        }
        /// relativeVelocity is the opposite of the balloon's original
        /// velocity, so we just need to flip Y
        Vector2 v = coll.relativeVelocity;
        v.y = -v.y;
        coll.gameObject.GetComponent<Rigidbody2D>().velocity = v;
    }
}
