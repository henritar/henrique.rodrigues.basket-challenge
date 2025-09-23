using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Interfaces.UI;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Runtime.UI.GameplayUI
{
    public class GameplayUIView : BaseUIView, IGameplayUIView
    {
        [SerializeField] private TextMeshProUGUI _playerScoreText;
        [SerializeField] private TextMeshProUGUI _npcScoreText;

        public void UpdatePlayerScore(int score)
        {
            _playerScoreText.text = score.ToString();        
        }
        public void UpdateNpcScore(int score)
        {
            _npcScoreText.text = score.ToString();
        }
    }
}