using UnityEngine;
using System.Collections;

namespace onegam_1406 {
    public class MalletDriver : MonoBehaviour {
        //private Vector3 downBase = new Vector3(1.342f, .825f, -1.235f);
        //private Vector3 downDelta;
        //private Vector3 downRot = new Vector3(0, -45, 0);
        //private float downSpeed = .1f;
        //private Interpolate.Function downFunc = Interpolate.Ease(Interpolate.EaseType.EaseInCubic);

        //private Vector3 upPos = new Vector3(2.342f, 3.820f, -1.235f);
        //private Vector3 upRot = new Vector3(-43.88f, -48.71f, 25.948f);
        //private float upSpeed = .5f;
        //private Interpolate.Function upFunc = Interpolate.Ease(Interpolate.EaseType.EaseInOutCubic);

        public Transform mallet;

        private Vector3 moveBase, moveDelta;
        private Vector3 rotBase, rotDelta;
        private float moveStart = -1f, moveSpeed;
        private Interpolate.Function moveFunc;

        private Plane zeroPlane;

        private bool isSwinging = false;

        public void Start() {
            zeroPlane.SetNormalAndPosition(Vector3.up, Vector3.zero);
        }

        public void Update() {
            UpdatePosition();
            HandleMovement();
            if(Input.GetButtonDown("Fire1")) {
                StartCoroutine(Swing());
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
        }

        private IEnumerator Swing() {
            MoveDown();
            isSwinging = true;
           yield return new WaitForSeconds(.5f);
            MoveUp();
            isSwinging = false;
        }

        private void MoveDown() {
        }
        private void MoveUp() {
        }
    }
}