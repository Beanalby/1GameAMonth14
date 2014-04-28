using UnityEngine;
using System.Collections;

public class gbBeebulburp : MonoBehaviour {

    public void PlayerEntered(gbPlayer player) {
        if(player.ShipParts >= 3) {
            Debug.Log("+++ You win!");
            Destroy(gameObject);
        }
    }
}
