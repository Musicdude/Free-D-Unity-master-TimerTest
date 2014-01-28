using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider))]
[RequireComponent (typeof(TextMesh))]
[RequireComponent (typeof(MeshRenderer))]

public abstract class ButtonBase : MonoBehaviour
{
	[SerializeField] protected Color mHoverColor;		// hover color
	[SerializeField] protected Color mNormalColor;		// normal color
	[SerializeField] protected Color mClickColor;		// clicked color
	protected TextMesh 		mTextMesh;					// text mesh
	protected RaycastHit	mHit;						// ray cast
	protected bool			mClicked;					// click flag
	protected bool 			mHover = false;				// hover flag
	#region Unity Function
	private void Awake()	{		mTextMesh = gameObject.GetComponent<TextMesh>();	}
	protected virtual void Start()
	{
		ButtonManager.Instance.OnClickHook		+= OnClick;
		ButtonManager.Instance.OnHoverHook		+= OnHover;
		ButtonManager.Instance.OnExitHook 		+= OnExit;
		mClicked = false;
	}
	#endregion

	#region Class Function
	protected virtual void OnHover(Ray _ray)
	{
		if(Physics.Raycast(_ray,out mHit))
		{
			if(mHit.collider.gameObject == this.gameObject)
			{
				if(!mHover) SoundManager.Instance.Play("Hover");	// play sound
				mHover = true;										// set flag
				mTextMesh.color = mHoverColor;						// change color
			}
		}
	}
	protected virtual void OnExit(Ray _ray)
	{
		if(!Physics.Raycast(_ray,out mHit))
		{
			if(mClicked)
			{
				mClicked = false;
				ButtonManager.Instance.OnHoverHook += OnHover;
				ButtonManager.Instance.OnReleaseHook -= OnRelease;
			}
			mHover   = false;
			mTextMesh.color = mNormalColor;
		}
	}
	protected void OnClick(Ray _ray)
	{
		if(Input.GetMouseButtonDown(0))
		{
			if(Physics.Raycast(_ray,out mHit))
			{
				if(this.gameObject == mHit.collider.gameObject)
				{
					mClicked = true;
					ButtonManager.Instance.OnHoverHook -= OnHover;
					ButtonManager.Instance.OnReleaseHook += OnRelease;
					mTextMesh.color = mClickColor;
				}
			}
		}
	}
	protected abstract void OnRelease(Ray _ray);
	#endregion
}