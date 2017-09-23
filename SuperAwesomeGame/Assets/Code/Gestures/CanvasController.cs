using UnityEngine;
using TouchScript;
using TouchScript.Hit;
using TouchScript.Gestures;
using System;

//For operations on the Canvas Screen, use only if it needed.

public class CanvasController : MonoBehaviour
{  
    void Start()
    {        
 
    }

    private void OnEnable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.PointersPressed += touchesBeganHandler;
            TouchManager.Instance.PointersReleased += touchesEndHandler;
            GetComponent<TapGesture>().Tapped += touchTap;
        }
    }

    private void OnDisable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.PointersPressed -= touchesBeganHandler;
            TouchManager.Instance.PointersReleased -= touchesEndHandler;
            GetComponent<TapGesture>().Tapped -= touchTap;
        }
    }        

    private void touchesBeganHandler(object sender, PointerEventArgs e)
    {                          
        var point = e.Pointers[0];
        Vector2 position = point.Position;
		//TODO
    }

    private void touchesEndHandler(object sender, PointerEventArgs e)
    {
        var point = e.Pointers[0];
        Vector2 position = point.Position;
		// TODO
    }

    private void touchTap(object sender, EventArgs e)
    {
        var gesture = sender as TapGesture;
        HitData hit;
        //gesture.GetTargetHitResult(out hit);
		hit = gesture.GetScreenPositionHitData();

		Debug.Log(hit.ToString());
		//TODO
    }
}