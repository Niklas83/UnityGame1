using UnityEngine;
using System.Collections;

public class AudioListenerMover : MonoBehaviour {
    //Observe that this class does not inherit from BaseMover, as this mover does not use the grid

    public float MovementSpeed;

    public GameObject InitiallySelectedPlayer;

    public Vector3 _StartPosition;
    //public Vector3 _InitiallyEndPosition;
    public Vector3 _CurrentlySelectedEndPosition;     //If the selected player has changed while moving to the current one
    public bool _IsMoving = false;

    private bool clickedNewTargetWhileMoveing = false;


    private float DistanceBetweenAudioAndPlayer;



    public void MoveToSelectedPlayer(GameObject selectedPlayer)
    {
        if (selectedPlayer != InitiallySelectedPlayer)
        {
            InitiallySelectedPlayer = selectedPlayer;

            if (_IsMoving == false)
            {
                _StartPosition = this.gameObject.transform.position;
                StartCoroutine(SmoothMoveTowardsSelectedPlayer());
            }

            else
            {
                clickedNewTargetWhileMoveing = true;
                _StartPosition = this.gameObject.transform.position;
            }
        }
    }

    private IEnumerator SmoothMoveTowardsSelectedPlayer()
    {
        _IsMoving = true;
        float t = 0;
        _CurrentlySelectedEndPosition = InitiallySelectedPlayer.transform.position;

        DistanceBetweenAudioAndPlayer = Vector3.Distance(this.gameObject.transform.position, InitiallySelectedPlayer.transform.position);


        while (t < 1f)
        {
            
            if (InitiallySelectedPlayer.transform.position == _CurrentlySelectedEndPosition || clickedNewTargetWhileMoveing == false)
            {
                t += Time.deltaTime * (MovementSpeed) / DistanceBetweenAudioAndPlayer;
                transform.position = Vector3.Lerp(_StartPosition, InitiallySelectedPlayer.transform.position, Mathf.Clamp01(t));
                yield return null;
            }
            else if (clickedNewTargetWhileMoveing == true)
            {
                _CurrentlySelectedEndPosition = InitiallySelectedPlayer.transform.position;
                t = 0;
                transform.position = Vector3.Lerp(_StartPosition, InitiallySelectedPlayer.transform.position, Mathf.Clamp01(t));
                clickedNewTargetWhileMoveing = false;
                yield return null;
            }
        }

        transform.position = InitiallySelectedPlayer.transform.position;

        _IsMoving = false;

        yield return 0;
    }
}
