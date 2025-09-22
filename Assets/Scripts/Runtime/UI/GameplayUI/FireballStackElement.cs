using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Runtime.UI.GameplayUI
{
    public class FireballStackElement : MonoBehaviour
    {
        [SerializeField] private Image _stack;

        public void ToggleStack(bool toggle)
        {
            _stack.gameObject.SetActive(toggle);
        }
    }
}