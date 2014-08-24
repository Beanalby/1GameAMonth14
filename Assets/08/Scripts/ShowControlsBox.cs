using UnityEngine;
using System.Collections;

namespace onegam_1408 {
    public class ShowControlsBox : MonoBehaviour {
        public GUISkin skin;
        private GUIStyle msgStyle;

        private ShowControls sc;
        private string mainMsg = null;
        private int layerPlayer;
        
        public void Start() {
            layerPlayer = LayerMask.NameToLayer("Player");
            switch(name) {
                case "scMovement":
                    sc = ShowControls.CreateDocked(new ControlItem[] {
                        new ControlItem("Use Left and Right to Move",
                            new KeyCode[] { KeyCode.LeftArrow, KeyCode.RightArrow }),
                        new ControlItem("Use Space to Jump", KeyCode.Space)
                    });
                    mainMsg = "Activate all the waypoints to your world!";
                    break;
                case "scCharge":
                    sc = ShowControls.CreateDocked(
                        new ControlItem("Hold down to charge beacons to your world.",
                            KeyCode.DownArrow));
                    mainMsg = "Beacons heal you,\nbut need recharged after a time.";
                    break;
                case "scWaypoint":
                    sc = ShowControls.CreateDocked(
                        new ControlItem("Hold down to charge waypoints.",
                            KeyCode.DownArrow));
                    mainMsg = "Waypoints take longer to charge,\n but stay permanently activated.";
                    break;
            }
            sc.showDuration = Mathf.Infinity;
        }

        public void OnGUI() {
            if(mainMsg != null && sc.IsShown) {
                ShadowAndOutline.DrawShadow(new Rect(0, 100, Screen.width, 200),
                    new GUIContent(mainMsg), skin.label, Color.white, Color.black,
                    new Vector3(2, -2));
            }
        }

        public void OnTriggerEnter2D(Collider2D other) {
            if(other.gameObject.layer == layerPlayer) {
                sc.Show();
            }
        }
        public void OnTriggerExit2D(Collider2D other) {
            if(other.gameObject.layer == layerPlayer) {
                sc.Hide();
            }
        }
    }
}