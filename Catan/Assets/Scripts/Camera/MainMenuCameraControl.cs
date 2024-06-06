using UnityEngine;
using Catan.UI;

namespace Catan.Camera
{

    public class MainMenuCameraControl : MonoBehaviour
    {

        public Animator animator;

        public MainMenu.MenuState state;

        public void ToPlayerSettings()
        {
            state = MainMenu.MenuState.PlayerSettings;
            UpdateAnimator();
        }
        public void ToMainMenu()
        {
            state = MainMenu.MenuState.MainMenu;
            UpdateAnimator();
        }

        public void UpdateAnimator()
        {
            animator.SetInteger("State", (int)state);
        }
    }
}
