using System;
using System.Collections.Generic;
using UnityEngine;

namespace _NueCore.Common.SheetReader
{
    [CreateAssetMenu(fileName = "SheetDataExample", menuName = "NueData/SheetReader/SheetDataExample", order = 0)]
    public class SheetDataExample : SheetDataBase
    {
        [SerializeField,SheetContent] private SheetContent content;
        
        [Serializable]
        public class SheetExamplePage
        {
            public string stringExample;
            public int intExample;
            public float floatExample;
            public bool boolExample;
        }
        [Serializable]
        public class SheetContent
        {
            [SheetPage("ExamplePage")]
            [SerializeField] private List<SheetExamplePage> examplePageList;
            [SheetPage("ExamplePage2")]
            [SerializeField] private List<SheetExamplePage> examplePageList2;

        }
     
    }
}