using UnityEngine;
using System.Collections;

public class gbBeebulburp : MonoBehaviour {

    public GameObject flyPrefab;

    public void PlayerEntered(gbPlayer player) {
        if(player.ShipParts >= 3) {
            Destroy(gameObject);
            Instantiate(flyPrefab, transform.position, Quaternion.identity);
        }
    }
}
