using UnityEngine;
using System.Collections;

public class FlippyDriver : MonoBehaviour {

    public AudioClip scoreSound;
    public GUISkin skin;
    public Texture2D scoreBg, buttonBg, buttonHighlightBg;
    private int score = 0;

    private const int STAGE_WIDTH = FlippyBackgroundMover.BackgroundWidth * 2;
    private int numStages = 3;
    private int stage = 0;
    public int Stage {
        get { return stage; }
    }
    public int NextStage {
        get { return (stage+1) % numStages; }
    }
    private int nextTransition = STAGE_WIDTH - FlippyBackgroundMover.BackgroundWidth / 2;
    public int NextTransition {
        get { return nextTransition; }
    }

    GUIStyle scoreLabelPlaying, scoreLabelEnd, scoreValuePlaying, scoreValueEnd,
        retryButton;
    FlippyPlayer player;

    private Interpolate.Function ease1 = Interpolate.Ease(Interpolate.EaseType.EaseOutCubic);
    private Interpolate.Function ease2 = Interpolate.Ease(Interpolate.EaseType.EaseInCubic);

    float scoreStart = -1, scoreDuration = .25f;

    public void Start() {
        player = GameObject.Find("FlippyPlayer").GetComponent<FlippyPlayer>();

        scoreLabelPlaying = new GUIStyle(skin.label);
        scoreLabelPlaying.alignment = TextAnchor.MiddleLeft;
        scoreLabelEnd = new GUIStyle(scoreLabelPlaying);
        scoreLabelEnd.fontSize = (int)(scoreLabelEnd.fontSize * 1.5f);
        scoreValuePlaying = new GUIStyle(skin.label);
        scoreValuePlaying.alignment = TextAnchor.MiddleRight;
        scoreValueEnd = new GUIStyle(scoreValuePlaying);
        scoreValueEnd.fontSize = (int)(scoreValueEnd.fontSize * 1.5f);
        retryButton = new GUIStyle(skin.label);
        retryButton.alignment = TextAnchor.MiddleCenter;
    }
    public void PipePassed(FlippyPipes pipes) {
        AudioSource.PlayClipAtPoint(scoreSound, transform.position);
        score++;
        scoreStart = Time.time;
    }
    public void Update() {
        if(Input.GetKeyDown(KeyCode.D)) {
            PipePassed(null);
        }
        if(player.transform.position.x > nextTransition) {
            stage = (stage + 1) % numStages;
            nextTransition += STAGE_WIDTH;
        }
    }

    public void OnGUI() {
        GUIStyle scoreLabel, scoreValue;
        GUI.skin = skin;
        Rect scoreRect;
        // center the score box if they're not playing
        if(player.IsPlaying) {
            float yPos = 10;
            if(scoreStart != -1) {
                float percent = (Time.time - scoreStart) / scoreDuration;
                if(percent >= 1) {
                    scoreStart = -1;
                } else if(percent < .5f) {
                    yPos = ease1(10, 10, percent, .5f);
                } else {
                    yPos = ease2(20, -10, percent - .5f, .5f);
                }
            }
            scoreRect = new Rect(10, yPos, scoreBg.width, scoreBg.height);
            scoreLabel = scoreLabelPlaying;
            scoreValue = scoreValuePlaying;
        } else {
            scoreRect = new Rect(Screen.width / 2 - scoreBg.width,
                Screen.height / 2 - scoreBg.height,
                2*scoreBg.width, 2*scoreBg.height);
            scoreLabel = scoreLabelEnd;
            scoreValue = scoreValueEnd;
        }
        GUI.DrawTexture(scoreRect, scoreBg);
        int padding = 5;
        scoreRect.xMin += padding;
        scoreRect.xMax -= padding;
        scoreRect.yMin += padding;
        scoreRect.yMax -= padding;
        GUI.Label(scoreRect, "Score: ", scoreLabel);
        GUI.Label(scoreRect, score.ToString(), scoreValue);

        if(!player.IsPlaying) {
            Rect restartRect = new Rect(Screen.width / 2 - buttonBg.width / 2,
                Screen.height / 2 + 75, buttonBg.width, buttonBg.height);
            Texture2D tex;
            Vector3 pos = Input.mousePosition;
            pos.y = Screen.height - pos.y;
            if(restartRect.Contains(pos)) {
                tex = buttonHighlightBg;
            } else {
                tex = buttonBg;
            }
            GUI.DrawTexture(restartRect, tex);
            if(GUI.Button(restartRect, "PLAY AGAIN", retryButton)) {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }
}
