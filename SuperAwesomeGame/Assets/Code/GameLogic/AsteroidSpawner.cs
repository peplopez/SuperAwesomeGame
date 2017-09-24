using System.Collections;
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
		bool big=false;

		//10% of asteroids will be of large type
		big = ( UnityEngine.Random.Range(0, 10)==1 );		

		randomAsteroid = Instantiate(Asteroid) ;
		if (big)
			randomAsteroid.AddComponent<BigAsteroid>();
		else
			randomAsteroid.AddComponent<Asteroid>();

		randomAsteroid.transform.position = GetRandomSpawnPosition();

		float speed = GetRandomSpeed(); 
		randomAsteroid.GetComponent<Asteroid>().Speed = speed;

		return randomAsteroid;
	}

	private Vector2 GetRandomSpawnPosition()
	{
		return new Vector2(UnityEngine.Random.Range(App.GM.utils.LeftScreenLimitX, App.GM.utils.RightScreenLimitX), App.GM.utils.SPAWN_Y);
	}

	private float GetRandomSpeed()
	{
		return UnityEngine.Random.Range(App.GM.utils.MIN_SPEED, App.GM.utils.MAX_SPEED);
	}


	// Update is called once per frame
	void Update () {
		
	}
}
