using UnityEngine;
using System.Collections;

namespace onegam_1407 {

    public enum BranchChoice { Left, Right };

    public class RailBranch : MonoBehaviour {
        public RailSection railLeft;
        public RailSection railRight;
        public BranchChoice ActiveAtStart = BranchChoice.Left;

        public void Start() {
            if(ActiveAtStart == BranchChoice.Left) {
                railLeft.EnableRail();
                railRight.DisableRail();
            } else {
                railLeft.DisableRail();
                railRight.EnableRail();
            }
        }

        public void Switch() {
            railLeft.ToggleRail();
            railRight.ToggleRail();
        }
    }
}