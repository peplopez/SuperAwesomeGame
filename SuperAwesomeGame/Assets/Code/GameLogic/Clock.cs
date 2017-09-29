using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour {

	// Use this for initialization
	Animator mAnimatorComponent;
	AnimatorStateInfo mInitialStateInfo;
	AudioSource[] mAudioSourceComponent;

	void Start () {
		mAnimatorComponent = GetComponent<Animator>();
		
		SubscribeToEvents();
		mInitialStateInfo = mAnimatorComponent.GetCurrentAnimatorStateInfo(0);
		mAudioSourceComponent = GetComponents<AudioSource>();
	}

	void PlayTick()
	{		
		mAudioSourceComponent[0].Play();
	}

	void PlayRing()
	{
		mAudioSourceComponent[1].Play();
	}

	void RestartClock()
	{
		mAnimatorComponent.speed = 1;
		mAnimatorComponent.Play("ClockRunning",0,0);
		App.GM.utils.Fade(true, Color.grey);
	}	

	void SubscribeToEvents()
	{
		Messaging.AddListener(GameEvent.StartStage, OnStartStage, Messaging.Filter.All);
		Messaging.AddListener(GameEvent.EndStage, OnEndStage, Messaging.Filter.All);
	}
	void UnSubscribeToEvents()
	{
		Messaging.RemoveListener(GameEvent.StartStage, OnStartStage);
		Messaging.RemoveListener(GameEvent.EndStage, OnEndStage);
	}

	void OnStartStage(GameObject sender, GameObject receiver, GameEvent gameEvent, object param)
	{
		//Only attend to other's gameobject events of this type
		if (sender!=this.gameObject)
			RestartClock();
	}

	void OnEndStage(GameObject sender, GameObject receiver, GameEvent gameEvent, object param)
	{
		//Only attend to other's gameobject events of this type
		if (sender != this.gameObject)
			mAnimatorComponent.speed = 0;		
	}

	void Update ()
	{
	//The clock, with its animation, controls the 60 seconds and send the notification to the GameManager.
		if (mAnimatorComponent.IsInTransition(0))
		{
			if (mAnimatorComponent.GetCurrentAnimatorStateInfo(0).fullPathHash == mInitialStateInfo.fullPathHash)
			{
				Messaging.Send(gameObject, null, GameEvent.EndStage, null);
				Debug.Log("Alarm sounds. Player Wins!");
			}
			else
			{
				//Stop the clock Animation. The player choose now when to restart the game. Clock waits stopped
				mAnimatorComponent.speed = 0;
				Debug.Log("Stopping clock until player restart game");
			}
		}
	}
}