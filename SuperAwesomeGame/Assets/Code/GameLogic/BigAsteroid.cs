using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;

public class BigAsteroid : Asteroid {

	// Use this for initialization
	GameObject mOriginalAsteroidPrefab;

	new void Start ()
	{
		base.Start();
		mBig = true;
		transform.localScale *= 1.5f;
		mOriginalAsteroidPrefab = Resources.Load("Asteroid") as GameObject;
	}
		
	/*new void Update () {
		base.Update();
	}*/

	protected override void tapHandler(object sender, System.EventArgs e)
	{
		Messaging.Send(gameObject, null, GameEvent.AsteroidHittedByPlayer, null);
		m_tapGesture.Tapped -= tapHandler;
		Debug.Log("Big Asteroid Destroy");
		StartCoroutine(DestroyByPlayerHitCorroutine());
	}

	/*void ImpactWithGround()
	{
		m_tapGesture.Tapped -= tapHandler;
		Messaging.Send(gameObject, null, GameEvent.AsteroidFallen, null);
		StartCoroutine(DestroyCorroutine());
		Debug.Log("Impact with ground. Player lose one life.");		
	}*/

	protected override IEnumerator DestroyByPlayerHitCorroutine()
	{
		while (transform.localScale.x > 0f)
		{
			transform.localScale = transform.localScale - 15 * new Vector3(0.5f, 0.5f, 0.5f) * Time.deltaTime;
			yield return null;
		}
		GameObject goLeft = GameObject.Instantiate(mOriginalAsteroidPrefab) as GameObject;
		GameObject goRight =GameObject.Instantiate(mOriginalAsteroidPrefab) as GameObject;

		Vector2 bigPosition = transform.position;
		if (goLeft != null)
		{
			Asteroid asteroidComponent= goLeft.AddComponent<Asteroid>();			
			bigPosition.x -= 1;
			goLeft.transform.position = bigPosition;
			asteroidComponent.Speed = 4;
			asteroidComponent.ActivateGravity(true);
		}

		if (goRight != null)
		{
			Asteroid asteroidComponent = goRight.AddComponent<Asteroid>();			
			bigPosition.x += 2;
			goRight.transform.position = bigPosition;
			asteroidComponent.Speed = 4;
			asteroidComponent.ActivateGravity(false);
		}
		Destroy(this.gameObject);
	}


	public IEnumerator DestroyCorroutine()
	{
		while (transform.localScale.x > 0f)
		{
			transform.localScale = transform.localScale - 5 * new Vector3(0.5f, 0.5f, 0.5f) * Time.deltaTime;
			yield return null;
		}
		Destroy(this.gameObject);
	}

}
