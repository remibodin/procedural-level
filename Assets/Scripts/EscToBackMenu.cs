using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscToBackMenu : MonoBehaviour 
{

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GameMode.Instance.LoadScene("Menu");
		}
	}
}
