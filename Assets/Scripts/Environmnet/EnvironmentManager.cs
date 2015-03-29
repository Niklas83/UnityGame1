using UnityEngine;
using System.Collections;

public class EnvironmentManager : MonoBehaviour
{
    public bool LightningOn;

    public float MinTimeBetweenLightnings = 5f;

    public float MaxTimeBetweenLightnings = 15f;

    public float MinTimeShowingLight = 0.2f;

    public float MaxTimeShowingLight = 0.4f;

    public int PercentChanceForExtraLightning = 80;


    private Light _lightning;

    private AudioSource _audioSrc;

    private bool _extraLightningChanceTested = false;

    private bool _justRanExtraLightning = false;

    private float _currentLightningTimer;

    private bool _isDoingLightning;

    // Use this for initialization
    void Start()
    {
        _lightning = GetComponent<Light>();
        _audioSrc = GetComponent<AudioSource>();
        _isDoingLightning = false;
        _currentLightningTimer = Random.Range(MinTimeBetweenLightnings, MaxTimeBetweenLightnings);
    }

    // Update is called once per frame
    void Update()
    {
        if (LightningOn && !_isDoingLightning)
        {
            _isDoingLightning = true;
            StartCoroutine(StartLightning());
        }

    }

    public IEnumerator StartLightning()
    {
        yield return new WaitForSeconds(_currentLightningTimer);
        DoLightning();
        
        
        _isDoingLightning = false;
        _extraLightningChanceTested = false;
    }

    private void DoLightning()
    {
        if (!_justRanExtraLightning)
        {
            _lightning.transform.rotation = Quaternion.Euler(Random.Range(45f, 55f), Random.Range(1f, 361f), 0f);
        }
        StartCoroutine(TurnLightOn());

    }

    private IEnumerator TurnLightOn()
    {
        float randomShowTimer = Random.Range(MinTimeShowingLight, MaxTimeShowingLight);
        _lightning.enabled = true;
        if (!_audioSrc.isPlaying)
        {
            _audioSrc.Play();
        }
        yield return new WaitForSeconds(randomShowTimer);
        _lightning.enabled = false;

        //Kör extra lightning test
        if (PercentChanceForExtraLightning > Random.Range(0, 100) && _extraLightningChanceTested == false && _justRanExtraLightning == false)
        {
            _currentLightningTimer = randomShowTimer + 0.2f;
            _justRanExtraLightning = true;
        }
        else if (_extraLightningChanceTested == false)
        {
            _currentLightningTimer = Random.Range(MinTimeBetweenLightnings, MaxTimeBetweenLightnings);
            _justRanExtraLightning = false;
        }
        _extraLightningChanceTested = true;
    }
}
