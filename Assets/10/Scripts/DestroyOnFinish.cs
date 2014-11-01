using UnityEngine;
using System.Collections;

namespace onegam_1410 {
    [RequireComponent(typeof(ParticleSystem))]
    public class DestroyOnFinish : MonoBehaviour {

        public void Start() {
            Destroy(gameObject, GetComponent<ParticleSystem>().duration);
        }
    }
}