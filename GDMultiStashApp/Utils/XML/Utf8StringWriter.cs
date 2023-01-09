using System.IO;
using System.Text;

namespace Utils.XML
{
	internal sealed class Utf8StringWriter : StringWriter
	{
		// https://stackoverflow.com/a/3862106

		public override Encoding Encoding => Encoding.UTF8;
	}
}
