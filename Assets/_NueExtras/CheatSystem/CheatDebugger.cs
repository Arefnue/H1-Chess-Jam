using System.Text;
using _NueCore.Common.ReactiveUtils;
using TMPro;
using UniRx;
using UnityEngine;

namespace _NueExtras.CheatSystem
{
    public class CheatDebugger : MonoBehaviour
    {
        [SerializeField] private TMP_Text statsLog;
        
        private StringBuilder _str = new StringBuilder();
        private bool _isCheatActive;
        private void Awake()
        {
            RBuss.OnEvent<CheatREvents.EnableCheatREvent>().TakeUntilDestroy(gameObject).Subscribe(ev =>
            {
                _isCheatActive = true;
            });
            
            RBuss.OnEvent<CheatREvents.DisableCheatREvent>().TakeUntilDestroy(gameObject).Subscribe(ev =>
            {
                _isCheatActive = false;
            });
        }

        // private void Update()
        // {
        //     if (!_isCheatActive)
        //         return;
        //     _str.Clear();
        //
        //     // if (RunStatic.ActiveRunner != null)
        //     // {
        //     //     foreach (var kvp in RunStatic.ActiveRunner.GlobalCommonModValues)
        //     //     {
        //     //         var keyName = kvp.Key.ToString();
        //     //         keyName = keyName.Replace("Percent", " %");
        //     //         _str.Append(keyName).Append(": ").Append(kvp.Value).AppendLine();
        //     //         if (commonModValues.ContainsKey(kvp.Key))
        //     //             commonModValues[kvp.Key] = kvp.Value;
        //     //         else
        //     //             commonModValues.Add(kvp.Key, kvp.Value);
        //     //     }
        //     // }
        //     statsLog.SetText(_str.ToString());
        // }
    }
}