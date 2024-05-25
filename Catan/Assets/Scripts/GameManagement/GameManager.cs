using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.Players;
using Catan.GameBoard;
using Catan.ResourcePhase;
using Catan.Settings;


namespace Catan.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        public Player[] players;
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
        public BoardInitializer boardInitializer;
        public Board board;
        public int totalTurns = 0;
        public int totalRounds = 0;
        public Texture2D[] resourceIcons;

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
        public void Roll()
        {
            int r0 = Random.Range(1, 7);
            int r1 = Random.Range(1, 7);
            return;

        }

        public void AdvanceTurn()
        {
            nextTurn = true;
        }

        public void OnVictory(Player winner)
        {
            Debug.Log("Winner! " + winner.playerName + " has won!");
            ResetGameAndBegin();
        }

        public void OnStaleMate()
        {
            ResetGameAndBegin();

        }
        public void ResetGameAndBegin()
        {
            ResetGame();
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
        }

        public void ToNextTurn()
        {
            phase++;
            Player winner = null;
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
