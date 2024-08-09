using UnityEngine;
using System.Linq;
using Pathfinding;

public class TargetMover : MonoBehaviour 
{
	public LayerMask mask;

	public Transform target;
	public IAstarAI[] ais;

	

	Camera cam;

	public void Start () {
			
		cam = Camera.main;
			
		ais = FindObjectsOfType<MonoBehaviour>().OfType<IAstarAI>().ToArray();

	}

	void Update () {
		
		PointAndClick(ais[0]);
	}

	void PointAndClick(IAstarAI ai)
	{

		if (Input.GetMouseButton(0))
		{

			Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
			LayerMask mask = 6;

			RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, mask);

			if (hit.collider != null)
			{
				ai.destination = hit.point;
			}
		}
	}
}
