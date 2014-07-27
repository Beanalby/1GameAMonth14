using UnityEngine;
using System.Collections;

namespace onegam_1407 {

    [RequireComponent(typeof(SphereCollider))]
    public class TrainDriver : MonoBehaviour {

        private float speed = 3f;

        private RailSection rail;
        private float railPos = 0;
        private int groundMask;
        private SphereCollider sphere;

        private bool run = true;

        public void Start() {
            groundMask = 1 << LayerMask.NameToLayer("Ground");
            sphere = GetComponent<SphereCollider>();

            rail = FindNextSection();
        }

        public void Update() {
            if(run) {
                Move();
            }
        }

        private RailSection FindNextSection() {
            Collider[] hits = Physics.OverlapSphere(transform.position + (transform.rotation * sphere.center),
                sphere.radius, groundMask);
            if(hits.Length != 1) {
                return null;
            }
            RailSection rail = hits[0].GetComponent<RailSection>();
            return rail;
        }

        public void Move() {
            railPos += Time.deltaTime * speed;
            Vector3 newPos = rail.GetPosition(railPos);
            transform.rotation = Quaternion.LookRotation(newPos - transform.position);
            transform.position = newPos;
            float remainder = rail.GetRemainder(railPos);
            if(remainder > 0) {
                rail = FindNextSection();
                railPos = remainder;
                if(rail == null) {
                    Debug.LogError("End of the line!");
                    run = false;
                    Debug.Break();
                } else {
                    Debug.Log("On to next rail, got " + rail.name);
                }
            }
        }
    }
}