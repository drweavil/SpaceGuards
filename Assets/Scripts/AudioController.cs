using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
	public static AudioController instance;
	public float currentScale = 1;
	public AudioSource loseExplode;

	public AudioSource leftType1;
	public AudioSource rightType1;

	public AudioSource leftType2;
	public AudioSource rightType2;

	public AudioSource leftType3;
	public AudioSource rightType3;

	public AudioSource leftType4;
	public AudioSource rightType4;

	public AudioSource leftType7;
	public AudioSource rightType7;

	public List<AudioSource> scaledSounds = new List<AudioSource>();

	public Dictionary<int, List<AudioSource>> planetSongs = new Dictionary<int, List<AudioSource>> ();
	public List<AudioSource> planet1Songs = new List<AudioSource> ();
	public List<AudioSource> planet2Songs = new List<AudioSource> ();
	public List<AudioSource> planet3Songs = new List<AudioSource> ();
	public List<AudioSource> planet4Songs = new List<AudioSource> ();
	public List<AudioSource> planet5Songs = new List<AudioSource> ();
	public List<AudioSource> planet6Songs = new List<AudioSource> ();

	public AudioSource currentSong;

	void Awake(){
		instance = this;
		StartCoroutine(GameController.ActionAfterFewFramesCoroutine (1, () => {
			planetSongs.Add(1, planet1Songs);
			planetSongs.Add(2, planet2Songs);
			planetSongs.Add(3, planet3Songs);
			planetSongs.Add(4, planet4Songs);
			planetSongs.Add(5, planet5Songs);
			planetSongs.Add(6, planet6Songs);

			scaledSounds.Add(currentSong);
		}));
	}


	public void SetTimeScale(float scale){
		if (scale < 0) {
			scale = 0;
		}else if(scale > 1){
			scale = 1;
		}
		currentScale = scale;
		foreach (AudioSource audio in scaledSounds) {
			audio.pitch = scale;
		}
	}



	public void SetSongByPlanet(int planetID){
		int index = scaledSounds.IndexOf (currentSong);
		List<AudioSource> songsList = planetSongs [planetID];
		currentSong = songsList[Random.Range(0, songsList.Count)];
		currentSong.pitch = 1;
		currentSong.Play ();
		scaledSounds [index] = currentSong;
	}
}
