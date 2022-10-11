using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPatch : MonoBehaviour
{
    float currentTime = 0f;
    Collider2D player;
    PlayerData playerData;
    public Animator animator;

    AudioSource audioSource;
    public AudioClip audioGetPatch;

    public AnimationClip getPatchClip;

    private void Start()
    {
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
            transform.Find("Visual").GetComponent<Transform>().localScale = new Vector3(1, 1, 0);
        }
    }



    // Update is called once per frame
    void Update()
    {
        if(player && Input.GetKey(KeyCode.E) && !playerData.hasPatch)
        {
            currentTime += Time.deltaTime;

            animator.SetBool("GetPatch", true);
            animator.SetFloat("AnimTime", (currentTime * getPatchClip.length) / playerData.timeBeforeGetPatch);

            transform.Find("Visual").GetComponent<Transform>().localScale = new Vector3(Mathf.Lerp(1, 0.1f, ((currentTime * 100) / playerData.timeBeforeGetPatch) / 100), Mathf.Lerp(1, 0.1f, ((currentTime * 100) / playerData.timeBeforeGetPatch) / 100), 0);

            if (currentTime >= playerData.timeBeforeGetPatch)
            {
                playerData.hasPatch = true;
                playerData.GetComponent<PlayerMovement>().line.startColor = Color.green;
                audioSource.PlayOneShot(audioGetPatch);
                playerData.GetComponent<PlayerMovement>().line.endColor = Color.green;
                Destroy(gameObject);
            }
            return;
        }
        else
        {
            currentTime = 0;
        }

        if (playerData && !playerData.hasPatch && !Input.GetKey(KeyCode.E))
        {
            animator.SetFloat("AnimTime", 0f);
            animator.SetBool("GetPatch", false);
            transform.Find("Visual").GetComponent<Transform>().localScale = new Vector3(1, 1, 0);
        }
    }
}
