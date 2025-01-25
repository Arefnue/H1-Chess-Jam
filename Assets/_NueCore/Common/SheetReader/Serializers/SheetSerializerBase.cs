using System.Threading.Tasks;

namespace _NueCore.Common.SheetReader.Serializers
{
    public abstract class SheetSerializerBase
    {
        protected readonly object targetObject;
        protected readonly string outputPath;

        public SheetSerializerBase(object targetObject, string outputPath)
        {
            this.targetObject = targetObject;
            this.outputPath = outputPath;
        }

        public abstract Task Run();
    }
}