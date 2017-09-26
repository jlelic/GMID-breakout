using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public Vector3 Velocity = new Vector3(0, 0, 0.1f);
    public float Speed = 10f;
    public GameObject BallTrailPrefab;
    public AudioClip BallWallAudioClip;
    public AudioClip BallGroundAudioClip;
    public AudioClip BallPlayerAudioClip;

    private float _ballMovementChangeModifier = 15f;
    private Rigidbody _rigidbody;
    private Coroutine _trailCoroutine;
    private AudioSource _ballRollAudioSource;
    private AudioSource _ballImpactAudioSource;
    private SphereCollider _collider;
    private bool _wasOnGround = true;
    private bool _isOnGround;
    private CameraShake _cameraShaker;
    private GamePlayHandler _gamePlayHandler;

    // Use this for initialization
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        // _rigidbody.AddForce(Vector3.forward * 600 + Vector3.down * 200 + Vector3.right * 800);
        _trailCoroutine = StartCoroutine(CreateTrail());
        var audioSources = GetComponents<AudioSource>();
        _ballRollAudioSource = audioSources[0];
        _ballImpactAudioSource = audioSources[1];
        _collider = GetComponent<SphereCollider>();
        _cameraShaker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        _gamePlayHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GamePlayHandler>();
        _gamePlayHandler.OnBallAdded();
    }

    void OnCollisionEnter(Collision col)
    {
        var tag = col.gameObject.tag;
        if (tag == "Player")
        {
            _cameraShaker.ShakeCamera(0.04f);
            var posDiff = transform.position - col.gameObject.transform.position;
            var v = _rigidbody.velocity;
            v.Set(
                Velocity.x + posDiff.x * _ballMovementChangeModifier,
                Velocity.y,
                Velocity.z
            );
            _rigidbody.velocity = v;
            _ballImpactAudioSource.clip = BallPlayerAudioClip;
            _ballImpactAudioSource.volume = 1;
            _ballImpactAudioSource.Play();
        }
        else if (tag == "Wall")
        {
            _ballImpactAudioSource.clip = BallWallAudioClip;
            _ballImpactAudioSource.volume = 1;
            _ballImpactAudioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var v = _rigidbody.velocity;
        v.Set(v.x, v.y, Mathf.Sign(v.z) * Speed);
        _rigidbody.velocity = v;
        if (gameObject.transform.position.z < -12)
        {
            StopCoroutine(_trailCoroutine);
            Destroy(gameObject);
            _gamePlayHandler.OnBallDestroyed();
            return;
        }

        _wasOnGround = _isOnGround;
        _isOnGround = Physics.Raycast(new Ray(transform.position, -Vector3.up), _collider.bounds.extents.y + 0.1f);

        if (!_wasOnGround && _isOnGround)
        {
            var power = Mathf.Abs(v.y/9);
            _ballImpactAudioSource.clip = BallGroundAudioClip;
            _ballImpactAudioSource.volume = power;
            _ballImpactAudioSource.Play();
            _cameraShaker.ShakeCamera(0.1f*power);
        }

        if (!_isOnGround)
        {
            if (_ballRollAudioSource.isPlaying)
            {
                _ballRollAudioSource.Pause();
            }
        }
        else if (!_ballRollAudioSource.isPlaying)
        {
            _ballRollAudioSource.Play();
        }
    }

    public void ShrinkBall()
    {
        StartCoroutine(ShrinkBallCoroutine());
    }

    public void EnlargeBall()
    {
        StartCoroutine(EnlargeBallCoroutine());
    }

    public void SpeedUpBall()
    {
        StartCoroutine(SpeedUpBallCoroutine());
    }


    public IEnumerator CreateTrail()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.03f);
            //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //sphere.transform.position = gameObject.transform.position;
            GameObject trail =
                Instantiate(BallTrailPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
            trail.transform.parent = gameObject.transform;
        }
    }

    private IEnumerator ShrinkBallCoroutine()
    {
        float stepSize = 0.02f;
        int stepCount = 20;
        for (int i = 0; i < stepCount; i++)
        {
            var s = transform.localScale.x - stepSize;
            gameObject.transform.localScale = new Vector3(s, s, s);
            _rigidbody.mass -= stepSize;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(7);

        for (int i = 0; i < stepCount; i++)
        {
            var s = transform.localScale.x + stepSize;
            gameObject.transform.localScale = new Vector3(s, s, s);
            _rigidbody.mass += stepSize;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator EnlargeBallCoroutine()
    {
        float stepSize = 0.05f;
        int stepCount = 20;
        for (int i = 0; i < stepCount; i++)
        {
            var s = transform.localScale.x + stepSize;
            gameObject.transform.localScale = new Vector3(s, s, s);
            _rigidbody.mass += stepSize;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(15);

        for (int i = 0; i < stepCount; i++)
        {
            var s = transform.localScale.x - stepSize;
            gameObject.transform.localScale = new Vector3(s, s, s);
            _rigidbody.mass -= stepSize;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator SpeedUpBallCoroutine()
    {
        Speed += 6f;
        _ballRollAudioSource.pitch += 1;

        yield return new WaitForSeconds(5);

        _ballRollAudioSource.pitch -= 1;
        Speed -= 6f;
    }

}