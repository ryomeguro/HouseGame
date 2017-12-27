using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	public GameObject RulePanel;
	public GameObject[] Pages;
	public Button prevButton, nextButton;

	Scene mainScene;

	int pageIndex = 0;

	// Use this for initialization
	void Start () {
		mainScene = SceneManager.GetSceneByName ("Main");
		//SceneManager.LoadSceneAsync ("Main");
	}

	public void OnStartButtonPushed(){
		StartCoroutine ("GoToMain");
	}

	IEnumerator GoToMain(){
		SceneManager.LoadSceneAsync ("Main");
		while (!mainScene.isLoaded) {
			yield return null;
		}
		SceneManager.SetActiveScene (mainScene);
	}

	public void OnPushHowToPlay(){
		prevButton.interactable = false;
		nextButton.interactable = true;
		pageIndex = 0;
		Pages [0].SetActive (true);
		RulePanel.SetActive (true);
	}

	public void OnPushPrevButton(){
		Pages [pageIndex - 1].SetActive (true);
		Pages [pageIndex].SetActive (false);
		pageIndex--;
		if (pageIndex == 0) {
			prevButton.interactable = false;
		}
		nextButton.interactable = true;
	}

	public void OnPushNextButton(){
		Pages [pageIndex + 1].SetActive (true);
		Pages [pageIndex].SetActive (false);
		pageIndex++;
		if (pageIndex == Pages.Length - 1) {
			nextButton.interactable = false;
		}
		prevButton.interactable = true;
	}

	public void OnPushBack(){
		RulePanel.SetActive (false);
		Pages [pageIndex].SetActive (false);
		RulePanel.SetActive (false);
	}

}
