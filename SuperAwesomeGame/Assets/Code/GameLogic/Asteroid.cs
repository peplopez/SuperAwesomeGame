using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;

public class Asteroid : MonoBehaviour {
	
	protected TapGesture m_tapGesture;

	float force = 100f;

	//Is the large asteroid. I prefered not to choose a solution based on inhiterance for the case of the large asteroids. 
	protected bool mSuper=false;
	public bool Big { get { return mSuper; } }

	protected float mSpeed;
	public float Speed { get { return mSpeed; } set { mSpeed = value; } }
	
	private Vector2 mDirection;
	private Rigidbody2D physicsComponent;

	//In process of destruction
	bool Impacted = false;
	protected Vector3 DestructionSpeed = new Vector3(0.5f, 0.5f, 0.5f) * 5;

	AudioSource mAudioExplosion;

	protected void Start ()
	{
		AddGestures();

		mAudioExplosion = GetComponent<AudioSource>();
	}

	protected void DestructionSound()
	{
		mAudioExplosion.Play();
	}

	// Update is called once per frame
	protected void Update () {
		//Asteriod's fall			
		transform.Translate(mSpeed * Vector3.down * Time.deltaTime);

		//Check collision to the ground
		if (!Impacted && transform.position.y < App.GM.utils.GroundY)
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
		StartCoroutine(DestroyByPlayerHitCorroutine());
	}

	//Same behaviour in both cases, no need for being virtual
	protected void ImpactWithGround()
	{
		//Asteroid in state of auto-destruction
		Impacted = true;

		m_tapGesture.Tapped -= tapHandler;
		
		//Corroutine for not being destroyed inmediataly, it could be an explosion or make it small until disappear, etc
		StartCoroutine(DestroyByGroundImpact());
	}

	protected virtual IEnumerator DestroyByPlayerHitCorroutine()
	{
		DestructionSound();
		while (transform.localScale.x > 0f)
		{			
			transform.localScale -= DestructionSpeed * Time.deltaTime;
			yield return null;
		}		
		Destroy(this.gameObject);
	}

	protected IEnumerator DestroyByGroundImpact()
	{
		while (transform.localScale.x > 0f)
		{
			transform.localScale -= DestructionSpeed * Time.deltaTime;
			yield return null;
		}
		//Send event to GameManager, which is subscribed, and substract one live 
		Messaging.Send(gameObject, null, GameEvent.AsteroidFallen, null);
		Destroy(this.gameObject);
	}

	//It is only used by the asteroids created by the SuperAsteroid. These asteroids have a limited time oblicuous trayectory
	// StopGravity delayed method will turn the asteroid to a completely normal asteroid.
	// In the scene there are two Collider2D gameobjects that ensure that these asteroids never are no accesible to the player to hit.
	public void ActivateGravity(bool left)
	{
		if (physicsComponent != null)
			Debug.LogError("Incorrect use for this method, physicsComponent should be null");

		Invoke("StopGravity", 0.4f);
		physicsComponent = gameObject.AddComponent<Rigidbody2D>();
		GetComponent<Collider2D>().isTrigger = false;
		mDirection = new Vector2((left ? -force : force), 1);

		physicsComponent.AddForce(mDirection);
	}

	//Only used by SuperAsteroid little asteroids to disable the added force.
	void StopGravity()
	{		
		Destroy(physicsComponent);
	}


	void StopAsteroid()
	{
		mSpeed = 0;
	}
}
