using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageFade : MonoBehaviour
{
	public Image img;
	private Color mColor;
	private float mSpeed;

	public void Fade(bool away, Color color, float speed=1)
	{
		// fades the image out when you click
		mColor = color;
		mSpeed = speed;
		StartCoroutine(FadeImage(away));		
	}

	IEnumerator FadeImage(bool fadeAway)
	{
		// fade from opaque to transparent
		if (fadeAway)
		{
			// loop over 1 second backwards
			for (float i = 1; i >= 0; i -= Time.deltaTime * mSpeed)
			{
				// set color with i as alpha
				img.color = new Color(mColor.r, mColor.g, mColor.b, i);
				yield return null;
			}
		}
		// fade from transparent to opaque
		else
		{
			// loop over 1 second
			for (float i = 0; i <= 1; i += Time.deltaTime * mSpeed)
			{
				// set color with i as alpha
				img.color = new Color(mColor.r, mColor.g, mColor.b, i);
				yield return null;
			}
		}
	}
}
