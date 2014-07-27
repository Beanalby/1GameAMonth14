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
                railLeft.gameObject.SetActive(true);
                railRight.gameObject.SetActive(false);
            } else {
                railLeft.gameObject.SetActive(false);
                railRight.gameObject.SetActive(true);
            }
        }

        public void Switch() {
            railLeft.gameObject.SetActive(!railLeft.gameObject.activeSelf);
            railRight.gameObject.SetActive(!railRight.gameObject.activeSelf);
        }
    }
}