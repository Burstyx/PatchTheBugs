using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouteurBugs : MonoBehaviour
{
    public int bugsCount;
    public int chanceToGetABug = 15;
    public int interval = 2;

    PlayerMovement playerMovement;

    public float efficiency = 100f;
    public float efficiencyReductorPerBug = .5f;

    public Sprite routeurStage1;
    public Sprite routeurStage2;
    public Sprite routeurStage3;
    public Sprite routeurStage4;

    SpriteRenderer bugObject;

    public GameObject bug;

    bool addedChance = false;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        StartCoroutine(SummonBug());
        StartCoroutine(DecreaseRouteurEfficiency());
    }

    private void Update()
    {
        if(chanceToGetABug < 80)
        {
            if (!addedChance)
            {
                if (Mathf.Floor(GameManager.Instance.secondInGame) % 60 == 59)
                {
                    chanceToGetABug += 5;
                    GameManager.Instance.chanceToSpawnPatch += 5;
                    GameManager.Instance.chanceToSpawnBonus += 2;

                    if(GameManager.Instance.durationBetweenCalcChancePatch > 1.5f)
                        GameManager.Instance.durationBetweenCalcChancePatch -= .5f;
                    if (GameManager.Instance.durationBetweenCalcChanceBonus > 1.5f)
                        GameManager.Instance.durationBetweenCalcChanceBonus -= .5f;
                    addedChance = true;
                }
            }
            else if (Mathf.Floor(GameManager.Instance.secondInGame) % 60 != 59)
            {
                addedChance = false;
            }
        }
    }

    IEnumerator DecreaseRouteurEfficiency()
    {
        while (!GameManager.Instance.lost)
        {
            yield return new WaitForSeconds(.5f);
            efficiency -= efficiencyReductorPerBug * bugsCount;
            playerMovement.currentMaxDistance = (playerMovement.maxDistance * efficiency) / 100;
        }
    }

    IEnumerator SummonBug()
    {
        while (!GameManager.Instance.lost)
        {
            int randValue = Random.Range(0, 100);

            if (randValue >= 0 && randValue <= chanceToGetABug)
            {
                bugsCount++;
                

                Instantiate(bug, transform.position, Quaternion.identity);

                print("NEW BUG!");

                if (bugsCount > 2)
                {
                    GetComponent<SpriteRenderer>().sprite = routeurStage1;
                    print("Stage 1 atteint !");
                }

                if (bugsCount > 5)
                {
                    GetComponent<SpriteRenderer>().sprite = routeurStage2;
                }

                if (bugsCount > 8)
                {
                    GetComponent<SpriteRenderer>().sprite = routeurStage3;
                }

                if (bugsCount > 10)
                {
                    GetComponent<SpriteRenderer>().sprite = routeurStage4;
                }
            }

            yield return new WaitForSeconds(interval);
        }
        
    }
}
