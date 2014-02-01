using UnityEngine;
using System.Collections;

public enum GameWorld { human, race, dino, space };

public class WorldState : MonoBehaviour {

    public GameWorld world;

	void Start () {
        world = GameWorld.human;
	}
}
