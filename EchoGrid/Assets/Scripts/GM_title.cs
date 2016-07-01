﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GM_title : MonoBehaviour {

	Vector2 touchOrigin = -Vector2.one;
	float touchTime = 0f;
	private float minSwipeDist = 100f;
	AudioClip swipeAhead;
	AudioClip swipeRight;
	AudioClip swipeLeft;
	AudioClip to_tutorial;
	AudioClip to_main;
	AudioClip[] clips;
	int cur_clip = 0;
	int total_clip = 2;
	float time_interval = 2.0f;

	// Use this for initialization
	void Start () {
		GameObject.Find ("GameMode").GetComponent <GameMode>().init ();
		//load instruction clips
		clips = new AudioClip[total_clip];
		clips[0] = Resources.Load ("instructions/Welcome to Echo Adventure") as AudioClip;
		clips[1] = Resources.Load ("instructions/Swipe right to play the game or swipe left to enter the tutorial") as AudioClip;

		swipeAhead = Resources.Load("fx/swipe-ahead") as AudioClip;
		swipeRight = Resources.Load("fx/swipe-right") as AudioClip;
		swipeLeft = Resources.Load("fx/swipe-left") as AudioClip;
		to_tutorial = Resources.Load ("instructions/Welcome to the tutorial") as AudioClip;
		to_main = Resources.Load ("instructions/Swipe right to continue from last time or double tap to start a new game") as AudioClip;
	}

	void play_audio(){
		if (SoundManager.instance.PlayVoice (clips[cur_clip])) {
			cur_clip += 1;
			if (cur_clip >= total_clip)
				cur_clip = 0;
		}
	}

	// Update is called once per frame
	void Update () {
		play_audio ();

		//Check if we are running either in the Unity editor or in a standalone build.
		#if UNITY_STANDALONE || UNITY_WEBPLAYER

		//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
		if (Input.GetKeyUp(KeyCode.RightArrow)) {
			GameMode.gamemode = GameMode.Game_Mode.CONTINUE;
			SceneManager.LoadScene("Main_pre");
			SoundManager.instance.PlayVoice(to_main, true);
			//SoundManager.instance.PlaySingle(swipeRight);
		} else if (Input.GetKeyUp(KeyCode.LeftArrow)) {
			GameMode.gamemode = GameMode.Game_Mode.TUTORIAL;
			SceneManager.LoadScene("Main");
			SoundManager.instance.PlayVoice(to_tutorial, true);
			//SoundManager.instance.PlaySingle(swipeLeft);
		} else if (Input.GetKeyUp("f")) {
			//SceneManager.LoadScene("Main");
			//SoundManager.instance.PlaySingle(swipeAhead);
		}

		//Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
		#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

		float TOUCH_TIME = 0.05f;

		//Check if Input has registered more than zero touches
		int numTouches = Input.touchCount;

		if (numTouches > 0) {
			//Store the first touch detected.
			Touch myTouch = Input.touches[0];

			//Check if the phase of that touch equals Began
			if (myTouch.phase == TouchPhase.Began){
			//If so, set touchOrigin to the position of that touch
			touchOrigin = myTouch.position;
			touchTime = Time.time;
			}

			//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
			else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0) {
				//Set touchEnd to equal the position of this touch
				Vector2 touchEnd = myTouch.position;

				//Calculate the difference between the beginning and end of the touch on the x axis.
				float x = touchEnd.x - touchOrigin.x;

				//Calculate the difference between the beginning and end of the touch on the y axis.
				float y = touchEnd.y - touchOrigin.y;

				//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
				touchOrigin.x = -1;

				//Check if the difference along the x axis is greater than the difference along the y axis.
				if (Mathf.Abs(x) > Mathf.Abs(y) && Mathf.Abs(x) >= minSwipeDist)
				{
					//If x is greater than zero, set horizontal to 1, otherwise set it to -1
					if (x > 0) {//RIGHT
						GameMode.gamemode = GameMode.Game_Mode.CONTINUE;
						SceneManager.LoadScene("Main_pre");
						SoundManager.instance.PlayVoice(to_main, true);
						//SoundManager.instance.PlaySingle(swipeRight);
					} else {//LEFT
						GameMode.gamemode = GameMode.Game_Mode.TUTORIAL;
						SceneManager.LoadScene("Main");
						SoundManager.instance.PlayVoice(to_tutorial, true);
					}
				} else if (Mathf.Abs(y) > Mathf.Abs(x) && Mathf.Abs(y) >= minSwipeDist) {
					//If y is greater than zero, set vertical to 1, otherwise set it to -1
					if (y > 0) {//FRONT
						//SoundManager.instance.PlaySingle(swipeAhead);
					} else {//BACK
						//SoundManager.instance.PlaySingle(swipeAhead);
					}
				} else if (Mathf.Abs(Time.time - touchTime) > TOUCH_TIME) {
					if (numTouches == 2){
						//GameMode.gamemode = GameMode.Game_Mode.MAIN;
						//SceneManager.LoadScene("Main");
					}
					else{}
				}
			}
		}
		#endif //End of mobile platform dependendent compilation section started above with #elif	
	}
}
