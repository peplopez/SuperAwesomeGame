using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	// Use this for initialization
	public ConstantsAndUtils utils;
	private uint mCounterOfAsteriodsDestroyedByPlayer = 0;
	public readonly int INITIAL_LIVES = 5;
	private int mLivesCounter;
	public Text mScore;
	public Text mLives;
	public AsteroidSpawner mAsteroidSpawner;

	public GameObject mTittle;

	[SerializeField]
	private int Difficulty;
		
	[SerializeField]
	private Camera mCamera;
	[SerializeField]
	private Canvas mCanvas;

	[SerializeField]
	private GameObject mStageClearObject;
	[SerializeField]
	private GameObject mGameoverObject;
	
	//Audio was the latest things that I added, has a very simple implementation
	private AudioSource[] mAudioComponent;

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

	void SetDifficulty(int diff)
	{
		if (this.utils == null)
		{
			Debug.LogError("utils must be initialized");
			return;
		}
		if (diff > 2 )
		{
			Debug.LogWarning("For now, 2 is the maximum difficulty level");
			diff = 2;
		}
		this.utils.difficulty = diff;
	}

	void OnEndStage(GameObject sender, GameObject receiver, GameEvent gameEvent, object param)
	{
		//The stage finish with a victory
		if (mLivesCounter > 0)
		{
			mStageClearObject.SetActive(true);
			mAsteroidSpawner.BroadcastMessage("StopAsteroid");
			mAudioComponent[1].Play();
		}
	}

	void OnAsteroidFallen(GameObject sender, GameObject receiver, GameEvent gameEvent, object param)
	{
		mLivesCounter--;
		mAudioComponent[0].Play();

		if (mLivesCounter <= 0)
		{
			mLivesCounter = 0;
			utils.EnableFadeObject();
			utils.Fade(false, Color.red);			
			mAsteroidSpawner.BroadcastMessage("StopAsteroid");
			mGameoverObject.SetActive(true);
			// Send EndStage event 
			Messaging.Send(gameObject, null, GameEvent.EndStage, null);
		}
		else
		{
			utils.EnableFadeObject();
			utils.Fade(true, Color.red, 4f);
		}
		mLives.text = mLivesCounter.ToString();
	}

	void OnAsteroidHittedByPlayer(GameObject sender, GameObject receiver, GameEvent gameEvent, object param)
	{
		mCounterOfAsteriodsDestroyedByPlayer++;
		mScore.text = mCounterOfAsteriodsDestroyedByPlayer.ToString();
	}

	void Start ()
	{
		App.GM = this;
		mTittle.SetActive(false);
		utils = new ConstantsAndUtils(mCamera, mCanvas);
		SetDifficulty(Difficulty);
		SetLevelProperties();
		SubscribeToEvents();
		mAudioComponent = GetComponents<AudioSource>();
	}

	public void SetLevelProperties()
	{
		utils.DisableFadeObject();
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