using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBooster : MonoBehaviour
{
    Collider2D player;
    Animator animator;
    PlayerData playerData;
    public float currentTime = 0f;

    AudioSource audioSource;
    public AudioClip audioGetPatch;

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
        if(player && Input.GetKey(KeyCode.E))
        {
            currentTime += Time.deltaTime;

            transform.Find("Visual").GetComponent<Transform>().localScale = new Vector3(Mathf.Lerp(1, 0.1f, ((currentTime * 100) / playerData.timeBeforeGetBooster) / 100), Mathf.Lerp(1, 0.1f, ((currentTime * 100) / playerData.timeBeforeGetBooster) / 100), 0);
            if (currentTime >= player.GetComponent<PlayerData>().timeBeforeGetBooster)
            {
                audioSource.PlayOneShot(audioGetPatch);
                player.GetComponent<PlayerData>().timeBeforeGetPatch /= (float) 1.05;
                player.GetComponent<PlayerData>().timeBeforeGetBooster /= (float)1.05;
                player.GetComponent<PlayerData>().timeBeforePatchBug /= (float)1.05;
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            currentTime = 0;
        }

        if(!Input.GetKey(KeyCode.E))
        {
            transform.Find("Visual").GetComponent<Transform>().localScale = new Vector3(1, 1, 0);
        }
    }
}
