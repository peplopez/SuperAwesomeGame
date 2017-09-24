﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {

	public GameObject Asteroid;
	public float m_randomGenerationRate=0.5f;
	
	// Use this for initialization
	void Start ()
	{
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
		while (true)
		{
			GameObject asteriodObject = CreateAsteroidOfRandomType();			

			asteriodObject.transform.parent = this.gameObject.transform;
			
			yield return new WaitForSeconds(m_randomGenerationRate);
		}
	}

	private GameObject CreateAsteroidOfRandomType()
	{
		GameObject randomAsteroid;
		bool large=false;
		
		//10% of asteroids will be of large type
		large = UnityEngine.Random.Range(0, 10)==1;
		float speed = UnityEngine.Random.Range(2, 5); 

		randomAsteroid = Instantiate(Asteroid) ;		

		randomAsteroid.transform.position = new Vector2(UnityEngine.Random.Range(App.GM.utils.LeftScreenLimitX, App.GM.utils.RightScreenLimitX), App.GM.utils.SPAWN_Y);

		if (large)
			randomAsteroid.transform.localScale *= 2;

		randomAsteroid.GetComponent<Asteroid>().Speed = speed;

		return randomAsteroid;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
