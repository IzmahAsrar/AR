using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModel_Handler : MonoBehaviour {

	[Header("Level Objectives Objectives")]
	public string primaryObjective;
	public string[] secondaryObjectives;

	[Header("Cut Scene Init")]
	public bool containsCutScene;
	public bool skipCutScene;
	public float cutSceneDuration;
	public UnityEngine.Events.UnityEvent onCutSceneEndEvent;

	[Header("Tmer per level aloted")]
	public float timeAloted;
	public TimerScript timerScript;

	void Start()
	{
		timerScript.time = timeAloted;
		timerScript.timerText = GameplayScript_Handler.gsh.timerText;
	}


	public IEnumerator cutSceneCalled()
	{
		yield return new WaitForSeconds (cutSceneDuration);
		if (!skipCutScene) {
			GameplayScript_Handler.gsh.showButtons ();
			GameplayScript_Handler.gsh.ObjectiveDialoug ();
			onCutSceneEndEvent.Invoke ();
		}
	}

	public void skipCutSceneHandler()
	{
		skipCutScene = true;
		GameplayScript_Handler.gsh.showButtons ();
		GameplayScript_Handler.gsh.ObjectiveDialoug ();
		onCutSceneEndEvent.Invoke ();
	}
}
