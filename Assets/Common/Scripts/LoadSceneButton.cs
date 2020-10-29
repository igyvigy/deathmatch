using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public string SceneName;

    public void LoadTargetScene()
    {
        SceneManager.LoadSceneAsync(SceneName);
    }
}