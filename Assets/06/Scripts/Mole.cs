using UnityEngine;
using System.Collections;

namespace onegam_1406 {

    public class Mole : MonoBehaviour {
        private Vector3 raiseDistance = new Vector3(0, 2.5f, 0);
        private float raiseSpeed = .25f;
        private float raiseDuration = 1.5f;

        private Vector3 moveBase, moveDelta;
        private float moveStart = -1;
        Interpolate.Function moveFunc = Interpolate.Ease(Interpolate.EaseType.EaseOutCubic);

        private bool isRaised = false;
        public bool IsRaised {
            get { return isRaised; }
        }

        public void Update() {
            HandleMove();
         }

        private void HandleMove() {
            if(moveStart == -1) {
                return;
            }
            float percent = (Time.time - moveStart) / raiseSpeed;
            if(percent >= 1) {
                transform.localPosition = moveBase + moveDelta;
                moveStart = -1;
            } else {
                transform.localPosition = Interpolate.Ease(
                    moveFunc, moveBase, moveDelta, percent, 1);
            }
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
            moveStart = Time.time;
            moveBase = transform.localPosition;
            moveDelta = raiseDistance - transform.localPosition;
        }
        public void Lower() {
            if(isRaised) {
                isRaised = false;
                moveStart = Time.time;
                moveBase = transform.localPosition;
                moveDelta = -transform.localPosition;
            }
        }
    }
}