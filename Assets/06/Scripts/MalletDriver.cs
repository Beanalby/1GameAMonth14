using UnityEngine;
using System.Collections;

namespace onegam_1406 {
    public class MalletDriver : MonoBehaviour {

        private static MalletDriver _instance = null;
        public static MalletDriver Instance {
            get {
                if(_instance == null) {
                    Debug.LogError("Accessing BoardDriver instance before Awake");
                    Debug.Break();
                    return null;
                }
                return _instance;
            }
        }
        public void Awake() {
            if(_instance != null) {
                Debug.LogError("Two MalletDrivers, that shouldn't happen.");
                Debug.Break();
                return;
            }
            _instance = this;
        }

        public Mallet mallet;

        private Plane zeroPlane;
        private float upStart = -1;
        private Vector3 swingPos;
        private float swingDuration = .29f; // swingUp animation durationa

        public GameObject crosshairs;

        public bool CanSwing = false;

        private bool isDown = false;
        public void Start() {
            zeroPlane.SetNormalAndPosition(Vector3.up, Vector3.zero);
            CanSwing = true; // will eventually be done by GameController
        }

        public void Update() {
            UpdatePosition();
            if(CanSwing && Input.GetButtonDown("Fire1") && !isDown) {
                SwingDown();
            }
            if(CanSwing && isDown && Input.GetButtonUp("Fire1")) {
                SwingUp();
            }
        }

        void UpdatePosition() {
            // project the cursor onto the zero plane to find where
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float pos;
            zeroPlane.Raycast(mouseRay, out pos);
            transform.position = mouseRay.GetPoint(pos);
            // if the mallet's currently down, leave it.
            if(isDown) {
                return;
            }
            // if we're not moving up, mallet sticks to the target
            if(upStart == -1) {
                mallet.transform.position = transform.position;
                return;
            }
            // Mallet has been released and is moving up.
            // Leave it in place for most of the upward swing (to avoid
            // whacking other moles), then start interpolating the mallet
            // back to  where it should be (which is a moving target)
            float percent = (Time.time - upStart) / swingDuration;
            if(percent >= 1) {
                // it's totally up, screw all this interpolation crap
                // and we can go back to snapping to the target
                upStart = -1;
                crosshairs.SendMessage("SwingEnabled", SendMessageOptions.RequireReceiver);
                mallet.transform.position = transform.position;
            } else {
                if(percent > .75f) {
                    mallet.transform.position = Vector3.Lerp(swingPos,
                        transform.position,
                        (percent - .75f) * 4);
                }
            }
        }

        private void SwingDown() {
            mallet.transform.position = transform.position;
            isDown = true;
            mallet.SwingDown();
            swingPos = transform.position;
            crosshairs.SendMessage("SwingDisabled", SendMessageOptions.RequireReceiver);
        }
        private void SwingUp() {
            mallet.SwingUp();
            isDown = false;
            upStart = Time.time;
        }

        public void GameEnded() {
            CanSwing = false;
            mallet.gameObject.SetActive(false);
            crosshairs.SetActive(false);
        }
    }

}