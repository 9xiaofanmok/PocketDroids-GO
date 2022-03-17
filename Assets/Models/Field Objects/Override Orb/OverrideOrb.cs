using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class OverrideOrb : MonoBehaviour
{

    [SerializeField] private float throwSpeed = 30.0f;
    [SerializeField] private float collisionStallTime = 2.0f;
    [SerializeField] private float stallTime = 5.0f;
    [SerializeField] private AudioClip dropSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip throwSound;

    private float lastX;
    private float lastY;
    private bool released;
    private bool holding;
    private bool trackingCollisions = false;
    private Rigidbody rigidbody;
    private AudioSource audioSource;
    private InputStatus inputStatus;

    private enum InputStatus
    {
        Grabbing,
        Holding,
        Releasing,
        None
    }


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();

        Assert.IsNotNull(audioSource);
        Assert.IsNotNull(rigidbody);
        Assert.IsNotNull(dropSound);
        Assert.IsNotNull(successSound);
        Assert.IsNotNull(throwSound);
    }

    private void Update()
    {
        // check if player has released ball
        if (released)
        {
            return;
        }

        // check if player is holding ball
        if (holding)
        {
            // Track their input
            FollowInput();
        }

        // Update input status
        UpdateInputStatus();

        // React to input status
        switch (inputStatus)
        {
            case InputStatus.Grabbing:
                Grab();
                break;
            case InputStatus.Holding:
                Drag(); 
                break;
            case InputStatus.Releasing:
                Release();
                break;
            case InputStatus.None:
                return;
            default:
                return;
        }
    }


    private void UpdateInputStatus()
    {
        #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            inputStatus = InputStatus.Grabbing;
        } else if (Input.GetMouseButton(0))
        {
            inputStatus = InputStatus.Holding;
        } else if (Input.GetMouseButtonUp(0))
        {
            inputStatus = InputStatus.Releasing;
        } else
        {
            inputStatus = InputStatus.None;
        }
        #endif
        #if !UNITY_EDITOR
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            inputStatus = InputStatus.Grabbing;
        } else if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            inputStatus = InputStatus.Releasing;
        } else if (Input.touchCount == 1)
        {
            inputStatus = InputStatus.Holding;
        } else
        {
            inputStatus = InputStatus.None;
        }
        #endif

    }

    private void FollowInput()
    {
        Vector3 inputPos = GetInputPosition();
        // put z about 7.5f away from the camera
        inputPos.z = Camera.main.nearClipPlane * 7.5f;

        // figure out where the player is based on where the finger is (x and y, and our estimated z)
        Vector3 pos = Camera.main.ScreenToWorldPoint(inputPos);

        // Lerp: smooth out transition from one Vector3 to another
        transform.localPosition = Vector3.Lerp(transform.localPosition, pos, 50.0f * Time.deltaTime);

    }

    private void Grab()
    {
        // ray is an infinite line starting at origin and going in some direction
        // ScreenPointToRay: ray going from camera though a screen point
        Ray ray = Camera.main.ScreenPointToRay(GetInputPosition());
        RaycastHit point;

        // Physics.Raycast: casts a ray from (origin, in direction, of length maxDistance)
        if (Physics.Raycast(ray, out point, 100.0f) && point.transform == transform)
        {
            holding = true;
            transform.parent = null;
        }


    }

    private void Drag()
    {
        lastX = GetInputPosition().x;
        lastY = GetInputPosition().y;
    }

    private void Release()
    {
        // make sure user's finger is at a higher y point than it was, prevents ball from being thrown backwards
        if (lastY < GetInputPosition().y)
        {
            Throw(GetInputPosition());
        }
    }

    private Vector2 GetInputPosition()
    {
        Vector2 result = new Vector2();

        #if UNITY_EDITOR
        result = Input.mousePosition;
        #endif
        #if !UNITY_EDITOR
        result = Input.GetTouch(0).position;
        #endif
        return result;
    }

    private void Throw(Vector2 targetPos)
    {
        rigidbody.useGravity = true;
        trackingCollisions = true;

        float yDiff = (targetPos.y - lastY) / Screen.height * 100;
        // calc. throw speed of the orb, based on the constant and difference as to how much the user moved
        float speed = throwSpeed * yDiff;

        // calc % diff of x
        // calc if user is to go left or right and angle
        float x = (targetPos.x / Screen.width) - (lastX / Screen.width);
        x = Mathf.Abs(GetInputPosition().x - lastX) / Screen.width * 100 * x;

        // calc direction for orb to be thrown at based on world space rather than local space
        Vector3 direction = new Vector3(x, 0.0f, 1.0f);
        direction = Camera.main.transform.TransformDirection(direction);

        // throws orb 
        rigidbody.AddForce((direction * speed / 2.0f) + Vector3.up * speed);

        audioSource.PlayOneShot(throwSound);

        released = true;
        holding = false;

        Invoke("PowerDown", stallTime);
 
    }

    private void PowerDown()
    {
        CaptureSceneManager manager = FindObjectOfType<CaptureSceneManager>();
        if (manager != null)
        {
            manager.OrbDestroyed();
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!trackingCollisions)
        {
            return;
        }

        trackingCollisions = false;
        if (collision.gameObject.CompareTag(PocketDroidsConstants.TAG_DROID))
        {
            audioSource.PlayOneShot(successSound);
        } else
        {
            audioSource.PlayOneShot(dropSound);
        }

        Invoke("PowerDown", collisionStallTime);
    }

}
