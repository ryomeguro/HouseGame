using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTreeRandomAnimation : MonoBehaviour {

	public float defaultduration;

	float offset;
	float time;
	float duration;

	Animator animator;
	// Use this for initialization
	void Start () {
		offset = Random.Range(0f,defaultduration);
		time = offset;
		animator = gameObject.GetComponent<Animator>();
		setDuration ();
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (duration < time) {
			//Debug.Log ("Bounce!");
			animator.SetTrigger("Bounce");
			time = 0;
			setDuration();
		}
	}

	void setDuration(){
		duration = defaultduration + Random.Range (-3f, 3);
	}
}
