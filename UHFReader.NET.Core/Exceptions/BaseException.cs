namespace UHFReader.Core.Exceptions
{
	public class BaseException : Exception
	{
		public int Result { get; protected set; }

		public BaseException(int result, string message) : base(message)
		{
			this.Result = result;
		}
	}
}
