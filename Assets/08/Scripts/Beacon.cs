using UnityEngine;
using System.Collections;

namespace onegam_1408 {
    public class Beacon : MonoBehaviour {
        public MeshRenderer beaconLight;
        public Portal portal;
        private bool isCharged = false;

        private Material lightMat;
        private bool isCharging = false;
        private float charge = 0f;
        private float offThreshold = .2f, onThreshold = .5f;
        private float decaySpeed = .05f, chargeSpeed = .5f;

        private Color offColor = Color.red, onColor = Color.green;

        public void Start() {
            lightMat = beaconLight.material;
            lightMat.SetColor("Emission", offColor);
            portal.gameObject.SetActive(true);
        }

        public void Update() {
            updateCharge();
        }

        private void updateCharge() {
            charge = Mathf.Max(0, charge - decaySpeed * Time.deltaTime);
            if(isCharging) {
                charge = Mathf.Min(1, charge + chargeSpeed * Time.deltaTime);
                if(!isCharged && charge > onThreshold) {
                    TurnOn();
                }
            } else {
                if(isCharged && charge < offThreshold) {
                    TurnOff();
                }
            }
            lightMat.SetColor("_Emission", Color.Lerp(offColor, onColor, charge));
        }

        private void TurnOn() {
            isCharged = true;
            portal.EnablePortal();
        }
        private void TurnOff() {
            isCharged = false;
            portal.DisablePortal();
        }
        public void StartCharge() {
            isCharging = true;
        }
        public void StopCharge() {
            isCharging = false;
        }
    }
}