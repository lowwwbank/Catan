using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Catan.Camera;
using Catan.Settings;
using Catan.Players;

namespace Catan.UI
{

    public class MainMenu : MonoBehaviour
    {

        public MainMenuCameraControl cam;
        public GameObject mainMenu;
        public GameObject playerSettings;


        public void ToPlayerSettings()
        {
            UpdateUI(MenuState.PlayerSettings);
            cam.ToPlayerSettings();
        }

        public void ToMainMenu()
        {
            UpdateUI(MenuState.MainMenu);
            cam.ToMainMenu();
        }


        public void StartGame()
        {
            if (SetPlayers())
            {
                SceneManager.LoadScene("Game");
            }
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public bool SetPlayers()
        {
            Player p0 = GameObject.Find("PlayerPanel0").GetComponent<PlayerSettings>().GetPlayer();
            Player p1 = GameObject.Find("PlayerPanel1").GetComponent<PlayerSettings>().GetPlayer();


            List<Player> players = new List<Player>();

            players.Add(p0);
            players.Add(p1);


            GameSettings.players = players.ToArray();
            return true;
        }

        public void UpdateUI(MenuState state)
        {
            DisableUI();

            switch (state)
            {
                case MenuState.MainMenu:
                    mainMenu.SetActive(true);
                    break;
                case MenuState.PlayerSettings:
                    playerSettings.SetActive(true);
                    break;
                default:
                    mainMenu.SetActive(true);
                    break;
            }
        }
        public void DisableUI()
        {
            mainMenu.SetActive(false);
            playerSettings.SetActive(false);
        }

        public enum MenuState
        {
            MainMenu,
            PlayerSettings
        }
    }
}
