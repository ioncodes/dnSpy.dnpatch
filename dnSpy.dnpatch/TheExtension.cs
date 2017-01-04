using System.Collections.Generic;
using dnSpy.Contracts.Decompiler;
using dnSpy.Contracts.Extension;
using dnSpy.Contracts.Text;

// Each extension should export one class implementing IExtension

namespace dnSpy.dnpatch
{
    [ExportExtension]
    sealed class TheExtension : IExtension
    {
        public IEnumerable<string> MergedResourceDictionaries
        {
            get { yield break; }
        }

        public void OnEvent(ExtensionEvent @event, object obj)
        {
            //Logger.Instance.WriteLine("Stopped");
        }

        public ExtensionInfo ExtensionInfo => new ExtensionInfo
        {
            ShortDescription = "dnpatch",
        };
    }
}