using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;

public class SuperAsteroid : Asteroid {

	GameObject mOriginalAsteroidPrefab;

	new void Start ()
	{
		base.Start();
		mSuper = true;
		transform.localScale *= 1.5f;
		mOriginalAsteroidPrefab = Resources.Load("Asteroid") as GameObject;
	}	

	protected override void tapHandler(object sender, System.EventArgs e)
	{
		Messaging.Send(gameObject, null, GameEvent.AsteroidHittedByPlayer, null);
		m_tapGesture.Tapped -= tapHandler;
		Debug.Log("Super Asteroid Destroy");
		StartCoroutine(DestroyByPlayerHitCorroutine());
	}	

	protected override IEnumerator DestroyByPlayerHitCorroutine()
	{
		while (transform.localScale.x > 0f)
		{
			transform.localScale -= 3 * DestructionSpeed * Time.deltaTime;
			yield return null;
		}

		//Generate two new Asteroids, not to fast ones
		CreateTwoAsteroids(2f);
	}

	private void CreateTwoAsteroids(float  speed)
	{		
		GameObject goLeft = GameObject.Instantiate(mOriginalAsteroidPrefab) as GameObject;
		GameObject goRight = GameObject.Instantiate(mOriginalAsteroidPrefab) as GameObject;

		goLeft.transform.parent = App.GM.mAsteroidSpawner.transform;
		goRight.transform.parent = App.GM.mAsteroidSpawner.transform;

		Vector2 superAsteroidPosition = transform.position;
		if (goLeft != null)
		{
			Asteroid asteroidComponent = goLeft.AddComponent<Asteroid>();
			superAsteroidPosition.x -= 1;
			goLeft.transform.position = superAsteroidPosition;
			asteroidComponent.Speed = speed;
			asteroidComponent.ActivateGravity(true);
		}

		if (goRight != null)
		{
			Asteroid asteroidComponent = goRight.AddComponent<Asteroid>();
			superAsteroidPosition.x += 2;
			goRight.transform.position = superAsteroidPosition;
			asteroidComponent.Speed = speed;
			asteroidComponent.ActivateGravity(false);
		}
		Destroy(this.gameObject);
	}

}
