using UnityEngine;
using System.Collections;

namespace onegam_1407 {
    public enum SwitchPosition { Left, Right };

    public class Switch : MonoBehaviour {
        public Transform SwitchTip;
        public RailBranch target;
        private SwitchPosition pos;
        public Transform switchMesh;
        public MeshRenderer sign;

        public void Start() {
            sign.material.SetTexture("_MainTex", target.GetSignTexture());
        }

        public void SwitchLeft() {
            if(pos != SwitchPosition.Left) {
                pos = SwitchPosition.Left;
                Debug.Log("Switching to " + pos);
                target.Switch();
                switchMesh.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            }
        }
        public void SwitchRight() {
            if(pos != SwitchPosition.Right) {
                pos = SwitchPosition.Right;
                Debug.Log("Switching to " + pos);
                target.Switch();
                switchMesh.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
            }
        }
    }
}