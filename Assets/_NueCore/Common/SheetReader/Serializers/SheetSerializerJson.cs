using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace _NueCore.Common.SheetReader.Serializers
{
    public class SheetSerializerJson : SheetSerializerBase
    {
        public SheetSerializerJson(object targetObject, string outputPath) : base(targetObject, outputPath) { }

        public override async Task Run()
        {
            var serialized = JsonUtility.ToJson(targetObject, true);
            await File.WriteAllTextAsync(outputPath, serialized);

            //onComplete?.Invoke();
        }
    }
}