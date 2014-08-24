using UnityEngine;
using System.Collections;

namespace onegam_1408 {
    [RequireComponent(typeof(CircleCollider2D))]
    public class Portal : MonoBehaviour {

        public float fullScale=-1;

        private float scaleStart=-1, scaleDuration = 1f, scaleFrom, scaleTo;
        
        private float spinSpeed = 30f;

        private int layerPlayer;
        private CircleCollider2D cc;
        private bool didInit = false;

        public void Start() {
            Init();
        }
        private void Init() {
            if(didInit) {
                return;
            }
            didInit = true;
            transform.localScale = Vector3.zero;
            layerPlayer = LayerMask.NameToLayer("Player");
            cc = GetComponent<CircleCollider2D>();
            cc.enabled = false;
        }

        public void Update() {
            UpdateScale();
            UpdateRotation();
        }

        private void UpdateScale() {
            if(scaleStart == -1f) {
                return;
            }
            float percent = (Time.time - scaleStart) / scaleDuration;
            if(percent >= 1) {
                FinishedScale();
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

        public void EnablePortal(bool force) {
            // EnablePortal might be forced before we did Start()
            Init();
            if(force) {
                transform.localScale = new Vector3(fullScale, fullScale, fullScale);
            } else {
                scaleStart = Time.time;
                scaleFrom = 0;
                scaleTo = fullScale;
            }
            cc.enabled = true;
        }
        public void DisablePortal() {
            scaleStart = Time.time;
            scaleFrom = fullScale;
            scaleTo = 0;
        }

        private void FinishedScale() {
            if(scaleTo == 0) {
                cc.enabled = false;
            }
            transform.localScale = new Vector3(scaleTo, scaleTo, scaleTo);
            scaleStart = -1f;

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