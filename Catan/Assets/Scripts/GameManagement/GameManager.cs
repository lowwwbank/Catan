using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.Players;
using Catan.GameBoard;
using Catan.UI;
using Catan.ResourcePhase;
using TMPro;
using Catan.Scoring;
using Catan.Settings;
namespace Catan.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        public Player[] players;
        public Texture2D[] resourceIcons;
        public Player currentPlayer
        {
            get
            {
                return players[turn];
            }
        }
        public int turn = -1;
        public int phase = -1;
 
        public bool starting = true;
        public bool reverseTurnOrder = false;
 
        public bool nextTurn = false;
 
        public bool movingRobber = false;
        public (int, int) robberLocation;
        public UIManager UIManager;
 
        public BoardInitializer boardInitializer;
        public Board board;
        public ScoreBuilder scoreBuilder;
        public int totalTurns = 0;
        public int totalRounds = 0;
 
        [HideInInspector]
 
        public void Start()
        {
            LoadPlayers();
            boardInitializer.Initialize();
        }
 
        public void SetDefaultPlayers()
        {
            Player[] gplayers = new Player[4];
            GameSettings.players = gplayers;
        }
 
        public void LoadPlayers()
        {
            if (GameSettings.players == null)
            {
                SetDefaultPlayers();
            }
            players = GameSettings.players;
        }
        public void UpdateScores(bool updateUI = true)
        {
            scoreBuilder.CalculateScores(players);
            if (updateUI) { UIManager.UpdateUI(); }
        }
 
        public void Roll()
        {
            int r0 = Random.Range(1, 7);
            int r1 = Random.Range(1, 7);
            Rolled(r0 + r1);
            return;
 
        }
        public void Rolled(int result)
        {
            if (result == 7)
            {
                RolledSeven();
                return;
            }
            board.DistributeResources(players, result);
            UIManager.Rolled(result);
        }
        public void RolledSeven()
        {
            UIManager.SplitResources(new Stack<Player>(ResourceDistributor.GetSplittingPlayers(players)));
        }
        public void AdvanceTurn()
        {
            nextTurn = true;
        }
        public void OnVictory(Player winner)
        {
            Debug.Log("Winner! " + winner.playerName + " has won!");
            if (GameSettings.testing)
            {
                ResetGameAndBegin();
            }
        }
 
        public void OnStaleMate()
        {
            if (GameSettings.testing)
            {
                ResetGameAndBegin();
            }
        }
 
        public void ResetGameAndBegin()
        {
            ResetGame();
            UIManager.AdvanceTurn();
        }
        public void ResetGame()
        {
            boardInitializer.Reinitialize();
            turn = 0;
            phase = -1;
            starting = true;
            reverseTurnOrder = false;
            nextTurn = false;
            movingRobber = false;
            totalRounds = 0;
            totalTurns = 0;
            UpdateScores(false);
            UIManager.ResetUI();
        }
 
        public void ToNextTurn()
        {
            scoreBuilder.CalculateScores(players);
            phase++;
 
            Player winner = players.GetWinner();
            if (winner != null)
            {
                OnVictory(winner);
                return;
            }
 
            if (totalTurns > GameSettings.maxTurns)
            {
                OnStaleMate();
                return;
            }
            if (starting)
            {
                if (phase > 1 && !reverseTurnOrder)
                {
                    phase = 0;
                    turn++;
                }
                else if (phase > 1 && reverseTurnOrder)
                {
                    phase = 0;
                    turn--;
                }
 
                if (turn >= players.Length)
                {
                    reverseTurnOrder = true;
                    turn = players.Length - 1;
                }
                if (turn < 0 && reverseTurnOrder)
                {
                    starting = false;
                    reverseTurnOrder = false;
                    turn = 0;
                }
            }
            else
            {
                if (phase > 2)
                {
                    phase = 0;
                    turn++;
                    totalTurns += 1;
                }
 
                if (turn >= players.Length || turn == -1)
                {
                    turn = 0;
                    totalRounds += 1;
                }
            }
 
            UIManager.UpdateUI();
        }
 
        public void Update()
        {
            if (nextTurn)
            {
                nextTurn = false;
                ToNextTurn();
            }
        }
    }
}
