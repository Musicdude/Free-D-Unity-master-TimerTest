using UnityEngine;
using System.Collections;

public class ButtonChangeDifficulty : ButtonBase
{
	[SerializeField]private DifficultyType mType;
	private bool			mSelected;
	#region Unity Function
	protected override void Start ()
	{
		base.Start ();
		if(GameManager.Instance.GetCurrentDifficulty().mDifficulty == mType)			Selected = true;
		else																			Selected = false;
		Debug.Log(gameObject.name + " " + mSelected.ToString());
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
	public DifficultyType Type			{ get { return mType; } }
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
						GameManager.Instance.SetDifficulty(mType);
						GameManager.Instance.SaveData();
					}
				}
			}
			ButtonManager.Instance.OnReleaseHook -= OnRelease;
		}
	}
	#endregion

}

