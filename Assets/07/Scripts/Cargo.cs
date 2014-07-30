using UnityEngine;
using System.Collections;

namespace onegam_1407 {

    public class Cargo : MonoBehaviour {
        public CityName city;
        public Texture[] cubes;

        public void Start() {
            GetComponentInChildren<MeshRenderer>().material.SetTexture(
                "_MainTex", cubes[(int)city]);
        }
    }
}