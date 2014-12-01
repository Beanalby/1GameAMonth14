using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace onegam_1411 {
    public class Catcher : MonoBehaviour {

        private const int MAX_BALLOONS = 8;

        private List<Balloon> balloons;

        private Vector3 moveFrom=Vector3.zero, moveTo=Vector3.zero;
        private float moveStart = -1, moveDuration=.5f;

        private int pickupLayer;
        private bool forcingPop=false;

        private int currentCombo = 0;
        private Color currentComboColor = Color.black;

        public bool IsPopping {
            get { return moveStart != -1; }
        }

        public bool HasBalloons {
            get { return balloons.Count != 0; }
        }

        public void Start() {
            balloons = new List<Balloon>();
            pickupLayer = LayerMask.NameToLayer("Pickup");
        }
        public void Update() {
            HandleForcePop();
        }
        public void FixedUpdate() {
            HandleMovement();
        }

        public void OnTriggerEnter2D(Collider2D other) {
            if(other.gameObject.layer != pickupLayer) {
                return;
            }
            AddBalloon(other.GetComponent<Balloon>());
        }

        private void AddBalloon(Balloon balloon) {
            // existing balloons "move down" to compensate us moving up
            Vector3 pos;
            foreach(Balloon b in balloons) {
                pos = b.transform.localPosition;
                pos.y -= 1;
                b.transform.localPosition = pos;
            }
            // add the new balloon
            balloon.rigidbody2D.isKinematic = true;
            balloon.transform.parent = transform;
            balloon.transform.localPosition = new Vector3(0, -1, 0);
            balloons.Add(balloon.GetComponent<Balloon>());
            balloon.Caught();

            // we move up
            pos = transform.localPosition;
            pos.y += 1;
            transform.localPosition = pos;
            // if we're in the process of moving, adjust the targets
            if(moveStart != -1) {
                moveFrom.y += 1;
                moveTo.y += 1;
            }

            TryPop();
        }

        // used at the end of the game where we MUST pop all balloons
        public void ForcePop() {
            forcingPop = true;
        }

        private void HandleMovement() {
            // move ourselves if we're in the process
            if(moveStart == -1) {
                return;
            }
            float percent = (Time.time - moveStart) / moveDuration;
            if(percent >= 1) {
                transform.localPosition = moveTo;
                moveStart = -1;
                moveFrom = Vector3.zero;
                moveTo = Vector3.zero;
                // now that we're done moving, check for the next pop
                TryPop();
            } else {
                transform.localPosition = Vector3.Lerp(moveFrom, moveTo, percent);
            }
        }

        private void HandleForcePop() {
            if(!forcingPop) {
                return;
            }
            if(IsPopping) {
                return;
            }
            if(balloons.Count == 0) {
                forcingPop = false;
                return;
            }
            Pop(balloons[0]);
        }

        private void TryPop() {
            // with less than two balloons, popping never occurs.
            if(balloons.Count < 2) {
                return;
            }
            // if we're already moving downwards from a pop, don't bother
            // checking.  We'll automatically re-check for the next pop
            // once the movement is done.
            if(moveFrom != Vector3.zero) {
                return;
            }

            // if we have too many balloons, force pop
            if(balloons.Count >= MAX_BALLOONS) {
                Pop(balloons[0]);
                return;
           }

            // if the bottom balloon is a different color than any of the
            // other balloons, we pop it!
            Color bottom = balloons[0].color;
            for(int i = 1; i < balloons.Count; i++) {
                if(balloons[i].color != bottom) {
                    Pop(balloons[0]);
                    return;
                }
            }
            // todo: clear combo & such
        }

        private void Pop(Balloon balloon) {
            if(balloon.color != currentComboColor) {
                currentCombo = 0;
                currentComboColor = balloon.color;
            }
            currentCombo++;
            GameDriver.Instance.BalloonScored(balloon, currentCombo);

            balloons.Remove(balloon);
            balloon.PopGood();

           // move us downward
            moveFrom = transform.localPosition;
            moveTo = moveFrom;
            moveTo.y -= 1;
            moveStart = Time.time;
        }
    }
}