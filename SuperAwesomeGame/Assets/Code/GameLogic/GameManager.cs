using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	// Use this for initialization
	public ConstantsAndUtils utils;

	[SerializeField]
	private Camera camera;

	void Start () {
		utils = new ConstantsAndUtils(camera);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}