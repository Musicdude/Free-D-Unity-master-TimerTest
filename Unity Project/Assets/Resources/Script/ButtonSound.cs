using UnityEngine;
using System.Collections;

public class ButtonSound : ButtonBase 
{
	[SerializeField] private SoundType mType;
	private TextMesh mMesh;
	private void Start()
	{
		base.Start();
		mMesh = gameObject.GetComponent<TextMesh>();
		if(mType == SoundType.music)
		{
			if(Global.mMusicOn) mMesh.text = "On";
			else 				mMesh.text = "Off";
			return;
		}
		if(mType == SoundType.sfx)
		{
			if(Global.mSFXOn)	mMesh.text = "On";
			else 				mMesh.text = "Off";
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
						Debug.Log(gameObject.name + " Release");
						if(mType == SoundType.music)
						{
							Global.mMusicOn = !Global.mMusicOn;
							if(Global.mMusicOn) mMesh.text = "On";
							else 				mMesh.text = "Off";
						}
						else if(mType == SoundType.sfx)
						{
							Global.mSFXOn = !Global.mSFXOn;
							if(Global.mSFXOn)	mMesh.text = "On";
							else 				mMesh.text = "Off";
						}
						SoundManager.Instance.SetVolume(mType);
						mTextMesh.color = mNormalColor;
						mClicked = false;
					}
				}
			}
			ButtonManager.Instance.OnReleaseHook -= OnRelease;
		}
	}
}
