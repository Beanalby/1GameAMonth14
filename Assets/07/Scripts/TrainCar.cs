using UnityEngine;
using System.Collections.Generic;

namespace onegam_1407 {
    public class TrainCar : TrainCommon {

        Queue<RailSection> railQueue = new Queue<RailSection>();

        protected override RailSection GetNextSection() {
            return railQueue.Dequeue();
        }
        protected override void SetNextSection(RailSection next) {
            if(rail == null) {
                rail = next; // initial rail
            } else {
                railQueue.Enqueue(next);
            }
            PropagateNextSection(next);
        }
    }
}