using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyPanelManager : MonoBehaviour {

	int houseBuiltCost;
	int houseMaintenanceCost;
	int treeBuiltCost;
	int treeIncome;

	bool slotEndFlg = false;

	public UIManager uiManager;
	public SoundManager soundManager;

	public AudioClip dramroll;

	public Text houseBuildText, houseMaintainText, treeBuiltText, treeIncomeText;

	Animator animator;

	void Awake(){
		animator = GetComponent<Animator> ();
	}

	public void SetProperty(int houseBuiltCost, int houseMaintenanceCost, int treeBuiltCost, int treeIncome){
		this.houseBuiltCost = houseBuiltCost;
		this.houseMaintenanceCost = houseMaintenanceCost;
		this.treeBuiltCost = treeBuiltCost;
		this.treeIncome = treeIncome;
	}

	public IEnumerator SlotStart(){
		soundManager.stopBGM ();
		uiManager.EnablePanel (animator);
		slotEndFlg = false;
		StartCoroutine ("SlotChange");
		yield return new WaitForSeconds (1f);
		soundManager.playMusic (dramroll);
		yield return new WaitForSeconds (2f);
		StartCoroutine ("SlotStop");
		soundManager.playBGM (2f);
	}

	IEnumerator SlotChange(){
		while (!slotEndFlg) {
			TextChange (Random.Range (0, 999), Random.Range (0, 999), Random.Range (0, 999), Random.Range (0, 999));
			yield return null;
		}
	}

	void TextChange(int a, int b, int c, int d){
		houseBuildText.text = "家の建設費:$" + a;
		houseMaintainText.text = "家の維持費:$" + b;
		treeBuiltText.text = "木の設置費:$" + c;
		treeIncomeText.text = "木の収入:$" + d;
	}

	IEnumerator SlotStop(){
		slotEndFlg = true;
		TextChange (houseBuiltCost, houseMaintenanceCost, treeBuiltCost, treeIncome);
		yield return new WaitForSeconds (3f);
		StartCoroutine ("SlotEnd");
	}

	void SlotEnd(){
		uiManager.DisablePanel (animator);
		uiManager.UIUpdate();
		uiManager.ClearLog();
	}
}
