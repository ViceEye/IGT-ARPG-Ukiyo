using UnityEngine;

namespace Ukiyo.Menu
{
    public class ButtonManager : MonoBehaviour
    {

        public void ExitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}
