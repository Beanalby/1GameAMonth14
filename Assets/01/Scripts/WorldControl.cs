using UnityEngine;
using System.Collections;

public class WorldControl : MonoBehaviour {

    public GUISkin skin;
    public Texture2D buttonLight, buttonDark;
    private int vPadding = 30, hPadding = 15;
    public bool showingMenu = false;
    private bool needsCentered;
    private WorldState state;

    private Vector3 normalGravity, spaceGravity;

    public void Start() {
        state = GetComponent<WorldState>();
        normalGravity = Physics.gravity;
        spaceGravity = normalGravity / 4;
    }

    public void Update() {
        if (Input.GetButtonDown("Fire2")) {
            showingMenu = true;
            needsCentered = true;
        }
        if (showingMenu) {
            if (needsCentered
                    && Input.GetAxisRaw("Horizontal") == 0
                    && Input.GetAxisRaw("Vertical") == 0) {
                        needsCentered = false;
            }
        }
        if (Input.GetButtonUp("Fire2")) {
            showingMenu = false;
            if (!needsCentered) {
                if (Input.GetAxisRaw("Horizontal") == -1) {
                    HandleUpdateWorld(GameWorld.race);
                } else if (Input.GetAxisRaw("Horizontal") == 1) {
                    HandleUpdateWorld(GameWorld.dino);
                } else if (Input.GetAxisRaw("Vertical") == -1) {
                    HandleUpdateWorld(GameWorld.space);
                } else if (Input.GetAxisRaw("Vertical") == 1) {
                    HandleUpdateWorld(GameWorld.human);
                }
            }
        }
    }

    private void HandleUpdateWorld(GameWorld newWorld) {
        state.world = newWorld;
        foreach (WorldSwitcher obj in GameObject.FindObjectsOfType<WorldSwitcher>()) {
            obj.WorldSwitched(newWorld);
        }
        SendMessage("UpdateWorld", newWorld);
        if (newWorld == GameWorld.space) {
            Physics.gravity = spaceGravity;
        } else {
            Physics.gravity = normalGravity;
        }
    }

    public void OnGUI() {
        GUI.skin = skin;
        DrawMenu();
    }

    private void DrawMenu() {
        if (!showingMenu) {
            return;
        }
        Rect raceRect = new Rect(
            Screen.width / 2 - (buttonLight.width + hPadding),
            Screen.height / 2 - buttonLight.height / 2,
            buttonLight.width, buttonLight.height);
        Rect dinoRect = new Rect(
            Screen.width / 2 + hPadding,
            Screen.height / 2 - buttonLight.height / 2,
            buttonLight.width, buttonLight.height);
        Rect humanRect = new Rect(
            Screen.width / 2 - buttonLight.width / 2,
            Screen.height / 2 - (buttonLight.height + vPadding),
            buttonLight.width, buttonLight.height);
        Rect spaceRect = new Rect(
            Screen.width / 2 - buttonLight.width / 2,
            Screen.height / 2 + vPadding,
            buttonLight.width, buttonLight.height);

        Texture2D tex;

        if (!needsCentered && Input.GetAxisRaw("Horizontal") == -1) {
            tex = buttonLight;
        } else {
            tex = buttonDark;
        }
        GUI.DrawTexture(raceRect, tex);
        GUI.Label(raceRect, "Race");
        if (!needsCentered && Input.GetAxisRaw("Horizontal") == 1) {
            tex = buttonLight;
        } else {
            tex = buttonDark;
        }
        GUI.DrawTexture(dinoRect, tex);
        GUI.Label(dinoRect, "Dino");
        if (!needsCentered && Input.GetAxisRaw("Vertical") == -1) {
            tex = buttonLight;
        } else {
            tex = buttonDark;
        }
        GUI.DrawTexture(spaceRect, tex);
        GUI.Label(spaceRect, "Space");
        if (!needsCentered && Input.GetAxisRaw("Vertical") == 1) {
            tex = buttonLight;
        } else {
            tex = buttonDark;
        }
        GUI.DrawTexture(humanRect, tex);
        GUI.Label(humanRect, "Human");
    }
}
