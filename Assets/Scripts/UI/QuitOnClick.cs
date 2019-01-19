using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuitOnClick : MonoBehaviour 
{

	// Use this for initialization
	void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener( () => {
            Application.Quit();
        });
    }
}
