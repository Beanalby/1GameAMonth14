using UnityEngine;
using System.Collections;

namespace onegam_1411 {
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleOneShot : MonoBehaviour {
        public void Start() {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constant);
        }
    }
}