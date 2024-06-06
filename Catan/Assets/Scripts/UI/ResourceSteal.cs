using Catan.GameManagement;
using Catan.Players;
using Catan.ResourcePhase;
using Catan.TradePhase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceSteal : MonoBehaviour
{
    public GameObject playerButtonPrefab;
    public TextMeshProUGUI titleText;
    public Player stealer;
    public Player[] candidates;


    public static readonly Vector3[] locations = { new Vector3(0, 0, 0), new Vector3(0, -80, 0), new Vector3(0, 80, 0) };


    public void ClearItems()
    {
        foreach (Transform child in transform)
        {
            if (child.name != "TopLabel")
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void AddItems()
    {
        titleText.text = stealer.playerName + " - Steal resource";
        titleText.color = stealer.playerColor;

        for (int i = 0; i < candidates.Length; i++)
        {
            GameObject btn = Instantiate(playerButtonPrefab);
            btn.transform.SetParent(transform);
            btn.transform.position = transform.position + locations[i];
            btn.GetComponentInChildren<TextMeshProUGUI>().text = candidates[i].playerName + ": " + candidates[i].resourceSum + " resources";

            int index = i;
            btn.GetComponent<Button>().onClick.AddListener(() => Submit(index));
            btn.GetComponent<Button>().image.color = candidates[i].primaryUIColor;
        }
    }


    public void Submit(int toStealFrom)
    {
        Player thief = stealer;
        Player victim = candidates[toStealFrom];
        int rand = Random.Range(0, victim.resources.Length);
        int rIndex = rand;

        for (int i = 0; i < victim.resources.Length; i++)
        {
            if (victim.resources[(i + rand) % victim.resources.Length].amount > 0)
            {
                rIndex = (i + rand) % victim.resources.Length;
            }
        }
        Trader.Trade(thief, victim, new Resource[0], new Resource[] { new Resource(victim.resources[rIndex].type, 1) });
        GameObject.Find("Game Manager").GetComponent<GameManager>().UIManager.EndSteal();
    }


    public void InitializePlayers(Player initialPlayer, Player[] players)
    {
        stealer = initialPlayer;
        candidates = players;

        ClearItems();
        AddItems();


    }
}
