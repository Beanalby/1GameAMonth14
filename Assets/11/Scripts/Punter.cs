using UnityEngine;
using System.Collections;

namespace onegam_1411 {
    public class Punter : MonoBehaviour {

        public AudioClip puntSound;

        private int balloonMask;

        public void Start() {
            balloonMask = 1 << LayerMask.NameToLayer("Pickup");
        }

        public void Update() {
            // todo: move input management into player
            if(Input.GetButtonDown("Jump")) {
                Punt();
            }
        }

        private void Punt() {
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            foreach(Ray2D ray in GetPuntRays()) {
                PuntRay(ray.origin, ray.direction, box.size.x);
            }
            AudioSource.PlayClipAtPoint(puntSound, Camera.main.transform.position);
       }

        private void PuntRay(Vector2 origin, Vector2 direction, float distance) {
            foreach(RaycastHit2D hit in Physics2D.RaycastAll(origin, direction, distance, balloonMask)) {
                hit.transform.GetComponent<Balloon>().Punt(direction);
            }
        }

        private Ray2D[] GetPuntRays() {
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            Ray2D[] rays = new Ray2D[6];
            Vector2 origin = new Vector2();

            // shoot rays to the right, on top
            origin.x = transform.position.x + box.center.x - box.size.x/2;
            origin.y = transform.position.y + box.center.y + box.size.y/2;
            rays[0] = new Ray2D(origin, Vector2.right);
            // middle
            origin.y = transform.position.y + box.center.y;
            rays[1] = new Ray2D(origin, Vector2.right);
            // bottom
            origin.y = transform.position.y + box.center.y - box.size.y / 2;
            rays[2] = new Ray2D(origin, Vector2.right);
            
            // also shoot rays on the left, starting with top
            origin.x = transform.position.x - box.center.x + box.size.x / 2;
            origin.y = transform.position.y + box.center.y + box.size.y/2;
            rays[3] = new Ray2D(origin, -Vector2.right);
            // middle
            origin.y = transform.position.y + box.center.y;
            rays[4] = new Ray2D(origin, -Vector2.right);
            // bottom
            origin.y = transform.position.y + box.center.y - box.size.y / 2;
            rays[5] = new Ray2D(origin, -Vector2.right);
            
            return rays;
        }

        public void OnDrawGizmos() {
            // draw our punting rays for debugging
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            foreach(Ray2D ray in GetPuntRays()) {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(ray.origin, ray.origin + (ray.direction * box.size.x));
            }
       }
    }
}