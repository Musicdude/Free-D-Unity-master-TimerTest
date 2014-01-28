using UnityEngine;
using System.Collections;

/* <summary>
 * A simple rotation script
 * for any objects.
 * </summary>
 */

public class RotationScript : MonoBehaviour 
{
	[SerializeField] private Vector3 mAxis;
	[SerializeField] private float	 mRotationSpeed; 
	[SerializeField] private Space	 mRotateSpace;

	private void Update()		{	transform.Rotate(mAxis,mRotationSpeed * Time.deltaTime,mRotateSpace);	}
	public Vector3 Axis			{	set{ mAxis = value; 			}	}
	public float RotationSpeed	{	set { mRotationSpeed = value; 	}	}
	public Space RotateSpace	{	set { mRotateSpace = value;		}	}
}