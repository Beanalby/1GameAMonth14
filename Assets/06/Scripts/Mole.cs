using UnityEngine;
using System.Collections;

namespace onegam_1406 {

    public class Mole : MonoBehaviour {
        private float raiseDistance = 2f;
        private float raiseDuration = 1.5f;

        private bool isRaised = false;
        public bool IsRaised {
            get { return isRaised; }
        }

        public void Update() {
         }

        public void Raise() {
            StartCoroutine(doRaiseCycle());
        }
        private IEnumerator doRaiseCycle() {
            doRaise();
            yield return new WaitForSeconds(raiseDuration);
            Lower();
        }
        private void doRaise() {
            isRaised = true;
            transform.localPosition = new Vector3(0, raiseDistance, 0);
        }
        public void Lower() {
            if(isRaised) {
                transform.localPosition = Vector3.zero;
                isRaised = false;
            }
        }
    }
}