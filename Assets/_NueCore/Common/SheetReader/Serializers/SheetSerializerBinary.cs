using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace _NueCore.Common.SheetReader.Serializers
{
    public class SheetSerializerBinary : SheetSerializerBase
    {
        public SheetSerializerBinary(object targetObject, string outputPath) : base(targetObject, outputPath) { }

        public override async Task Run()
        {
            var binaryFormatter = new BinaryFormatter();
            await using var fileStream = new FileStream(outputPath, FileMode.Create);
            await Task.Run(() => binaryFormatter.Serialize(fileStream, targetObject));

            //onComplete?.Invoke();
        }
    }
}