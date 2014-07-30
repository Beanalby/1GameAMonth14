using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace onegam_1407 {
    public enum CityName { Onett=0, Twoson, Threed, Fourside, Fiveoh, Sixas};

    public class City : MonoBehaviour {
        public static Array CityNames = Enum.GetValues(typeof(CityName));
        public CityName city;
        public Texture[] signs;

        public Color[] cityColorA;
        public Color[] cityColorB;

        public void Start() {
            MeshRenderer sign = transform.Find("sign").GetComponent<MeshRenderer>();
            sign.material.SetTexture("_MainTex", signs[(int)city]);
            MeshRenderer cityRend = transform.Find("city").GetComponent<MeshRenderer>();
            cityRend.materials[0].color = cityColorA[(int)city];
            cityRend.materials[1].color = cityColorB[(int)city];
        }

    }
}