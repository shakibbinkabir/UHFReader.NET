using System.Net;

namespace UHFReader.Core.Readers
{
	public class NetReader : Reader
	{
		public NetReader(IPEndPoint endPoint)
		{
			this.OpenNetPort(endPoint.Port, endPoint.Address.ToString());
		}

		~NetReader()
		{
			//this.CloseNetPort();
		}
	}
}
