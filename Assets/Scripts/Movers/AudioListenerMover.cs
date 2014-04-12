using UnityEngine;
using System.Collections;

// Note that this class does not inherit from BaseMover, as this mover does not use the grid
public class AudioListenerMover : MonoBehaviour 
{
    public float movementSpeed;
    public GameObject initiallySelectedPlayer;
    public Vector3 startPosition;
    public Vector3 currentlySelectedEndPosition;     //If the selected player has changed while moving to the current one
    public bool isMoving = false;

    private bool _clickedNewTargetWhileMoving = false;
    private float _distanceBetweenAudioAndPlayer;

    public void MoveToSelectedPlayer(GameObject selectedPlayer)
    {
        if (selectedPlayer != initiallySelectedPlayer)
        {
            initiallySelectedPlayer = selectedPlayer;

            if (isMoving == false)
            {
                startPosition = this.gameObject.transform.position;
                StartCoroutine(SmoothMoveTowardsSelectedPlayer());
            }
            else
            {
                _clickedNewTargetWhileMoving = true;
                startPosition = this.gameObject.transform.position;
            }
        }
    }

    private IEnumerator SmoothMoveTowardsSelectedPlayer()
    {
        isMoving = true;
        float t = 0;
        currentlySelectedEndPosition = initiallySelectedPlayer.transform.position;

        _distanceBetweenAudioAndPlayer = Vector3.Distance(this.gameObject.transform.position, initiallySelectedPlayer.transform.position);

        while (t < 1f)
        {
            if (initiallySelectedPlayer.transform.position == currentlySelectedEndPosition || _clickedNewTargetWhileMoving == false)
            {
                t += Time.deltaTime * (movementSpeed) / _distanceBetweenAudioAndPlayer;
                transform.position = Vector3.Lerp(startPosition, initiallySelectedPlayer.transform.position, Mathf.Clamp01(t));
                yield return null;
            }
            else if (_clickedNewTargetWhileMoving == true)
            {
                currentlySelectedEndPosition = initiallySelectedPlayer.transform.position;
                t = 0;
                transform.position = Vector3.Lerp(startPosition, initiallySelectedPlayer.transform.position, Mathf.Clamp01(t));
                _clickedNewTargetWhileMoving = false;
                yield return null;
            }
        }

        transform.position = initiallySelectedPlayer.transform.position;

        isMoving = false;
        yield return 0;
    }
}
