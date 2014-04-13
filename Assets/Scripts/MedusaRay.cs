using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MedusaRay
{
	private LineRenderer _beamRenderer;
	private Ray _ray;
	private MedusaUnit _medusa;
	private Vector3 _direction;
	
	public MedusaRay(Vector3 direction, GameObject beamPrefab, MedusaUnit medusa) {
		_medusa = medusa;
		_direction = direction;
		
		GameObject beam = GameObject.Instantiate(beamPrefab) as GameObject;
		_beamRenderer = beam.GetComponent<LineRenderer>();
		
		_ray = new Ray(medusa.transform.position + new Vector3(0, 0.25f, 0), direction);
    }

    public GameObject Blast()
    {
		RaycastHit hit;
    	if (Physics.Raycast((UnityEngine.Ray) _ray, out hit, 100f)) {
			GameObject go = hit.collider.gameObject;
    		Span(hit.distance);
    		return go;
    	}
		Span(50);
			
		return null;
    }

	public void Span(float distance)
	{
		Vector3 position = _medusa.transform.position + new Vector3(0, 0.25f, 0);
		Vector3 hitPosition = position + _direction * distance;
		
		_beamRenderer.SetPosition(0, position);
		_beamRenderer.SetPosition(1, hitPosition);
	}
}