using UnityEngine;
using System.Collections;

namespace onegam_1407 {
    public class RailSwitch : MonoBehaviour {
        public RailSection railLeft;
        public RailSection railRight;

        public void Start() {
            railLeft.gameObject.SetActive(false);
            railRight.gameObject.SetActive(true);
        }

        public void Update() {
            if(Input.GetKeyDown(KeyCode.Space)) {
                railLeft.gameObject.SetActive(!railLeft.gameObject.activeSelf);
                railRight.gameObject.SetActive(!railRight.gameObject.activeSelf);
            }
        }
    }
}