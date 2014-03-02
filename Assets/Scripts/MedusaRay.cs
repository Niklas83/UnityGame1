using System.Linq;
using UnityEngine;
using System.Collections;

public class MedusaRay : MonoBehaviour
{

    public LayerMask PlayerLayer;
    public LayerMask ObsticleLayer;


    private GameObject PlayerGameObject;
    private GameObject ObsticleGameObject;




	public void Blast(bool shootRight, bool shootLeft, bool shootUp, bool shootDown)
	{
        RaycastHit hit;

        Ray[] rayList = new Ray[4];

        if (shootRight == true)
	    {
	        rayList[0] = new Ray(transform.position + new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0));
	    }
        if (shootLeft == true)
        {
            rayList[1] = new Ray(transform.position + new Vector3(0, 0.5f, 0), new Vector3(-1, 0, 0));
        }
        if (shootUp == true)
        {
            rayList[2] = new Ray(transform.position + new Vector3(0, 0.5f, 0), new Vector3(0, 0, 1));
        }
        if (shootDown == true)
        {
            rayList[3] = new Ray(transform.position + new Vector3(0, 0.5f, 0), new Vector3(0, 0, -1));
        }

        for (int i = 0; i < rayList.Count(); i++)
        {
            PlayerGameObject = null;
            ObsticleGameObject = null;

            if (Physics.Raycast((UnityEngine.Ray)rayList[i], out hit, 100f, PlayerLayer))
            {

                PlayerGameObject = hit.collider.gameObject;

                Debug.DrawRay(rayList[i].origin, hit.point - rayList[i].origin, Color.red);

                Debug.Log(hit.collider.gameObject.name);
            }

            if (PlayerGameObject != null)
            {

                if (Physics.Raycast((UnityEngine.Ray) rayList[i], out hit, 100f, ObsticleLayer))
                {

                    ObsticleGameObject = hit.collider.gameObject;

                    Debug.DrawRay(rayList[i].origin, hit.point - rayList[i].origin, Color.red);

                    Debug.Log(hit.collider.gameObject.name);
                }

                if (ObsticleGameObject != null)
                {
                    var distanceToPlayer = Vector3.Distance(this.transform.position, PlayerGameObject.transform.position);
                    var distanceToObsticle = Vector3.Distance(this.transform.position,
                        ObsticleGameObject.transform.position);

                    if (distanceToObsticle > distanceToPlayer)
                    {
                        PlayerGameObject.SendMessage("MakePlayerFrozen");

                        

                        //TODO: ska fixa effekt så till att spelaren dör
                        Destroy(PlayerGameObject);
                    }
                }
                else
                {
                    PlayerGameObject.SendMessage("MakePlayerFrozen");
                    //TODO: ska fixa effekt så till att spelaren dör
                    Destroy(PlayerGameObject);
                }
            }
        }
        
	}
}
