
using UnityEngine;

namespace _NueExtras.StockSystem._StockUISubSystem
{
    public class StockUIController : MonoBehaviour
    {
        [SerializeField] private StockCounterLayout menuLayout;
        [SerializeField] private StockCounterLayout gameLayout;
        [SerializeField] private Transform rootTransform;
        
        private void Awake()
        {
        }

        private void Start()
        {
            SetMenu();
        }

        private void SetGame()
        {
            gameLayout.Show();
            menuLayout.Hide();
        }
        private void SetMenu()
        {
            menuLayout.Show();
            gameLayout.Hide();
        }
        
    }
}