using _NueCore.AudioSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace __Project.Systems.LevelSystem._LevelEndSubSystem
{
    public abstract class LevelEndPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Button continueButton;
        [SerializeField] private AudioData targetSfx;

        
        public virtual void Build(LevelEndInfo levelEndInfo)
        {
            continueButton.onClick.AddListener(()=>
            {
                levelEndInfo.OnContinueButtonClickedAction?.Invoke();
            });
            levelText.SetText(levelText.text.Replace("#level", (levelEndInfo.Level+1).ToString()));
            if (targetSfx)
            {
                targetSfx.Play();
            }
        }
    }
}