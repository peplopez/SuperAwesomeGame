using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;

public class Asteroid : MonoBehaviour {
	
	protected TapGesture m_tapGesture;

	protected float m_speed;
	public float Speed { get { return m_speed; } set { m_speed = value; } }
	
	// Use this for initialization
	void Start () {
		AddGestures();
	}
	
	// Update is called once per frame
	void Update () {
		//Asteriod's fall
		transform.Translate(m_speed * Vector3.down * Time.deltaTime);

		//Check collision to the ground
		if (transform.position.y < App.GM.utils.GroundY)
			ImpactWithGround();
	}

	protected virtual void AddGestures()
	{
		m_tapGesture = GetComponent<TapGesture>();
		m_tapGesture.Tapped += tapHandler;
	}

	private void tapHandler(object sender, System.EventArgs e)
	{
		Messaging.Send(gameObject, null, GameEvent.AsteroidHittedByPlayer, null);
		m_tapGesture.Tapped -= tapHandler;
		Debug.Log("Destroy");
		StartCoroutine(DestroyCorroutine());
	}

	void ImpactWithGround()
	{
		m_tapGesture.Tapped -= tapHandler;
		Messaging.Send(gameObject, null, GameEvent.AsteroidFallen, null);
		StartCoroutine(DestroyCorroutine());
		Debug.Log("Impact with ground. Player lose one life.");
		
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
