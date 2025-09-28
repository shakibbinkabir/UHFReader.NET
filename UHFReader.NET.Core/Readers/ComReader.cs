namespace UHFReader.Core.Readers
{
	public class ComReader : Reader
	{
		public ComReader(byte Baud)
		{
			int Port = 0;
			this.AutoOpenComPort(ref Port, Baud);
		}

		public ComReader(byte Baud, int Port)
		{
			this.OpenComPort(Port, Baud);
		}

		~ComReader()
		{
			//this.CloseComPort();
		}
	}
}
