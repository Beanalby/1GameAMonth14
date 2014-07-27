using UnityEngine;
using System.Collections;

namespace onegam_1407 {
    public class Switch : MonoBehaviour {
        public RailBranch target;
        public KeyCode key;

        public void Update() {
            if(Input.GetKeyDown(key) && GameDriver.Instance.IsRunning) {
                target.Switch();
            }
        }
    }
}