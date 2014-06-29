using UnityEngine;
using System.Collections;

namespace onegam_1406 {

    public class Mole : MonoBehaviour {
        public GameObject hitEffect;
        public GameObject missEffect;

        private float raiseSpeed;
        private Vector3 raiseDistance = new Vector3(0, 2.5f, 0);

        private Vector3 moveBase, moveDelta;
        private float moveStart = -1;
        Interpolate.Function moveFunc = Interpolate.Ease(Interpolate.EaseType.EaseOutCubic);

        private bool isRaised = false;
        public bool IsRaised {
            get { return isRaised; }
        }

        private Vector3 basePos;

        public void Start() {
            basePos = transform.position;
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
                if(!isRaised) {
                    // if we got here, then we didn't get hit.  BOO!
                    MoleMissed();
                }
            } else {
                transform.localPosition = Interpolate.Ease(
                    moveFunc, moveBase, moveDelta, percent, 1);
            }
        }

        public bool Raise(float duration) {
            // if it's raised or moving, fail this raising.
            if(isRaised || moveStart != -1) {
                return false;
            }
            isRaised = true;
            StartCoroutine(doRaiseCycle(duration));
            return true;
        }
        private IEnumerator doRaiseCycle(float duration) {
            raiseSpeed = duration / 8;
            doRaise();
            yield return new WaitForSeconds(duration * 0.875f);
            Lower();
        }
        private void doRaise() {
            moveStart = Time.time;
            moveBase = Vector3.zero;
            moveDelta = raiseDistance;
        }
        public void Lower() {
            if(isRaised) {
                isRaised = false;
                moveStart = Time.time;
                moveBase = raiseDistance;
                moveDelta = -raiseDistance;
            }
        }
 
        public void OnTriggerEnter(Collider other) {
            MoleHit();
        }

        private void MoleHit() {
            BoardDriver.Instance.MoleHit(this);
            // create the hitEffect and move it to our location
            GameObject obj = Instantiate(hitEffect) as GameObject;
            obj.transform.position += basePos;

            // immediately go all the way down, screw any movement
            transform.position = basePos;
            isRaised = false;
            moveStart = -1;
        }
        private void MoleMissed() {
            // skip it if the game's already over
            if(!GameDriver.Instance.IsRunning) {
                return;
            }
            BoardDriver.Instance.MoleMiss(this);
            // create a missEffect and move it to our location
            GameObject obj = Instantiate(missEffect) as GameObject;
            obj.transform.position += basePos;
        }

        /// <summary>
        /// For debugging, will be removed before publish
        /// </summary>
        /// <returns></returns>
        public MoleHole GetHole() {
            return transform.parent.GetComponent<MoleHole>();
        }
    }
}
