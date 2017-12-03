using UnityEngine;

public class SceneLoader : MonoBehaviour {

    public void StartGame() {
        Debug.Log(GameInformation.level);
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameInformation.level);
    }

    public void ReStartGame() {
        GameInformation.level = 1;
        //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        StartGame();
    }
}
