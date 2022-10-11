using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PatchABug : MonoBehaviour
{
    RouteurBugs routeurBugs;
    Collider2D player;
    PlayerData playerData;
    Animator animator;
    float currentTime = 0f;

    public VolumeProfile normal;
    public VolumeProfile bugPatched;

    AudioSource audioSource;
    public AudioClip audioGetPatch;

    Volume volumeProfile;

    public AnimationClip getPatchClip;

    // Start is called before the first frame update
    void Start()
    {
        routeurBugs = gameObject.GetComponentInParent<RouteurBugs>();
        print(gameObject.name);

        volumeProfile = GameObject.Find("Global Volume").GetComponent<Volume>();

        audioSource = GameObject.Find("SoundEffect").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision;
            animator = player.GetComponent<Animator>();
            playerData = player.GetComponent<PlayerData>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player && Input.GetKey(KeyCode.E) && playerData.hasPatch && routeurBugs.bugsCount > 0)
        {
            currentTime += Time.deltaTime;
            animator.SetBool("GetPatch", true);
            animator.SetFloat("AnimTime", getPatchClip.length - (currentTime * getPatchClip.length) / playerData.timeBeforePatchBug);
            if (currentTime >= playerData.timeBeforePatchBug)
            {
                audioSource.PlayOneShot(audioGetPatch);
                playerData.hasPatch = false;
                playerData.GetComponent<PlayerMovement>().line.startColor = Color.red;
                playerData.GetComponent<PlayerMovement>().line.endColor = Color.red;
                routeurBugs.efficiency += 4;
                routeurBugs.bugsCount--;
                Destroy(GameObject.FindGameObjectWithTag("VisualEffectBug"));
                currentTime = 0f;
                print("ONE BUG SOLVED! Remain: " + routeurBugs.bugsCount);
                StartCoroutine(BugFixedEffect());
            }
        }
        else
        {
            currentTime = 0;
        }

        if (player && playerData.hasPatch && !Input.GetKey(KeyCode.E))
        {
            animator.SetFloat("AnimTime", getPatchClip.length);
        }


    }

    IEnumerator BugFixedEffect()
    {
        volumeProfile.profile = bugPatched;

        yield return new WaitForSeconds(.5f);

        volumeProfile.profile = normal;
    }
}
