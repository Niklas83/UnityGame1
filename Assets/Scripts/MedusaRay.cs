using System.Linq;
using UnityEngine;
using System.Collections;

public class MedusaRay : MonoBehaviour
{
    public LayerMask PlayerLayer;
    public LayerMask ObsticleLayer;

    private GameObject PlayerGameObject;
    private GameObject ObsticleGameObject;

    public GameObject[] Blast(bool shootRight, bool shootLeft, bool shootUp, bool shootDown)
    {
        GameObject[] PlayerAndObsticle = new GameObject[2];

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

            if (Physics.Raycast((UnityEngine.Ray) rayList[i], out hit, 100f, PlayerLayer))
            {

                PlayerGameObject = hit.collider.gameObject;

                Debug.DrawRay(rayList[i].origin, hit.point - rayList[i].origin, Color.red);

                Debug.Log(hit.collider.gameObject.name);
            }

            if (PlayerGameObject != null)
            {
                PlayerAndObsticle[0] = PlayerGameObject; //Adds the player hit

                if (Physics.Raycast((UnityEngine.Ray) rayList[i], out hit, 100f, ObsticleLayer))
                {

                    ObsticleGameObject = hit.collider.gameObject;

                    Debug.DrawRay(rayList[i].origin, hit.point - rayList[i].origin, Color.red);

                    Debug.Log(hit.collider.gameObject.name);
                }

                if (ObsticleGameObject != null)
                {
                    PlayerAndObsticle[1] = ObsticleGameObject; //Adds the obsticle hit

                }

                this.gameObject.SendMessage("SetStartedToShoot", true);
                    //Sets the parameter to start executing the blast instead of trying to hit objects to return
                return PlayerAndObsticle;
            }


        }
        return null;
    }
}