using UnityEngine;
using System.Collections;

namespace onegam_1407 {
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(LineRenderer))]
    public class RobotDriver : MonoBehaviour {

        public Transform RobotTip;

        private float moveSpeed = 5f;
        private float maxDistance = 16;
        private SphereCollider sphere;
        private LineRenderer lr;

        private Switch currentSwitch = null;

        public void Start() {
            sphere = GetComponent<SphereCollider>();
            lr = GetComponent<LineRenderer>();
        }
        public void Update() {
            //if(GameDriver.Instance.IsRunning) {
                HandleSwitch();
                HandleMove();
            //}
        }

        private void HandleSwitch() {
            if(Input.GetButtonDown("Jump")) {
                GrabSwitch();
            } else if(Input.GetButtonUp("Jump")) {
                ReleaseSwitch();
            }
            if(currentSwitch != null) {
                UpdateLaser();
                if(Input.GetAxisRaw("Horizontal") < 0) {
                    currentSwitch.SwitchLeft();
                } else if(Input.GetAxisRaw("Horizontal") > 0) {
                    currentSwitch.SwitchRight();
                }
            }
        }

        private void UpdateLaser() {
            lr.SetPosition(0, RobotTip.position);
            lr.SetPosition(1, currentSwitch.SwitchTip.position);
        }
        private void GrabSwitch() {
            Collider[] hits = Physics.OverlapSphere(transform.position + sphere.center,
                sphere.radius);
            if(hits.Length != 0) {
                currentSwitch = hits[0].GetComponent<Switch>();
                lr.enabled = true;
            }
        }
        private void ReleaseSwitch() {
            currentSwitch = null;
            lr.enabled = false;
        }

        private void HandleMove() {
            if(currentSwitch != null) {
                // no movement while holding a switch
                transform.localRotation = Quaternion.identity;
                return;
            } 
            // move based on horizontal input
            Vector3 pos = transform.localPosition;
            float deltaX = moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
            pos.x = Mathf.Max(-maxDistance, Mathf.Min(maxDistance, pos.x + deltaX));
            transform.localPosition = pos;
            transform.localRotation = Quaternion.Euler(new Vector3(0, -90 * Input.GetAxis("Horizontal"), 0));
        }
    }
}