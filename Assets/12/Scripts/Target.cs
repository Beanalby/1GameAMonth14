using UnityEngine;
using System.Collections;

namespace onegam_1412 {
    public class Target : MonoBehaviour {

        public SpriteRenderer face, body;

        public Sprite body1, body2;
        public Sprite facePuked;

        private float animToggle = 1.25f;
        private float walkSpeed = .75f, runSpeed = 3;

        private float moveStart = -1;

        private Color skinMin = new Color(.3411f, .2785f, .0352f);
        private Color skinMax = new Color(.8352f, .7529f, .4274f);

        public void Start() {
            float percent = Random.Range(0f, 1f);
            body.material.SetColor("_Color", Color.Lerp(skinMin, skinMax, percent));
            moveStart = Time.time;
            rigidbody2D.velocity = walkSpeed * rigidbody2D.velocity.normalized;
        }

        public void Update() {
            UpdateBody();
        }

        private void UpdateBody() {
            if(moveStart == -1) {
                return;
            }
            float percent = ((Time.time - moveStart) % animToggle) / animToggle;
            if(percent < .5f) {
                body.sprite = body1;
            } else {
                body.sprite = body2;
            }
        }

        public void GotPuked(Transform player) {
            // got puked on, run away!
            face.sprite = facePuked;
            Vector3 runDir = (transform.position - player.transform.position).normalized;
            rigidbody2D.velocity = runSpeed * runDir;
            GameDriver.Instance.TargetPuked();
        }

        public void RemoveTarget() {
            GameDriver.Instance.TargetRemoved();
            Destroy(gameObject);
        }
    }
}