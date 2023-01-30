using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSaver : MonoBehaviour
{
	static int[][][] objectStatus = new int[2][][];
	const int ACTIVE = 0, DISABLED = 1, DESTROYED = 2;
	static int saveMode;
	public const int OBELISK = 0, SCENE_EDGE = 1;

	static bool[] firstSceneLoad;
	static bool firstGameLoad;
	static int scenes = 3; //set equal to number of scenes

	[SerializeField] GameObject[] sceneObjects;

	public static SceneSaver self;

    private void Awake()
    {
		self = GetComponent<SceneSaver>();

		if (!firstGameLoad)
        {
			objectStatus[0] = new int[scenes][];
			objectStatus[1] = new int[scenes][];
			firstSceneLoad = new bool[scenes];

			firstGameLoad = true;
        }
    }

    void Start()
	{
		if (!firstSceneLoad[GameManager.GetCurrentScene()])
		{
			firstSceneLoad[GameManager.GetCurrentScene()] = true;
			objectStatus[0][GameManager.GetCurrentScene()] = new int[sceneObjects.Length];
			objectStatus[1][GameManager.GetCurrentScene()] = new int[sceneObjects.Length];
		}
		else
		{
			int status;
			for (int i = 0; i < sceneObjects.Length; i++)
			{
				status = objectStatus[saveMode][GameManager.GetCurrentScene()][i];

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

	public void UpdateStatus(int mode)
    {
		saveMode = mode;
		for (int i = 0; i < sceneObjects.Length; i++)
		{
			if (!sceneObjects[i])
			{
				objectStatus[saveMode][GameManager.GetCurrentScene()][i] = DESTROYED;
			}
			else if (sceneObjects[i].activeSelf)
			{
				objectStatus[saveMode][GameManager.GetCurrentScene()][i] = ACTIVE;
			}
			else if (!sceneObjects[i].activeSelf)
			{
				objectStatus[saveMode][GameManager.GetCurrentScene()][i] = DISABLED;
			}
		}
	}
}
