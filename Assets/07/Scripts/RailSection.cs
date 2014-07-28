using UnityEngine;
using System.Collections;

namespace onegam_1407 {

    [RequireComponent(typeof(BoxCollider))]
    public abstract class RailSection : MonoBehaviour {
        public abstract Vector3 GetPosition(float distance);
        public abstract float GetRemainder(float distance);

        private Material mat;
        private Transform meshObj;
        private BoxCollider box;
        private Color enabledColor = new Color(.3f, .8f, .3f, 1f),
            disabledColor = new Color(.5f, 0, 0, .25f);
        private Vector3 enabledPos, disabledPos;

        protected void baseAwake() {
            box = GetComponent<BoxCollider>();
            MeshRenderer mr = GetComponentInChildren<MeshRenderer>();
            meshObj = mr.transform;
            mat = mr.material;

            enabledPos = meshObj.transform.position;
            disabledPos = enabledPos;
            disabledPos.y -= .015f;
        }

        public void EnableRail() {
            mat.color = enabledColor;
            box.enabled = true;
            meshObj.position = enabledPos;
        }
        public void DisableRail() {
            box.enabled = false;
            mat.color = disabledColor;
            meshObj.position = disabledPos;
        }

        public void ToggleRail() {
            if(box.enabled) {
                DisableRail();
            } else {
                EnableRail();
            }
        }
    }
}