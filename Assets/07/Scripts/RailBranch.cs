using UnityEngine;
using System.Collections;
using System.Linq;

namespace onegam_1407 {

    public enum BranchChoice { Left, Right };

    public class RailBranch : MonoBehaviour {
        public string branchName;
        public RailSection railLeft;
        public RailSection railRight;
        public MeshRenderer sign;
        public BranchChoice ActiveAtStart = BranchChoice.Left;
        public AudioClip switchSound;
        public Texture[] signs;

        public void Start() {
            if(ActiveAtStart == BranchChoice.Left) {
                railLeft.EnableRail();
                railRight.DisableRail();
            } else {
                railLeft.DisableRail();
                railRight.EnableRail();
            }
            sign.material.SetTexture("_MainTex", GetSignTexture());
        }

        public Texture GetSignTexture() {
            return signs.Single(c => c.name == "SwitchLabel-" + branchName);
        }
        public void Switch() {
            railLeft.ToggleRail();
            railRight.ToggleRail();
            AudioSource.PlayClipAtPoint(switchSound, Camera.main.transform.position);
        }
    }
}