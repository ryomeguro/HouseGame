using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	AudioSource bgm;

	void Awake(){
		bgm = GetComponent<AudioSource> ();
	}

	public void playMusic(AudioClip clip){
		bgm.PlayOneShot (clip);
	}

	public void playBGM(float delay){
		bgm.PlayDelayed (delay);
	}

	public void stopBGM(){
		bgm.Stop ();
	}

}
