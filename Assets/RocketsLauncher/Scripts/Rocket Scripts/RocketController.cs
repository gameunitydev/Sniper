using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour {

    [Header("Rocket Target")]
    public Transform targetTransform;

    [Header("Rocket Stats")]
    public float minSpeed = 15, maxSpeed = 20;    
    public float minRotationSpeed = 2, maxRotationSpeed = 6;

    [Header("Jitter")]
    public AnimationCurve jitterCurve;
    public AnimationCurve jitterHeightCurve;
    public float jitterMagnitude = 3;
    public float jitterFrequency = 2;

    [Header("Effects")]
    public GameObject explosionPrefab;
    public Transform rocketEffects;



    //PRIVATE VARIABLES
    private bool loaded = false;
    private float _speed, _rotationSpeed;

    private Quaternion _lookRotation;
    private Vector3 _direction;

    private float _distance;
    private float _startTime;

    private bool _reverseX, _reverseY;
    private float _magnitudeAmount;

    private Transform _myTransform;

    private Vector3 _startPosition;
    private Quaternion _startRotation;


    void Start()
    {
        //set random jitter magnitude
        jitterMagnitude += Random.Range(-3.0f, 1.0f);

        //set starting values for the private variables
        _startTime = Time.time - Random.Range(0.0f, 1.0f);

        _reverseX = Random.Range(0, 2) == 1 ? true : false;
        _reverseY = Random.Range(0, 2) == 1 ? true : false;

        _speed = Random.Range(minSpeed, maxSpeed);
        _rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

        _myTransform = transform;

        _startPosition = _myTransform.position;
        _startRotation = _myTransform.rotation;
    }

    //method called when the script is re-enabled
    void OnEnable()
    {
        //start rocket effects
        try
        {
            //search for all the particle systems
            ParticleSystem[] effects = transform.Find("Effects").GetComponentsInChildren<ParticleSystem>();

            //enable them
            for (int index = 0; index < effects.Length; index++)
                effects[index].Play();

        }
        catch { }
    }

    void Update()
    {
        //rotate towards the target if the target is assigned
        if (targetTransform)
        {
            _direction = targetTransform.position - _myTransform.position;
            _distance = _direction.magnitude;

            float rotationSpeedMultiplyer = _distance > 5 ? 1 : Mathf.Abs(_distance - 6) * 2;

            _lookRotation = Quaternion.LookRotation(_direction.normalized);
            _myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, _lookRotation, Time.deltaTime * _rotationSpeed * rotationSpeedMultiplyer);
        }

        //move forward
        _myTransform.position += _myTransform.forward * _speed * Time.deltaTime;

        //apply random jitter ti the rockets
        if (Time.time - _startTime > jitterFrequency)
            _startTime = Time.time;

        _magnitudeAmount = jitterCurve.Evaluate((Time.time - _startTime) / jitterFrequency) * (_reverseX ? -1 : 1) * jitterMagnitude * Time.deltaTime;
        _myTransform.position += _myTransform.right * _magnitudeAmount;

        _magnitudeAmount = jitterHeightCurve.Evaluate((Time.time - _startTime) / jitterFrequency) * (_reverseY ? -1 : 1) * jitterMagnitude * Time.deltaTime;
        _myTransform.position += _myTransform.up * _magnitudeAmount;
    }

    //method called when the rocket collides with something
    void OnCollisionEnter(Collision collision)
    {
        //check if it colided with other rockets or with something else
        if (!collision.gameObject.name.Contains("Rocket"))
        {
            //reset the position and the rotation
            _myTransform.position = _startPosition;
            _myTransform.rotation = _startRotation;

            //set the velocity to zero
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            //instantiate an explosion effect
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal);
            Instantiate(explosionPrefab, collision.contacts[0].point, rotation);

            //un-load the rocket
            this.Loaded = false;

            //stop rocket effects
            try
            {
                //search for all the particle systems
                ParticleSystem[] effects = transform.Find("Effects").GetComponentsInChildren<ParticleSystem>();

                //disable them
                for (int index = 0; index < effects.Length; index++)
                {
                    effects[index].Stop();
                }
            }
            catch { }

            //disable the controller
            this.enabled = false;
        }
    }

    public bool Loaded
    {
        get
        {
            return loaded;
        }
        set
        {
            loaded = value;

            if(loaded)
                GetComponent<MeshRenderer>().enabled = true;
            else GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
