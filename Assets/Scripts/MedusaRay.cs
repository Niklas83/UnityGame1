using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MedusaRay
{
	private LineRenderer _beamRenderer;
	private Ray _ray;
	private List<GameObject> _resultCache;
	private MedusaUnit _medusa;
	private Vector3 _direction;
	
	public MedusaRay(Vector3 direction, GameObject beamPrefab, MedusaUnit medusa) {
		_resultCache = new List<GameObject>();
		_medusa = medusa;
		_direction = direction;
		
		GameObject beam = GameObject.Instantiate(beamPrefab) as GameObject;
		_beamRenderer = beam.GetComponent<LineRenderer>();
		
		_ray = new Ray(medusa.transform.position + new Vector3(0, 0.5f, 0), direction);
    }

    public GameObject Blast()
    {
		RaycastHit hit;
    	if (Physics.Raycast((UnityEngine.Ray) _ray, out hit, 100f)) {
			GameObject go = hit.collider.gameObject;
    		SpanTo(hit.distance);
    		return go;
    	}
		SpanTo(50);
			
		return null;
    }

	public void SpanTo(float distance)
	{
		Vector3 hitPosition = _medusa.transform.position + _direction * distance;
		
		_beamRenderer.SetPosition(0, _medusa.transform.position);
		_beamRenderer.SetPosition(1, hitPosition);
	}
}