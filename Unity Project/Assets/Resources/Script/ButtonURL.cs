using UnityEngine;
using System.Collections;

public class ButtonURL : ButtonBase
{
	[SerializeField] private string mURL;
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
						Application.OpenURL(mURL);
					}
				}
			}
			ButtonManager.Instance.OnReleaseHook -= OnRelease;
		}
	}
	#endregion
}
