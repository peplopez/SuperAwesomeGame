using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour {

	// Use this for initialization
	Animator mAnimatorComponent;
	void Start () {
		mAnimatorComponent = GetComponent<Animator>();

		SubscribeToEvents();
		RestartClock();
	}

	void RestartClock()
	{
		mAnimatorComponent.Play("ClockRunning");
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

	}

	// Update is called once per frame
	void Update () {
		
	}
}
