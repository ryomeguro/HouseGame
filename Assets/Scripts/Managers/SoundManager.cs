using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioClip spawn,delete,error;

	public AudioSource bgm;
	public AudioSource se;

	void Awake(){
		bgm = GetComponent<AudioSource> ();
	}

	public void playMusic(AudioClip clip){
		se.PlayOneShot (clip);
	}

	public void playBGM(float delay){
		bgm.PlayDelayed (delay);
	}

	public void stopBGM(){
		bgm.Stop ();
	}

	public void spawnSound(){
		se.PlayOneShot (spawn);
	}

	public void deleteSound(){
		se.PlayOneShot (delete);
	}

	public void errorSound(){
		se.PlayOneShot (error);
	}

}
