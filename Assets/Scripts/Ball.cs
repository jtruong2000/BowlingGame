using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{

    public Rigidbody rb; // reference to the Rigidbody component of the ball
    public float startSpeed = 40f; // the speed at which the ball starts moving

    private Transform _arrow;

    private bool _ballMoving;

    private Transform _startPosition;

    private List<GameObject> _pins = new();

    private readonly Dictionary<GameObject, Transform> _pinsDefaultTransform = new();

    public int Point { get; set; }

    [SerializeField] private Animator cameraAnim;

    private TextMeshProUGUI feedBack;


    private void Start()
    {
        Application.targetFrameRate = 60;

        _arrow = GameObject.FindGameObjectWithTag("Arrow").transform;

        // get the reference to the Rigidbody component of the ball
        rb = GetComponent<Rigidbody>();

        _startPosition = transform;

        _pins = GameObject.FindGameObjectsWithTag("Pin").ToList();

        foreach (var pin in _pins)
        {
            _pinsDefaultTransform.Add(pin, pin.transform);
        }

        feedBack = GameObject.FindGameObjectWithTag("FeedBack").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (_ballMoving)
        {
            return;
        }
    
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Shoot());
        }
    
        // Check if A key is pressed
        if (Input.GetKey(KeyCode.A))
        {
            MoveBall(-1); // Move the ball left
        }
    
        // Check if D key is pressed
        if (Input.GetKey(KeyCode.D))
        {
            MoveBall(1); // Move the ball right
        }
    }
    
    private void MoveBall(float direction)
    {
        float horizontalSpeed = 5f; // Adjust the speed as needed
    
        // Calculate the new position based on the direction and speed
        float newXPosition = transform.position.x + direction * horizontalSpeed * Time.deltaTime;
    
        // Update the ball's position
        transform.position = new Vector3(Mathf.Clamp(newXPosition, -5f, 5f), transform.position.y, transform.position.z);
    }

    private IEnumerator Shoot()
    {
        cameraAnim.SetTrigger("Go");
        cameraAnim.SetFloat("CameraSpeed", _arrow.transform.localScale.z);
        _ballMoving = true;
        _arrow.gameObject.SetActive(false);
        rb.isKinematic = false;

        // calculate the force vector to apply to the ball
        Vector3 forceVector = _arrow.forward * (startSpeed * _arrow.transform.localScale.z);

        // calculate the position at which to apply the force (in this case, the center of the ball)
        Vector3 forcePosition = transform.position + (transform.right * 0.5f);

        // apply the force at the specified position
        rb.AddForceAtPosition(forceVector, forcePosition, ForceMode.Impulse);


        yield return new WaitForSecondsRealtime(7);

        _ballMoving = false;

        GenerateFeedBack();

        yield return new WaitForSecondsRealtime(2);

        ResetGame();
    }


    private static void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GenerateFeedBack()
    {
        feedBack.text = Point switch
        {
            0 => "Nothing!",
            > 0 and < 3 => "You are learning Now!",
            >= 3 and < 6 => "It was close!",
            >= 6 and < 10 => "It was nice!",
            _ => "Perfect! You are a master!"
        };

        feedBack.GetComponent<Animator>().SetTrigger("Show");
    }
}
