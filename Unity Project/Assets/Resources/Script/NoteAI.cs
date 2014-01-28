using UnityEngine;
using System.Collections;

[RequireComponent (typeof(RotationScript))]
[RequireComponent (typeof(SphereCollider))]
[RequireComponent (typeof(Rigidbody))]
public class NoteAI : MonoBehaviour 
{
	private NoteType	mType;
	private bool		mEnabled;
	private int			mMovementSpeed;

	#region Unity Function
	// Use this for initialization
	private void Awake()
	{
		RotationScript temp = gameObject.GetComponent<RotationScript>();	// Grab the Rotation Script
		temp.Axis 			= Vector3.up;									// Set Axis
		temp.RotateSpace 	= Space.Self;									// set rotation space
		temp.RotationSpeed	= 200.0f;										// set rotation speed

		gameObject.rigidbody.isKinematic	= true;					// Change to IsKinematic
		gameObject.rigidbody.useGravity 	= false;				// Don't use Gravity
		IsEnabled = false;

	}
	#endregion

	#region Class Function
	public bool IsEnabled
	{
		get {	return mEnabled;	}
		set 
		{	
			if(value)	NotesManager.Instance.UpdateMovementHook += UpdateMovement;
			else 		NotesManager.Instance.UpdateMovementHook -= UpdateMovement;
			mEnabled = value;	
			this.gameObject.SetActive(mEnabled);
		}
	}
	public NoteType Type
	{
		get {	return mType;	}
		set {	mType = value;	}
	}
	public int MovementSpeed	{	set { mMovementSpeed = value; }	}

	private void UpdateMovement()
	{
		gameObject.transform.Translate(0.0f,0.0f,-mMovementSpeed * Time.deltaTime,Space.World);	// Translate the Object.
	}
	#endregion
}
