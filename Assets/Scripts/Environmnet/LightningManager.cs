using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightningManager : MonoBehaviour
{
    public bool LightningOn;

    public float MinTimeBetweenLightnings = 8f;

    public float MaxTimeBetweenLightnings = 30f;

    public float MinTimeShowingLight = 0.2f;

    public float MaxTimeShowingLight = 0.8f;

    public int PercentChanceForExtraLightning = 25;

    public float MaxLightningIntensity = 2f;

    public float MinLightningIntensity = 1f;

    public float FadeOutMaxSpeed = 0.8f;

    public float FadeOutMinSpeed = 0.98f;

    public bool AudioClipLengthAddsToTimeBetweenLightnings = true;

    public float MinTimeForExtraLightning = 0.2f;

    public float MaxTimeForExtraLightning = 3f;

    public List<AudioClip> LightningAudio;

    private Light _lightning;

    private AudioSource _audioSrc;

    private bool _extraLightningChanceTested = false;

    private bool _justRanExtraLightning = false;

    private float _currentLightningTimer;

    private bool _isDoingLightning;

    private AudioClip _currentAudioClip;

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

        _lightning.intensity = Random.Range(MinLightningIntensity, MaxLightningIntensity);

        _lightning.enabled = true;
        // *** avmarkerade koden var aktuell innan _currentAudioClip.length vart en del av _currentLightningTimer **** 

        //if (!_audioSrc.isPlaying || !WaitForAudioToFinishBeforeNew)         //Check if the audio component shall wait for the audio to finish before starting a new audio clip
        //{
        //    //_audioSrc.clip = LightningAudio[Random.Range(0, LightningAudio.Count)];
        //    //_audioSrc.Play();

        //    _currentAudioClip = LightningAudio[Random.Range(0, LightningAudio.Count)];

        //    _audioSrc.PlayOneShot(_currentAudioClip);
        //}
        //else if (_justRanExtraLightning)
        //{
            _currentAudioClip = LightningAudio[Random.Range(0, LightningAudio.Count)];
            _audioSrc.PlayOneShot(_currentAudioClip);
     //   }

        StartCoroutine(FadeOutLightning());

        yield return new WaitForSeconds(randomShowTimer);
        //_lightning.enabled = false;

        //Kör extra lightning test
        if (PercentChanceForExtraLightning > Random.Range(0, 100) && _extraLightningChanceTested == false && _justRanExtraLightning == false)
        {
            _currentLightningTimer = randomShowTimer + Random.Range(MinTimeForExtraLightning,MaxTimeForExtraLightning);
            _justRanExtraLightning = true;
        }
        else if (_extraLightningChanceTested == false)
        {
            if (AudioClipLengthAddsToTimeBetweenLightnings)
            {
                _currentLightningTimer = Random.Range(MinTimeBetweenLightnings, MaxTimeBetweenLightnings) +
                                         _currentAudioClip.length;
                _justRanExtraLightning = false;
            }
            else
            {
                _currentLightningTimer = Random.Range(MinTimeBetweenLightnings, MaxTimeBetweenLightnings);
                _justRanExtraLightning = false;
            }
        }
        _extraLightningChanceTested = true;
    }


    private IEnumerator FadeOutLightning()
    {
        float fadeOutSpeed = Random.Range(FadeOutMinSpeed, FadeOutMaxSpeed);

        //float t = 0f;
        //while (t < 1f && _lightning.intensity> 0.1f)
        //{
        //    t += Time.deltaTime;
        //    _lightning.intensity = _lightning.intensity * fadeOutSpeed;
        //    yield return null;
        //}

        
        while (_lightning.intensity > 0.01f)
        {
            _lightning.intensity = _lightning.intensity * fadeOutSpeed;
            yield return null;
        }
        _lightning.enabled = false;
    }
}
