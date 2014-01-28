using UnityEngine;
using System.Collections;

public class ScrollerManager : MonoBehaviour
{
	[SerializeField] private GameObject[]	mList;
	[SerializeField] private bool			mContinuous;
	[SerializeField] private float 			mScrollingSpeed;
	private int mCurrentIndex;
	private void Start()
	{	
		mCurrentIndex = 0;
		foreach(GameObject go in mList)
		{
			go.GetComponent<ScrollingScript>().Continuous		=	mContinuous;
			go.GetComponent<ScrollingScript>().ScrollingSpeed	=	mScrollingSpeed;
		}

	}
	private void Update()
	{
		StartCoroutine(ScrollObjects());
	}
	private IEnumerator ScrollObjects()
	{	
		yield return StartCoroutine(mList[mCurrentIndex].GetComponent<ScrollingScript>().Scroll());
	}
	public void IncreaseIndex()
	{
		if(++mCurrentIndex == mList.Length) mCurrentIndex = 0;
	}
	public void Next()
	{
		mList[mCurrentIndex].GetComponent<ScrollingScript>().ChangeState();
	}
}

