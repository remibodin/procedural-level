using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class LoadSceneOnClick : MonoBehaviour
{
    public string sceneName;

    void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener( () => {
            GameMode.Instance.LoadScene(sceneName);
        });
    }
}