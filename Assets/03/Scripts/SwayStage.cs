using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SwayStage : MonoBehaviour {

    private static SwayStage instance;
    public static SwayStage Instance {
        get { return instance; }
    }
    
    public string NextStage;
    public GUISkin skin;

    private bool isComplete = false;

    public void Awake() {
        if(instance != null) {
            Debug.LogError("Already has a SwayStage (" + instance.name + ")");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void OnGUI() {
        GUI.skin = skin;
        if(isComplete) {
            //GUI.Label(new Rect(0, 100, Screen.width, 200), "Stage Complete");
        }
    }


    public void StageComplete() {
        isComplete = true;
        StartCoroutine(StartNextStage());
    }

    public void PlayerDied() {
        StartCoroutine(ReloadStage());
    }

    private IEnumerator ReloadStage() {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private IEnumerator StartNextStage() {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(NextStage);
    }

}
