using UnityEngine;
using System.Collections;

/* <summary>
 * The Timer Class is just
 * a simple timer.
 * </summary>
 */
public class Timer : MonoBehaviour
{
	[SerializeField] private float	mActualTimer = 0;	// Actual Timer
	[SerializeField] private float	mMaxTimer;			// Max Limit Timer

	public float ActualTimer
	{
		get	{	return mActualTimer;	}
		set {	mActualTimer = value;	}
	}
	public float MaxTimer
	{
		get { 	return mMaxTimer;	}
	}
}