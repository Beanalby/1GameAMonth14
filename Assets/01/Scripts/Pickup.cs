using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

    public bool pickedUp = false;

    public void OnTriggerEnter(Collider other) {
        if(other.tag != "Player") {
            return;
        }
        pickedUp = true;
        GameObject.Find("WorldState").GetComponent<GameState>().AddPickup();
        Destroy(gameObject);
    }
}
