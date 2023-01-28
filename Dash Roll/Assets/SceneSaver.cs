using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSaver : MonoBehaviour
{
	static int[][] objectStatus = new int[3][]; //set array size to number of scenes
	const int ACTIVE = 0, DISABLED = 1, DESTROYED = 2;

	static bool[] firstSceneLoad = new bool[3]; //set array size to number of scenes

	[SerializeField] GameObject[] sceneObjects;

	public static SceneSaver self;

    private void Awake()
    {
		self = GetComponent<SceneSaver>();
    }

    void Start()
	{
		if (!firstSceneLoad[GameManager.GetCurrentScene()])
		{
			firstSceneLoad[GameManager.GetCurrentScene()] = true;
			objectStatus[GameManager.GetCurrentScene()] = new int[sceneObjects.Length];
		}
		else
		{
			int status;
			for (int i = 0; i < sceneObjects.Length; i++)
			{
				status = objectStatus[GameManager.GetCurrentScene()][i];

				if (status == ACTIVE)
                {
					sceneObjects[i].SetActive(true);
				}
				else if (status == DISABLED)
                {
					sceneObjects[i].SetActive(false);
				}
				else if (status == DESTROYED)
                {
					Destroy(sceneObjects[i]);
				}
			}
		}
	}

	public void UpdateStatus()
    {
		for (int i = 0; i < sceneObjects.Length; i++)
		{
			if (!sceneObjects[i])
			{
				objectStatus[GameManager.GetCurrentScene()][i] = DESTROYED;
			}
			else if (sceneObjects[i].activeSelf)
			{
				objectStatus[GameManager.GetCurrentScene()][i] = ACTIVE;
			}
			else if (!sceneObjects[i].activeSelf)
			{
				objectStatus[GameManager.GetCurrentScene()][i] = DISABLED;
			}
		}
	}
}
