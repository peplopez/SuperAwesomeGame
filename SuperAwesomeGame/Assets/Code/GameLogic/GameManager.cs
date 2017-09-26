using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	// Use this for initialization
	public ConstantsAndUtils utils;
	public uint mCounterOfAsteriodsDestroyedByPlayer = 0;
	public readonly uint INITIAL_LIVES = 5;
	public uint mLives;
	public Text mScore;
	public AsteroidSpawner mAsteroidSpawner;

	[SerializeField]
	private Camera mCamera;
	[SerializeField]
	private Canvas mCanvas;

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
		mScore.text = mCounterOfAsteriodsDestroyedByPlayer.ToString();
		//Update counter in screen.

	}

	void Start ()
	{
		App.GM = this;
		utils = new ConstantsAndUtils(mCamera, mCanvas);

		SetLevelProperties();
		SubscribeToEvents();
		mAsteroidSpawner.gameObject.SetActive(true);
		Messaging.Send(gameObject, null, GameEvent.StartStage, null);
		//mAsteroidSpawner.enabled = true;
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