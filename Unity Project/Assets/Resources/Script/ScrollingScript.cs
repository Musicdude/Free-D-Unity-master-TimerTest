using UnityEngine;
using System.Collections;

public class ScrollingScript : MonoBehaviour
{
	[SerializeField] private Vector3 mStartPos;		// Starting Position
	[SerializeField] private Vector3 mWaitPos;		// Waiting Position
	[SerializeField] private Vector3 mEndPos;		// Ending Position

	private bool		mContinuous;	
	private float		mScrollingSpeed;

	private enum mState { start,waiting,wait,end };
	private mState mCurrentState = mState.start;
	
	public bool	 Continuous		{	set{ mContinuous	= value;	}	}
	public float ScrollingSpeed	{	set{ mScrollingSpeed = value;	}	}

	public void ChangeState()
	{
		if(mCurrentState == mState.waiting) mCurrentState = mState.wait;
	}
	public IEnumerator Scroll()
	{
		switch(mCurrentState)
		{
		case mState.start: 
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,mWaitPos,Time.deltaTime * mScrollingSpeed); 
			if( Vector3.Distance(gameObject.transform.position,mWaitPos) < 0.2f)
			{mCurrentState = mState.waiting;}
			break;
		case mState.waiting:
			if(mContinuous)
			{
				yield return new WaitForSeconds(2.0f);
				mCurrentState = mState.wait;
			}
			break;
		case mState.wait:
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,mEndPos,Time.deltaTime * mScrollingSpeed);
			if( Vector3.Distance(gameObject.transform.position,mEndPos) < 0.5f)
			mCurrentState = mState.end;
			break;
		case mState.end:
			gameObject.transform.position = mStartPos;
			mCurrentState 				  = mState.start;
			gameObject.transform.parent.GetComponent<ScrollerManager>().IncreaseIndex();
			yield break;
		}
	}
}

