using Assets.Scripts.Runtime.Shared.Interfaces.UI;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Runtime.UI.GameplayUI
{
    public class FireballBarController : MonoBehaviour, IFireballBarController
    {
        [SerializeField] private TextMeshProUGUI _fireballText;
        [SerializeField] private FireballStackElement[] _fireballStacks;

        private int _threshold = 1;

        public void EnableBarController(bool isEnabled)
        {
            gameObject.SetActive(isEnabled);
        }

        public void StacksFiller(int streak, int threshHold)
        {
            if (_threshold != threshHold)
            {
                SetStackThreshHold(threshHold);
            }

            for (int index = 0; index < threshHold; index++)
            {
                _fireballStacks[index].ToggleStack(index < streak);
            }

            _fireballText.gameObject.SetActive(streak >= threshHold);
        }


        private void SetStackThreshHold(int threshHold)
        {
            _threshold = threshHold;

            for (int i = 0; i < threshHold; i++)
            {
                _fireballStacks[i].transform.parent.gameObject.SetActive(true);
            }

            for (int j = threshHold; j < _fireballStacks.Length; j++)
            {
                _fireballStacks[j].transform.parent.gameObject.SetActive(false);
            }
        }
    }
}