using UnityEngine;
using System.Collections;

/* <summary>
 * The Planet AI is the script which will
 * which will be attached to the Gameobject
 * with the model of the Planet.
 * </summary>
 */
[RequireComponent (typeof(RotationScript))]
[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(SphereCollider))]
public class PlanetAI : MonoBehaviour
{
	#region Variables
	private int			mSpeed;		// Speed of Planet
	private int 		mDamage;	// Damage of Planet
	private bool		mEnabled;	// State of Planet
	#endregion

	#region Unity Function
	// Use this for initialization
	private void Awake()
	{
		gameObject.name = "planet";								// A random name
		mSpeed			= PlanetManager.Instance.PlanetSpeed;	// Set the Speed
		mDamage			= PlanetManager.Instance.PlanetDamage;	// Set the Damage
		IsEnabled		= false;								// Disable the Planet
	
		RotationScript temp = gameObject.GetComponent<RotationScript>();	// Grab the Rotation Script
		temp.Axis 			= Vector3.up;									// Set Axis
		temp.RotateSpace 	= Space.Self;									// set rotation space
		temp.RotationSpeed	= 40.0f;										// set rotation speed
		
		gameObject.rigidbody.isKinematic	= true;									// set rigid body settings
		gameObject.rigidbody.useGravity 	= false;								// set rigid body settings
		gameObject.rigidbody.constraints 	= RigidbodyConstraints.FreezePositionY;	// set rigid body settings
		gameObject.rigidbody.freezeRotation = true;									// set rigid body settings
	}
	#endregion

	#region Class Function
	public bool IsEnabled
	{
		get {	return mEnabled;	}
		set 
		{	
			mEnabled = value;	
			this.gameObject.SetActive(mEnabled);
			if(value)	PlanetManager.Instance.PlanetMovementHook += UpdateMovement;
			else 		PlanetManager.Instance.PlanetMovementHook -= UpdateMovement;
		}
	}
	public int Damage	{	get { return mDamage;	}	}
	#endregion

	#region Delegate Function
	public void UpdateMovement()	{	transform.Translate(0,0,-mSpeed * Time.deltaTime,Space.World);	}
	#endregion
}