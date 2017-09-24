using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;

public class Asteroid : MonoBehaviour {
	
	protected TapGesture m_tapGesture;

	float magnitude = 15;

	//Is the large asteroid. I prefered not to choose a solution based on inhiterance for the case of the large asteroids. 
	protected bool mBig=false;
	public bool Big { get { return mBig; } }

	protected float mSpeed;
	public float Speed { get { return mSpeed; } set { mSpeed = value; } }
	
	private Vector2 mDirection;
	// Use this for initialization
	protected void Start ()
	{
		AddGestures();
	}
	
	// Update is called once per frame
	protected void Update () {
		//Asteriod's fall
			
		transform.Translate(mSpeed * Vector3.down * Time.deltaTime);

		//if (gameObject.GetComponent<Rigidbody2D>() != null)
			//gameObject.GetComponent<Rigidbody2D>().AddForce(mDirection * magnitude);

		//Check collision to the ground
		if (transform.position.y < App.GM.utils.GroundY)
			ImpactWithGround();
	}

	protected virtual void AddGestures()
	{
		m_tapGesture = GetComponent<TapGesture>();
		m_tapGesture.Tapped += tapHandler;
	}

	protected virtual void tapHandler(object sender, System.EventArgs e)
	{
		Messaging.Send(gameObject, null, GameEvent.AsteroidHittedByPlayer, null);
		m_tapGesture.Tapped -= tapHandler;
		Debug.Log("Destroy");
		StartCoroutine(DestroyByPlayerHitCorroutine());
	}

	//Same behaviour in both cases, no need for being virtual
	protected void ImpactWithGround()
	{
		m_tapGesture.Tapped -= tapHandler;
		Messaging.Send(gameObject, null, GameEvent.AsteroidFallen, null);
		StartCoroutine(DestroyByGroundImpact());
		Debug.Log("Impact with ground. Player lose one life.");		
	}

	protected virtual IEnumerator DestroyByPlayerHitCorroutine()
	{
		while (transform.localScale.x > 0f)
		{
			transform.localScale = transform.localScale - 5 * new Vector3(0.5f, 0.5f, 0.5f) * Time.deltaTime;
			yield return null;
		}
		Destroy(this.gameObject);
	}

	protected IEnumerator DestroyByGroundImpact()
	{
		while (transform.localScale.x > 0f)
		{
			transform.localScale = transform.localScale - 5 * new Vector3(0.5f, 0.5f, 0.5f) * Time.deltaTime;
			yield return null;
		}
		Destroy(this.gameObject);
	}

	public void ActivateGravity(bool left)
	{
		gameObject.AddComponent<Rigidbody2D>();

		//if (gameObject.GetComponent<Rigidbody2D>() == null)
		//return;
		

		Rigidbody2D physicsComponent = gameObject.GetComponent<Rigidbody2D>();
		//Vector2 direction;
		//direction = transform.up.normalized;
		mDirection = new Vector2((left ? -.5f : .5f), 1);
		//direction = Vector2.up; 
		physicsComponent.AddForce(mDirection * 200);
	}

}
