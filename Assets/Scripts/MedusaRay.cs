using System.Linq;
using UnityEngine;
using System.Collections;

public class MedusaRay : MonoBehaviour
{
    public LayerMask playerLayer;
    public LayerMask obsticleLayer;

    private GameObject _playerGameObject;
    private GameObject _obsticleGameObject;

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
            _playerGameObject = null;
            _obsticleGameObject = null;

            if (Physics.Raycast((UnityEngine.Ray) rayList[i], out hit, 100f, playerLayer))
            {
                _playerGameObject = hit.collider.gameObject;
                Debug.DrawRay(rayList[i].origin, hit.point - rayList[i].origin, Color.red);
            }

            if (_playerGameObject != null)
            {
                PlayerAndObsticle[0] = _playerGameObject; //Adds the player hit

                if (Physics.Raycast((UnityEngine.Ray) rayList[i], out hit, 100f, obsticleLayer))
                {
                    _obsticleGameObject = hit.collider.gameObject;
                    Debug.DrawRay(rayList[i].origin, hit.point - rayList[i].origin, Color.red);
                }

                if (_obsticleGameObject != null)
                {
                    PlayerAndObsticle[1] = _obsticleGameObject; //Adds the obsticle hit

                }

                this.gameObject.SendMessage("SetStartedToShoot", true);
                    //Sets the parameter to start executing the blast instead of trying to hit objects to return
                return PlayerAndObsticle;
            }


        }
        return null;
    }
}