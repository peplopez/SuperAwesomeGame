using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantsAndUtils
{
#region VARIABLES
	public Camera m_OrtographicCamera;

	public readonly float OFFSET_X = 3;
	public readonly float SPAWN_Y = 6;
	public readonly float MIN_SPEED = 2f;
	public readonly float MAX_SPEED = 5f;

	public Vector2 LeftScreenLimit;
	public Vector2 RightScreenLimit;

	public float LeftScreenLimitX;
	public float RightScreenLimitX;

	public float GroundY = -5; //-4

#endregion

	public ConstantsAndUtils(Camera camera)
	{
		if (camera == null)
		{
			Debug.LogError("Set camera object in inspector");
			return;
		}

		m_OrtographicCamera = camera;
		LeftScreenLimit = m_OrtographicCamera.ScreenToWorldPoint(new Vector2(0 + OFFSET_X, SPAWN_Y));
		RightScreenLimit = m_OrtographicCamera.ScreenToWorldPoint(new Vector2(Screen.width - OFFSET_X, SPAWN_Y) ) ;

		LeftScreenLimitX = LeftScreenLimit.x;
		RightScreenLimitX = RightScreenLimit.x;
	}
}