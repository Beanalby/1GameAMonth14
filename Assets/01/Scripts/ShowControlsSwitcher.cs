using UnityEngine;
using System.Collections;

public class ShowControlsSwitcher : MonoBehaviour {

    private ShowControls sc = null;
    public void Start() {
        UpdateWorld(GameWorld.human);
    }

    public void UpdateWorld(GameWorld world) {
        if (sc != null) {
            Destroy(sc);
        }
        string moveDesc="Move yourself around", switchDesc="Change yourself", attackDesc = null;
        switch (world) {
            case GameWorld.dino:
                attackDesc = "Attack!";
                break;
            case GameWorld.space:
                attackDesc = "Fly up";
                break;
            case GameWorld.race:
                attackDesc = "Speed Boost";
                break;
        }
        if (attackDesc != null) {
            sc = ShowControls.CreateDocked(new ControlItem[] {
                new ControlItem(moveDesc, CustomDisplay.arrows),
                new ControlItem(switchDesc, KeyCode.LeftControl),
                new ControlItem(attackDesc, KeyCode.Space)
            });
        } else {
            sc = ShowControls.CreateDocked(new ControlItem[] {
                new ControlItem(moveDesc, CustomDisplay.arrows),
                new ControlItem(switchDesc, KeyCode.LeftControl)
            });
        }
        sc.size = ShowControlSize.Small;
        sc.position = ShowControlPosition.Bottom;
        sc.slideSpeed = 0;
        sc.showDuration = -1;
        sc.Show();
    }
}
