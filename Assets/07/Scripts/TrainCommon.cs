using UnityEngine;
using System.Collections.Generic;

namespace onegam_1407 {
    public abstract class TrainCommon : MonoBehaviour {

        private float speed = 3.5f;
        public TrainCommon nextCar = null;

        protected RailSection rail;
        protected float railPos = -1;


        abstract protected RailSection GetNextSection();
        abstract protected void SetNextSection(RailSection next);

        public void Update() {
            if(GameDriver.Instance.IsRunning) {
                Move();
            }
        }

        public void Move() {
            if(railPos == -1) {
                /// we're just starting, figure out our initial railPos from
                /// how far we are from the rail
                railPos = (transform.position - rail.transform.position).magnitude;
            }
            railPos += Time.deltaTime * speed;
            Vector3 newPos = rail.GetPosition(railPos);
            if(newPos != transform.position) {
                transform.rotation = Quaternion.LookRotation(newPos - transform.position);
            }
            transform.position = newPos;
            float remainder = rail.GetRemainder(railPos);
            if(remainder > 0) {
                rail = GetNextSection();
                railPos = remainder;
                if(rail == null) {
                    Debug.LogError("End of the line!");
                    Debug.Break();
                }
            }
        }

        protected void PropagateNextSection(RailSection next) {
            if(nextCar != null) {
                nextCar.SetNextSection(next);
            }
        }
    }
}
