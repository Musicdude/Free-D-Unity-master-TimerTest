using UnityEngine;
using System.Collections;

public class PointsScript : MonoBehaviour 
{
	[SerializeField] private DifficultyType mType;
	private void Start()	{	gameObject.GetComponent<TextMesh>().text = GameManager.Instance.GetScore(mType).ToString();	}
}
