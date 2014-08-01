using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace onegam_1407 {

    [RequireComponent(typeof(SphereCollider))]
    public class TrainDriver : TrainCommon {

        public float locomotiveSpeed = 3.5f;

        private int groundMask;
        private SphereCollider sphere;

        public void Start() {
            groundMask = 1 << LayerMask.NameToLayer("Ground");
            sphere = GetComponent<SphereCollider>();
            rail = GetNextSection();
            base.SetSpeed(locomotiveSpeed);
        }

        /// <summary>
        /// Use a collider to find the next rail section.
        /// Only done by the locomotive, other cars get rail sections through
        /// the queue.
        /// </summary>
        protected override RailSection GetNextSection() {
            Collider[] hits = Physics.OverlapSphere(transform.position + (transform.rotation * sphere.center),
                sphere.radius, groundMask);
            if(hits.Length != 1) {
                return null;
            }
            RailSection next = hits[0].GetComponent<RailSection>();
            PropagateNextSection(next);
            return next;
        }
        protected override void SetNextSection(RailSection next) {
            throw new System.NotImplementedException("Driver shouldn't get its rail set");
        }
    }
}