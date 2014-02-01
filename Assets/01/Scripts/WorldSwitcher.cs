using UnityEngine;
using System.Collections;

public class WorldSwitcher : MonoBehaviour {

    public GameObject humanVersion, raceVersion, dinoVersion, spaceVersion;

    public void WorldSwitched(GameWorld world) {
        GameObject current = transform.parent.gameObject;
        transform.parent = null;
        transform.position = current.transform.position;
        transform.rotation = current.transform.rotation;
        Destroy(current);
        GameObject prefab = null;
        switch (world) {
            case GameWorld.human:
                prefab = humanVersion; break;
            case GameWorld.race:
                prefab = raceVersion; break;
            case GameWorld.dino:
                prefab = dinoVersion; break;
            case GameWorld.space:
                prefab = spaceVersion; break;
        }
        current = Instantiate(prefab) as GameObject;
        current.transform.position = transform.position;
        current.transform.rotation = transform.rotation;
        if (current.tag == "Player") {
            // cancel out any rolling we may have encountered
            current.transform.rotation = Quaternion.Euler(
                new Vector3(0, transform.rotation.eulerAngles.y, 0));
        }
        transform.parent = current.transform;
    }
}
