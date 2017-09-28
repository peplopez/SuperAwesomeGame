using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour {

	// Use this for initialization
	Animator mAnimatorComponent;
	AnimatorStateInfo mInitialStateInfo;
	AnimatorStateInfo mAlarmStateInfo;

	public Camera cam;

	void Start () {
		mAnimatorComponent = GetComponent<Animator>();

		Vector3 screenPos = cam.ScreenToWorldPoint(new Vector3(400, 400, 0));
		
			//WorldToScreenPoint(this.transform.position);

		Debug.Log("target is " + screenPos.x + " pixels from the left");
		//this.transform.position = screenPos;

		SubscribeToEvents();
		//RestartClock();

		mInitialStateInfo = mAnimatorComponent.GetCurrentAnimatorStateInfo(0);
		mAlarmStateInfo = mAnimatorComponent.GetNextAnimatorStateInfo(0);
		/*
		Debug.Log("state current:" + mInitialStateInfo);
		Debug.Log("state next :" + mAlarmStateInfo);

		Debug.Log("fullpathcurrent: " + mInitialStateInfo.fullPathHash.ToString());
		Debug.Log("fullpathnext: " + mAlarmStateInfo.fullPathHash.ToString());

		Debug.Log("fullpathcurrentAnim: " + mAnimatorComponent.GetCurrentAnimatorClipInfo(0)[0].clip.name);
		Debug.Log("fullpathnextAnim: " + mAnimatorComponent.GetNextAnimatorClipInfo(0)[0].clip.name);
		*/
	}

	void RestartClock()
	{
		mAnimatorComponent.Play("ClockRunning");
		App.GM.utils.Fade(true, Color.grey);
		//mImageFade.Fade();
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
		RestartClock();
	}

	void OnEndStage(GameObject sender, GameObject receiver, GameEvent gameEvent, object param)
	{
		mAnimatorComponent.StopPlayback();
	}

	// Update is called once per frame
	void Update ()
	{
		if (mAnimatorComponent.IsInTransition(0))
		{
			if (mAnimatorComponent.GetCurrentAnimatorStateInfo(0).fullPathHash == mInitialStateInfo.fullPathHash)
			{
				Messaging.Send(gameObject, null, GameEvent.EndStage, null);
				Debug.Log("Alarm sounds");
			}
			else
			{
				Messaging.Send(gameObject, null, GameEvent.StartStage, null);
				Debug.Log("Restarting Alarm");
				//GetComponent<ImageFade>().Fade();
				//RestartClock
			}
		}
	}
}