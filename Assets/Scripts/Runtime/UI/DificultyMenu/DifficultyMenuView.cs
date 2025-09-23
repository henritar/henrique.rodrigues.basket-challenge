using Assets.Scripts.Runtime.Shared;
using Assets.Scripts.Runtime.Shared.Interfaces.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Runtime.UI.DificultyMenu
{
    public class DifficultyMenuView : BaseUIView, IDifficultyMenuView
    {
        [SerializeField] private TMP_Dropdown _difficultyDropdown;

        private List<NpcDifficultyConfig> _configValues = new List<NpcDifficultyConfig>();
        private Subject<NpcDifficultyConfig> _onDifficultyConfigChanged = new ();

        public IObservable<NpcDifficultyConfig> OnDifficultyConfigChanged => _onDifficultyConfigChanged;

        private void Start()
        {
            _difficultyDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }


        public void SetDifficultyValues(NpcDifficultyConfig[] values)
        {
            _configValues.Clear();
            _configValues.AddRange(values);

            _difficultyDropdown.ClearOptions();

            var options = new List<string>();
            foreach (var v in values)
            {
                options.Add(v.NpcDifficultyEnum.ToString());
            }

            _difficultyDropdown.AddOptions(options);
            _difficultyDropdown.value = 0;
            OnDropdownValueChanged(_difficultyDropdown.value);
        }

        private void OnDropdownValueChanged(int index)
        {
            if (index >= 0 && index < _configValues.Count)
            {
                var selectedValue = _configValues[index];
                _onDifficultyConfigChanged.OnNext(selectedValue);
            }
        }
    }
}