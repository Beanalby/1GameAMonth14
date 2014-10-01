using UnityEngine;
using System.Collections;

namespace onegam_1409 {
    public class CoinSparkle : MonoBehaviour {
        public void Start() {
            Destroy(gameObject, 1.5f);
        }
    }
}