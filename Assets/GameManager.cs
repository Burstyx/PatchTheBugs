using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Image wifiSprite;
    public Sprite[] wifiBarSprites;
    public Image warning;

    public GameObject routeur;
    Vector2 routeurPos;

    public GameObject patch;
    public GameObject bonus;

    public Transform player;

    public bool lost = false;
    public int secondInGame;

    public int chanceToSpawnPatch = 20;
    public int chanceToSpawnBonus = 10;

    public float durationBetweenCalcChancePatch = 4f;
    public float durationBetweenCalcChanceBonus = 5f;

    public Transform radiusCircle;

    bool warningBool = false;

    public static GameManager Instance;

    public AudioSource audioSource;
    public AudioClip endSound;

    public GameObject loseScreen;
    public Text timeLoseScreen;

    public Text timeText;

    private void Update()
    {
        radiusCircle.localScale = new Vector3(player.GetComponent<PlayerMovement>().currentMaxDistance * 1.5f * 1.6f, player.GetComponent<PlayerMovement>().currentMaxDistance * 1.5f * 1.6f, 0);
        radiusCircle.position = routeurPos;
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        AddRouteurRandomly();
        StartCoroutine(SummonPatchRandomly());
        StartCoroutine(SummonBonusRandomly());
    }

    IEnumerator SummonPatchRandomly()
    {
        while (!lost)
        {
            yield return new WaitForSeconds(durationBetweenCalcChancePatch);

            float spawnY = UnityEngine.Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(10, 10)).y, Camera.main.ScreenToWorldPoint(new Vector2(10, Screen.height - 10)).y);
            float spawnX = UnityEngine.Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(10, 10)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - 10, 10)).x);

            Vector2 spawnPosition = new Vector2(spawnX, spawnY);

            float chanceToSpawn = UnityEngine.Random.Range(0, 100);

            if (chanceToSpawn <= chanceToSpawnPatch)
            {
                Instantiate(patch, spawnPosition, Quaternion.identity);
            }
        }
    }

    IEnumerator SummonBonusRandomly()
    {
        while (!lost)
        {
            yield return new WaitForSeconds(durationBetweenCalcChanceBonus);

            float spawnY = UnityEngine.Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(10, 10)).y, Camera.main.ScreenToWorldPoint(new Vector2(10, Screen.height - 10)).y);
            float spawnX = UnityEngine.Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(10, 10)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - 10, 10)).x);

            Vector2 position = new Vector2(spawnX, spawnY);

            float chanceToSpawn = UnityEngine.Random.Range(0, 100);

            if (chanceToSpawn <= chanceToSpawnBonus)
            {
                Instantiate(bonus, position, Quaternion.identity);
            }
        }
    }

    private void Start()
    {
        warning.enabled = false;

        StartCoroutine(timer());
    }

    private void AddRouteurRandomly()
    {
        float spawnY = UnityEngine.Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(100, 100)).y, Camera.main.ScreenToWorldPoint(new Vector2(100, Screen.height - 100)).y);
        float spawnX = UnityEngine.Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(100, 100)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - 100, 100)).x);

        routeurPos = new Vector2(spawnX, spawnY);

        Instantiate(routeur, routeurPos, Quaternion.identity);
    }

    public void setWifiLevelTo(int lvl)
    {
        if(lvl == 3)
        {
            wifiSprite.enabled = true;
            wifiSprite.sprite = wifiBarSprites[0];
            warningBool = false;
            warning.enabled = false;
            StopCoroutine(blinkingWarning());
        }
        else if(lvl == 2)
        {
            wifiSprite.enabled = true;
            wifiSprite.sprite = wifiBarSprites[1];
            warningBool = false;
            warning.enabled = false;
            StopCoroutine(blinkingWarning());
        }
        else if(lvl == 1)
        {
            wifiSprite.enabled = true;
            wifiSprite.sprite = wifiBarSprites[2];
            warningBool = false;
            warning.enabled = false;
            StopCoroutine(blinkingWarning());
        }
        else if(lvl == 0)
        {
            if (!warningBool)
            {
                warningBool = true;

                StartCoroutine(blinkingWarning());
            }
        }
        else
        {
            wifiSprite.enabled = false;
            warningBool = false;
            warning.enabled = false;
            StopAllCoroutines();
        }
    }

    IEnumerator blinkingWarning()
    {
        while (warningBool)
        {
            warning.enabled = true;

            yield return new WaitForSeconds(1f);

            warning.enabled = false;

            yield return new WaitForSeconds(1f);
        }
    }

    int min;

    IEnumerator timer()
    {
        while(!lost)
        {
            yield return new WaitForSeconds(1f);
            secondInGame++;

            min += secondInGame % 60 == 0 ? 1 : 0;

            timeText.text = min + "min:" + secondInGame % 60 + "s";
        }
    }

    public void lose()
    {
        audioSource.clip = endSound;
        audioSource.Play();
        timeLoseScreen.text = timeText.text;
        loseScreen.SetActive(true);

    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
