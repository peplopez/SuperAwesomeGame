using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	// Use this for initialization
	public ConstantsAndUtils utils;
	public uint mCounterOfAsteriodsDestroyedByPlayer = 0;
	public readonly int INITIAL_LIVES = 5;
	public int mLivesCounter;
	public Text mScore;
	public Text mLives;
	public AsteroidSpawner mAsteroidSpawner;

	[SerializeField]
	private Camera mCamera;
	[SerializeField]
	private Canvas mCanvas;

	[SerializeField]
	private GameObject mStageClearObject;
	[SerializeField]
	private GameObject mGameoverObject;

	void SubscribeToEvents()
	{
		Messaging.AddListener(GameEvent.AsteroidFallen, OnAsteroidFallen, Messaging.Filter.All);
		Messaging.AddListener(GameEvent.AsteroidHittedByPlayer, OnAsteroidHittedByPlayer, Messaging.Filter.All);
		Messaging.AddListener(GameEvent.EndStage, OnEndStage, Messaging.Filter.All);
	}

	void UnSubscribeToEvents()
	{
		Messaging.RemoveListener(GameEvent.AsteroidFallen, OnAsteroidFallen);
		Messaging.RemoveListener(GameEvent.AsteroidHittedByPlayer, OnAsteroidHittedByPlayer);
		Messaging.RemoveListener(GameEvent.EndStage, OnEndStage);
	}

	void OnEndStage(GameObject sender, GameObject receiver, GameEvent gameEvent, object param)
	{
		if (mLivesCounter>0)
			mStageClearObject.SetActive(true);
	}

	void OnAsteroidFallen(GameObject sender, GameObject receiver, GameEvent gameEvent, object param)
	{
		mLivesCounter--;		
		if (mLivesCounter <= 0)
		{
			mLivesCounter = 0;
			utils.Fade(false, Color.red);
			//TODO - Actions for GAME OVER 
			// Send EndStage
			mAsteroidSpawner.BroadcastMessage("StopAsteroid");
			mGameoverObject.SetActive(true);
			Messaging.Send(gameObject, null, GameEvent.EndStage, null);
		}
		mLives.text = mLivesCounter.ToString();
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
		
	}

	public void SetLevelProperties()
	{
		mAsteroidSpawner.gameObject.SetActive(true);
		mGameoverObject.SetActive(false);
		mStageClearObject.SetActive(false);
		Messaging.Send(gameObject, null, GameEvent.StartStage, null);
		mLivesCounter = INITIAL_LIVES;
		mLives.text = mLivesCounter.ToString();
		mCounterOfAsteriodsDestroyedByPlayer = 0;
		mScore.text = mCounterOfAsteriodsDestroyedByPlayer.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDisable()
	{
		UnSubscribeToEvents();
	}
}