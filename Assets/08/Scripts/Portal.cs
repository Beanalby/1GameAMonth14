using UnityEngine;
using System.Collections;

namespace onegam_1408 {
    public class Portal : MonoBehaviour {

        private float scaleStart, scaleDuration = 1f, scaleFrom, scaleTo;
        
        private float spinSpeed = 30f;

        private int layerPlayer;

        public void Start() {
            transform.localScale = Vector3.zero;
            layerPlayer = LayerMask.NameToLayer("Player");
        }

        public void Update() {
            if(Input.GetKeyDown(KeyCode.Z)) {
                EnablePortal();
            }
            if(Input.GetKeyDown(KeyCode.X)) {
                DisablePortal();
            }
            UpdateScale();
            UpdateRotation();
        }

        private void UpdateScale() {
            if(scaleStart == 1f) {
                return;
            }
            float percent = (Time.time - scaleStart) / scaleDuration;
            if(percent >= 1) {
                transform.localScale = new Vector3(scaleTo, scaleTo, scaleTo);
                scaleStart = -1f;
            } else {
                float s = Mathf.Lerp(scaleFrom, scaleTo, percent);
                transform.localScale = new Vector3(s, s, s);
            }
        }
        private void UpdateRotation() {
            Vector3 rot = transform.localRotation.eulerAngles;
            rot.z += spinSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.Euler(rot);
        }

        public void EnablePortal() {
            scaleStart = Time.time;
            scaleFrom = 0;
            scaleTo = 1f;
        }
        public void DisablePortal() {
            scaleStart = Time.time;
            scaleFrom = 1f;
            scaleTo = 0;
        }

        public void OnTriggerEnter2D(Collider2D other) {
            if(other.gameObject.layer == layerPlayer) {
                Player.Instance.SendMessage("PortalEntered");
            }
        }
        public void OnTriggerExit2D(Collider2D other) {
            if(other.gameObject.layer == layerPlayer) {
                Player.Instance.SendMessage("PortalExited");
            }
        }
    }
}