using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace eXtensionSharp {
    public static class XEmbeddedResource {
        public static string xGetApiRequestFile(this string namespaceAndFileName) {
            try {
                using (var stream = typeof(XEmbeddedResource).GetTypeInfo().Assembly
                    .GetManifestResourceStream(namespaceAndFileName))
                using (var reader = new StreamReader(stream, Encoding.UTF8)) {
                    return reader.ReadToEnd();
                }
            }
            catch {
                //ApplicationProvider.WriteToLog<XEmbeddedResource>().Error(exception.Message);
                throw new Exception($"Failed to read Embedded Resource {namespaceAndFileName}");
            }
        }
    }
}