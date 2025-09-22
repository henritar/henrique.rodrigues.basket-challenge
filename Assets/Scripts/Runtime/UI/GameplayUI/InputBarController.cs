using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Shared.Interfaces.Data;
using Assets.Scripts.Runtime.Shared.Interfaces.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Runtime.UI.GameplayUI
{
    public class InputBarController : MonoBehaviour, IInputBarController
    {
        [SerializeField] private Image _inputBarFillImage;
        [SerializeField] private RectTransform _perfectZone;
        [SerializeField] private RectTransform _backboardZone;

        public void SetPower(float powerPercent)
        {
            var currentPower = Mathf.Clamp(powerPercent, 0f, 100f);

            _inputBarFillImage.fillAmount = currentPower / 100f;
        }

        public void SetZonePosition(IShotResultData shotData)
        {
            foreach (var data in shotData.ShotResultRanges)
            {
                float zoneHeight = (data.MaxPower - data.MinPower) * _inputBarFillImage.rectTransform.rect.height;
                float newY = _inputBarFillImage.rectTransform.rect.height * data.MinPower;

                if (data.Result == ShotResultEnum.PerfectShot)
                {
                    Vector2 sizeDelta = new(
                        _perfectZone.sizeDelta.x,
                        zoneHeight
                    );
                    _perfectZone.sizeDelta = sizeDelta;
                    _perfectZone.anchoredPosition = new Vector2(_perfectZone.anchoredPosition.x, newY);
                }
                else if (data.Result == ShotResultEnum.BackboardBasket)
                {
                    Vector2 sizeDelta = new(
                        _backboardZone.sizeDelta.x,
                        zoneHeight
                    );

                    _backboardZone.sizeDelta = sizeDelta;
                    _backboardZone.anchoredPosition = new Vector2(_backboardZone.anchoredPosition.x, newY);
                }
            }
        }

        public void EnableBarController(bool isEnabled)
        {
            gameObject.SetActive(isEnabled);
        }
    }
}