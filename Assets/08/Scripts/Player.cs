using UnityEngine;
using System.Collections;

namespace onegam_1408 {
    public class Player : MonoBehaviour {
        private float moveSpeed = 1f;
        public void Update() {
            rigidbody2D.MovePosition(rigidbody2D.position
                + new Vector2(moveSpeed * Time.deltaTime, 0));
        }
    }
}