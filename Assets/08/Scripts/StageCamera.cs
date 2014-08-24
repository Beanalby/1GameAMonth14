using UnityEngine;
using System.Collections;

namespace onegam_1408 {
    public class StageCamera : MonoBehaviour {
        public GameObject redHaze;
        private Color hazeFull = new Color(1, 0, 0, .5f), hazeEmpty = new Color(1, 0, 0, 0);
        private Material hazeMat;

        public void Start() {
            hazeMat = redHaze.GetComponent<MeshRenderer>().material;
        }

        public void SetHaze(float percent) {
            hazeMat.color = Color.Lerp(hazeEmpty, hazeFull, percent);
        }
    }
}