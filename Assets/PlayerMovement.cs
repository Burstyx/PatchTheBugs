using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float maxSpeed = 250f;
    public float maxDistance = 15f;
    public float currentMaxDistance;
    public float currentSpeed = 0f;
    public float interpolatePoint = 1f;

    public ParticleSystem lineExploser;
    ParticleSystem lineExploserObj;

    public float distance;

    public VolumeProfile normal;
    public VolumeProfile critical;

    Volume volumeProfile;

    public LineRenderer line;

    GameObject routeur;

    private void Awake()
    {
        line = new GameObject("Line").AddComponent<LineRenderer>();
        lineExploserObj = Instantiate(lineExploser, line.transform);

        volumeProfile = GameObject.Find("Global Volume").GetComponent<Volume>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = maxSpeed;
        
        currentMaxDistance = maxDistance;

        routeur = GameObject.FindGameObjectWithTag("Routeur");
        line.startColor = Color.red;
        line.endColor = Color.red;
    }

    void FixedUpdate()
    {
        Vector2 velocity = Vector2.zero;

        line.material = new Material(Shader.Find("Sprites/Default"));
        line.positionCount = 2;
        line.SetPosition(0, routeur.transform.position);
        line.SetPosition(1, transform.position);
        line.useWorldSpace = true;
        line.sortingOrder = 1;

        float xVel = Input.GetAxisRaw("Horizontal");
        float yVel = Input.GetAxisRaw("Vertical");

        velocity = new Vector2(xVel, yVel);
        velocity = velocity.normalized;

        distance = Vector2.Distance(transform.position, routeur.GetComponent<Transform>().position);

        if ((currentSpeed * 100) / maxSpeed > 50f)
        {
            GameManager.Instance.setWifiLevelTo(3);
            if (volumeProfile.profile.name != "BugPatched")
                volumeProfile.profile = normal;
        }
        if ((currentSpeed * 100) / maxSpeed <= 50f && (currentSpeed * 100) / maxSpeed > 25f)
        {
            GameManager.Instance.setWifiLevelTo(2);
            if(volumeProfile.profile.name != "BugPatched")
                volumeProfile.profile = normal;
        }
        if ((currentSpeed * 100) / maxSpeed <= 25f && (currentSpeed * 100) / maxSpeed > 20f)
        {
            GameManager.Instance.setWifiLevelTo(1);
            volumeProfile.profile = critical;
        }
        if ((currentSpeed * 100) / maxSpeed <= 20f && (currentSpeed * 100) / maxSpeed > 15f)
        {
            GameManager.Instance.setWifiLevelTo(0);
        }
        if ((currentSpeed * 100)/maxSpeed <= 15f)
        {
            print("You lose the connection!");
            lineExploserObj.transform.position = new Vector2(Mathf.Lerp(line.GetComponent<LineRenderer>().GetPosition(0).x, line.GetComponent<LineRenderer>().GetPosition(1).x, .5f), Mathf.Lerp(line.GetComponent<LineRenderer>().GetPosition(0).y, line.GetComponent<LineRenderer>().GetPosition(1).y, .5f));
            lineExploserObj.startColor = line.startColor;
            lineExploserObj.transform.gameObject.SetActive(true);
            GameManager.Instance.setWifiLevelTo(-1);
            GameManager.Instance.lost = true;
            StartCoroutine(loseScreen());
            rb.velocity = Vector2.zero;
            GetComponent<PlayerMovement>().enabled = false;
            line.enabled = false;
            return;
        }

        interpolatePoint = distance / currentMaxDistance;
        
        currentSpeed = Mathf.Lerp(0f, maxSpeed, 1 - interpolatePoint);
        line.startWidth = Mathf.Lerp(.025f, .1f, 1 - interpolatePoint);
        line.endWidth = Mathf.Lerp(.025f, .1f, 1 - interpolatePoint);

        rb.velocity = velocity * Time.fixedDeltaTime * currentSpeed;
    }

    IEnumerator loseScreen()
    {
        yield return new WaitForSeconds(2f);

        GameManager.Instance.lose();
    }
}
