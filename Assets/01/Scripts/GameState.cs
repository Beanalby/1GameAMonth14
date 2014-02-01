using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
    private int pickups = 0;
    bool finished = false;
    public int Pickups {
        get { return pickups; }
    }

    public void AddPickup() {
        pickups++;
        // check if there are any pickups left
        foreach(Pickup p in GameObject.FindObjectsOfType<Pickup>()) {
            if (!p.pickedUp) {
                return;
            }
        }
        // nothing left to be picked up
        finished = true;
    }

    public void OnGUI() {
        if (!finished) {
            GUI.Label(new Rect(10, 10, 200, 100), "Juice boxes: " + pickups);
        } else {
            GUI.Label(new Rect(10, 10, 200, 100), "All Juice boxes found!");
        }
    }
}