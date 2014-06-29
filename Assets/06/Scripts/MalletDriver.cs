using UnityEngine;
using System.Collections;

namespace onegam_1406 {
    public class MalletDriver : MonoBehaviour {

        public Mallet mallet;

        private Plane zeroPlane;
        private float swingStart = -1;
        private Vector3 swingPos;
        private float swingDuration = .8f;

        public void Start() {
            zeroPlane.SetNormalAndPosition(Vector3.up, Vector3.zero);
        }

        public void Update() {
            UpdatePosition();
            HandleMovement();
            if(Input.GetButtonDown("Fire1")) {
                Swing();
            }
        }

        void HandleMovement() {
        }

        void UpdatePosition() {
            // project the cursor onto the zero plane to find where
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float pos;
            zeroPlane.Raycast(mouseRay, out pos);
            transform.position = mouseRay.GetPoint(pos);
            if(swingStart == -1) {
                mallet.transform.position = transform.position;
            } else {
                // if we just swung, leave the mallet where it is.
                // after that, interpolate the mallet back to
                // where it should be (which is a moving target)
                float percent = (Time.time - swingStart) / swingDuration;
                if(percent >= 1) {
                    swingStart = -1;
                    mallet.transform.position = transform.position;
                } else {
                    if(percent > .75f) {
                        mallet.transform.position = Vector3.Lerp(swingPos,
                            transform.position,
                            (percent - .75f) * 4);
                    }
                }
            }
        }

        private void Swing() {
            swingStart = Time.time;
            swingPos = transform.position;
        }

        private void MoveDown() {
        }
        private void MoveUp() {
        }
    }
}