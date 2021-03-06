﻿using System.Collections;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {
		
	public GameObject Asteroid;
	public float m_randomGenerationRate;
	private bool StopSpawning;
		
	void SubscribeToEvents()
	{
		Messaging.AddListener(GameEvent.StartStage, OnStartStage, Messaging.Filter.All);
	}

	void UnSubscribeToEvents()
	{
		Messaging.RemoveListener(GameEvent.StartStage, OnStartStage);
	}

	void OnStartStage(GameObject sender, GameObject receiver, GameEvent gameEvent, object param)
	{
		int childs = transform.childCount;
		for (int i = 0; i < childs; i++)
		{
			Destroy(transform.GetChild(i).gameObject);
		}
		UnSubscribeToEvents();
		OnEnable();		
	}

	private void OnEnable()
	{
		SubscribeToEvents();
		StopSpawning = false;
		m_randomGenerationRate = 0.75f - 0.1f * App.GM.utils.difficulty;
		if (Asteroid != null)
		{
			StartCoroutine("Spawner");
		}
		else
		{
			Debug.LogError("Reference to Asteroid's prefab not set");
		}
	}

	private IEnumerator Spawner()
	{
		while (!StopSpawning)
		{
			GameObject asteriodObject = CreateAsteroidOfRandomType();			

			asteriodObject.transform.parent = this.gameObject.transform;
			
			yield return new WaitForSeconds(m_randomGenerationRate);
		}
	}

	private GameObject CreateAsteroidOfRandomType()
	{
		GameObject randomAsteroid;
		bool big=false;

		//10% (+ a difficulty bonus) of asteroids will be of large type
		big = ( UnityEngine.Random.Range(0, 10 - App.GM.utils.difficulty) == 1 );		

		randomAsteroid = Instantiate(Asteroid) ;
		if (big)
			randomAsteroid.AddComponent<SuperAsteroid>();
		else
			randomAsteroid.AddComponent<Asteroid>();

		randomAsteroid.transform.position = GetRandomSpawnPosition();		
		randomAsteroid.GetComponent<Asteroid>().Speed = GetRandomSpeed();

		return randomAsteroid;
	}

	private Vector2 GetRandomSpawnPosition()
	{
		return new Vector2(UnityEngine.Random.Range(App.GM.utils.LeftScreenLimitX, App.GM.utils.RightScreenLimitX), App.GM.utils.SPAWN_Y);
	}

	private float GetRandomSpeed()
	{
		return UnityEngine.Random.Range(App.GM.utils.MIN_SPEED, App.GM.utils.MAX_SPEED) + App.GM.utils.difficulty;
	}

	void StopAsteroid()
	{
		StopSpawning = true;		
	}

	private void OnDestroy()
	{
		UnSubscribeToEvents();
	}
}
