using UnityEngine;
using System.Collections;
using UnityEngine;
using System.Collections;

public class ButtonControl : ButtonBase
{
	[SerializeField]private AndroidControl mType;

	private bool			mSelected;
	#region Unity Function
	protected override void Start ()
	{
		#if UNITY_ANDROID || UNITY_IPHONE
			base.Start ();
			if(Global.mControlType == mType)			Selected = true;
			else										Selected = false;
		#elif UNITY_EDITOR || UNITY_STANDALONE
			Disable();
		#endif
	}
	#endregion
	
	#region Class Function
	public bool Selected
	{
		get {	return mSelected;	}
		set 
		{	
			mSelected = value;
			SetColor(mSelected);
		}
	}
	public AndroidControl Type			{ get { return mType; } }

	public void Disable()
	{
		this.enabled = false;
		mTextMesh.color = Color.gray;
	}

	private void SetColor(bool _flag)	
	{
		if(_flag)	mTextMesh.color = mClickColor;
		else  		mTextMesh.color = mNormalColor;
	}
	#endregion
	
	#region Inherited Function
	protected override void OnHover(Ray _ray)
	{
		if(Physics.Raycast(_ray,out mHit))
		{
			if(mHit.collider.gameObject == this.gameObject)
			{	
				if(!mHover) SoundManager.Instance.Play("Hover");
				mHover = true;
				if(!mSelected)	mTextMesh.color = mHoverColor;
			}
		}
	}
	protected override void OnExit(Ray _ray)
	{
		if(!Physics.Raycast(_ray,out mHit))
		{
			if(mClicked)
			{
				mClicked = false;
				ButtonManager.Instance.OnHoverHook += OnHover;
				ButtonManager.Instance.OnReleaseHook -= OnRelease;
			}
			mHover = false;
			if(!mSelected)	mTextMesh.color = mNormalColor;
		}
	}
	protected override void OnRelease (Ray _ray)
	{
		if(Input.GetMouseButtonUp(0))
		{
			if(mClicked)
			{
				if(Physics.Raycast(_ray,out mHit))
				{
					if(mHit.collider.gameObject == this.gameObject)
					{
						SoundManager.Instance.Play("Select");
						mSelected = true;
						mTextMesh.color = mClickColor;
						Debug.Log(gameObject.name + " Release");
						#if UNITY_ANDROID || UNITY_IPHONE
						Global.mControlType = mType;
						GameManager.Instance.SaveData();
						#endif
					}
				}
			}
			ButtonManager.Instance.OnReleaseHook -= OnRelease;
		}
	}
	#endregion
}