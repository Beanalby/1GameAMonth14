using UnityEngine;
using System.Collections;

/// <summary>
/// Instantiates two copies of a background, and moves the later one ahead
/// once the player has 
/// </summary>
public class FlippyBackgroundMover : MonoBehaviour {

    public GameObject bgPrefab;

    private Transform bgCurrent, bgOther, player;
    private float bgWidth = 44;

    public void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // put the current bg just ahead of the player, to invoke the swap soon
        bgCurrent = (GameObject.Instantiate(bgPrefab) as GameObject).transform;
        bgCurrent.transform.parent = transform;
        bgCurrent.transform.position = new Vector2(player.position.x + 1, 0);

        // bgOther will get placed ahead of bgCurrent in a second
        bgOther = (GameObject.Instantiate(bgPrefab) as GameObject).transform;
        bgOther.transform.parent = transform;
    }

    public void Update() {
        // if the player is past the midpoint of the current background,
        // move the other one ahead of the current one
        if(player.position.x > bgCurrent.position.x) {
            bgOther.position = new Vector2(bgCurrent.position.x + bgWidth, 0);
            // swap current & other
            Transform t = bgCurrent;
            bgCurrent = bgOther;
            bgOther = t;
        }
    }
}
