using UnityEngine;
using System.Collections;

namespace onegam_1406 {

    public class CameraShake : MonoBehaviour {

        private Vector3 basePos;

        private Vector3 shakeEffectFrom, shakeEffectTo;
        private float shakeEffectAmount = .08f;
        private float shakeEffectDuration = .08f;
        private float shakeEffectStart = -1;

        public void Start() {
            basePos = transform.position;
        }
        public void Update() {
            HandleshakeEffect();
        }

        private void HandleshakeEffect() {
            if(shakeEffectStart == -1)
                return;
            if(shakeEffectStart + shakeEffectDuration < Time.time) {
                shakeEffectStart = -1;
                transform.position = basePos;
                return;
            }
            float percent = (Time.time - shakeEffectStart) / shakeEffectDuration;
            Vector3 pos;
            if(percent < .5f)
                pos = Vector3.Lerp(shakeEffectFrom, shakeEffectTo, percent * 2f);
            else
                pos = Vector3.Lerp(shakeEffectTo, shakeEffectFrom, (percent - .5f) * 2f);
            transform.position = pos;
        }

        public void Shake() {
            // if it's already shaking, don't bother
            if(shakeEffectStart != -1) {
                return;
            }
            shakeEffectStart = Time.time;
            shakeEffectFrom = basePos;
            shakeEffectTo = shakeEffectFrom;
            shakeEffectTo.y += shakeEffectAmount;
        }
    }
}