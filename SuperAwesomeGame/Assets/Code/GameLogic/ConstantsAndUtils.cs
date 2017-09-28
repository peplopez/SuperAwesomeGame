using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantsAndUtils
{
#region VARIABLES
	public Camera m_OrtographicCamera;
	public Canvas m_canvas;
	private ImageFade m_imgFade;

	public readonly float OFFSET_X = 3;
	public readonly float SPAWN_Y = 6;

	//Change this variable in editor for adjust difficulty
	public int difficulty = 0; //   0 easy, 1 normal, 2 hard (could be a Enum but for simplicity is a simple int)

	public readonly float MIN_SPEED = 0f;
	public readonly float MAX_SPEED = 3f;

	public Vector2 LeftScreenLimit;
	public Vector2 RightScreenLimit;

	public float LeftScreenLimitX;
	public float RightScreenLimitX;

	public float GroundY = -5; //-4

	//public

	#endregion
	public void EnableFadeObject()
	{
		m_imgFade.img.enabled = true;
			//gameObject.SetActive(true);
	}

	public void DisableFadeObject()
	{
		m_imgFade.img.enabled = false;
		//m_imgFade.gameObject.SetActive(false);
	}


	public void Fade(bool away, Color color, float speed = 1f )
	{		
		m_imgFade.Fade(away, color, speed);
	}

	public ConstantsAndUtils(Camera camera, Canvas canvas)
	{
		if (camera == null)
		{
			Debug.LogError("Set camera object in inspector");
			return;
		}

		if (canvas == null)
		{			
			Debug.LogError("Set canvas object in inspector for the UI and fade in/out");
			return;
		}

		m_OrtographicCamera = camera;
		m_canvas = canvas;
		m_imgFade = m_canvas.GetComponent<ImageFade>();
		if (m_imgFade == null)
		{
			Debug.LogError("Set ImageFade object in inspector for the UI and fade in/out");
			return;
		}

		LeftScreenLimit = m_OrtographicCamera.ScreenToWorldPoint(new Vector2(0 + OFFSET_X, SPAWN_Y));
		RightScreenLimit = m_OrtographicCamera.ScreenToWorldPoint(new Vector2(Screen.width - OFFSET_X, SPAWN_Y) ) ;

		LeftScreenLimitX = LeftScreenLimit.x;
		RightScreenLimitX = RightScreenLimit.x;
	}
}