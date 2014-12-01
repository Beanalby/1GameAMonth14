using UnityEngine;
using System.Collections;

namespace onegam_1411 {
    public class ScoreEffect : MonoBehaviour {

        [HideInInspector]
        public float Amount;

        private Vector3 velocity = new Vector3(3, 0, 0);
        private float speedPerSecond = 10;
        private float duration = 1f;
        private float timeStart;

        public void Start() {
            timeStart = Time.time;
            GetComponent<TextMesh>().text = Amount.ToString();
            // scoot us to the side right off the bat
            Vector3 pos = transform.position;
            pos.x += 1;
            transform.position = pos;

        }
        public void Update() {
            if(Time.time - timeStart > duration) {
                Destroy(gameObject);
                return;
            }

            // set our vertical velocity based on how long we've been around
            velocity.y = speedPerSecond * (Time.time - timeStart);
            Vector3 pos = transform.position;
            pos += (velocity * Time.deltaTime);
            transform.position = pos;
        }
    }
}