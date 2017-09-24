﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour//Singleton<GameManager> //MonoBehaviour //Singleton<GameManager>
{
	// Use this for initialization
	public ConstantsAndUtils utils;
	public uint mCounterOfAsteriodsDestroyedByPlayer = 0;
	public readonly uint INITIAL_LIVES = 5;
	public uint mLives;

	[SerializeField]
	private Camera camera;

	void SubscribeToEvents()
	{
		Messaging.AddListener(GameEvent.AsteroidFallen, OnAsteroidFallen, Messaging.Filter.All);
		Messaging.AddListener(GameEvent.AsteroidHittedByPlayer, OnAsteroidHittedByPlayer, Messaging.Filter.All);
	}
	void UnSubscribeToEvents()
	{
		Messaging.RemoveListener(GameEvent.AsteroidFallen, OnAsteroidFallen);
		Messaging.RemoveListener(GameEvent.AsteroidHittedByPlayer, OnAsteroidHittedByPlayer);
	}

	void OnAsteroidFallen(GameObject sender, GameObject receiver, GameEvent gameEvent, object param)
	{
		mLives--;
		if (mLives == 0)
		{
			//TODO - Actions for GAME OVER 
			// Send EndStage
		}
	}

	void OnAsteroidHittedByPlayer(GameObject sender, GameObject receiver, GameEvent gameEvent, object param)
	{
		mCounterOfAsteriodsDestroyedByPlayer++;
		//Update counter in screen.

	}

	void Start ()
	{
		App.GM = this;
		utils = new ConstantsAndUtils(camera);

		SetLevelProperties();
		SubscribeToEvents();
	}

	void SetLevelProperties()
	{
		mLives = INITIAL_LIVES;
		mCounterOfAsteriodsDestroyedByPlayer = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDisable()
	{
		UnSubscribeToEvents();
	}
}