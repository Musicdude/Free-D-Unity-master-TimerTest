using UnityEngine;
using System.Collections;

public class ButtonReset : ButtonBase
{
	#region Inherited Function
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
						GameManager.Instance.ResetScores();
						GameManager.Instance.SaveData();
						mTextMesh.color = mNormalColor;
					}
				}
			}
			ButtonManager.Instance.OnReleaseHook -= OnRelease;
		}
	}
	#endregion
}
