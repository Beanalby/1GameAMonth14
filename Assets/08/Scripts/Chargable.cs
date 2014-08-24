using UnityEngine;
using System.Collections;

namespace onegam_1408 {
    public class Chargable : MonoBehaviour {
        public Portal portal;
        public MeshRenderer chargeLight;
        public float decaySpeed = .05f, chargeSpeed = .5f;
        public bool decayAfterFull = true;

        private Material lightMat;
        private Color offColor = Color.red, onColor = Color.green;

        private bool isCharged = false;
        public bool IsCharged { get { return isCharged; } }
        public bool IsFullyCharged { get { return charge >= 1; } }
        public bool CanStartCharging { get { return charge <= .9f; } }
        private bool isCharging = false;
        private float charge = 0f;
        private float offThreshold = .2f, onThreshold = .5f;

        public void Start() {
            portal.gameObject.SetActive(true);
            lightMat = chargeLight.material;
            lightMat.SetColor("Emission", offColor);
        }

        public void Update() {
            updateCharge();
        }

        private void updateCharge() {
            if(isCharging) {
                DecayCharge();
                EnhanceCharge();
                if(!isCharged && charge > onThreshold) {
                    TurnOn(false);
                }
            } else {
                // don't decay if we're full and we don't decayAfterFull
                if(!isCharged || decayAfterFull) {
                    DecayCharge();
                    if(isCharged && charge < offThreshold) {
                        TurnOff();
                    }
                }
            }
            lightMat.SetColor("_Emission", Color.Lerp(offColor, onColor, charge));
        }

        private void EnhanceCharge() {
            charge = Mathf.Min(1, charge + chargeSpeed * Time.deltaTime);
        }
        private void DecayCharge() {
            charge = Mathf.Max(0, charge - decaySpeed * Time.deltaTime);
        }
        private void TurnOn(bool force) {
            isCharged = true;
            portal.EnablePortal(force);
            SendMessage("TurnedOn", SendMessageOptions.DontRequireReceiver);
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

        // used when scene is loaded and this waypoint is already enabled
        public void ForceOn() {
            charge = 1;
            TurnOn(true);
        }
   }
}