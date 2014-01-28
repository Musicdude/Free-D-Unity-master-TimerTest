using UnityEngine;
using System.Collections;

/* <summary>
 * Spawner Component for Spawning Objects
 * </summary>
 */
[RequireComponent (typeof(Timer))]
public class Spawner : MonoBehaviour
{
	#region Variables
	[SerializeField] private Vector3	mSpawnLocationLowerLimit;	// Lower Limit
	[SerializeField] private Vector3	mSpawnLocationUpperLimit;	// Upper Limit
	[SerializeField] private Timer 		mSpawnTimer;				// The Timer
	#endregion

	#region Unity Function
	private void Start()	{	mSpawnTimer = this.GetComponent<Timer>();	}
	#endregion

	// Getter/Setters for Variables
	#region Class Function
	public Vector3 SpawnLocationLowerLimit
	{
		get	{	return mSpawnLocationLowerLimit;	}
		set {	mSpawnLocationLowerLimit = value;	}
	}
	public Vector3 SpawnLocationUpperLimit
	{
		get	{	return mSpawnLocationUpperLimit;	}
		set {	mSpawnLocationUpperLimit = value;	}
	}
	public Vector3 SpawnLocation
	{
		get
		{
			return new Vector3( 
	                            Random.Range(mSpawnLocationLowerLimit.x,mSpawnLocationUpperLimit.x),	// Range for X
	                            Random.Range(mSpawnLocationLowerLimit.y,mSpawnLocationUpperLimit.y),	// Range for Y
	                            Random.Range(mSpawnLocationLowerLimit.z,mSpawnLocationUpperLimit.z)		// Range for Z
	                           );
		}
	}
	public float SpawnTimer
	{
		get	{	return mSpawnTimer.ActualTimer;		}
		set {	mSpawnTimer.ActualTimer = value;	}
	}
	public void ResetSpawnTimer(){	mSpawnTimer.ActualTimer = mSpawnTimer.MaxTimer;	}
	#endregion
}
