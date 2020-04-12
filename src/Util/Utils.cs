using System;
using System.Text;

namespace TyniBannerlordFixes
{
    public static class Utils
    {		
		public static string FlattenException(this Exception exception)
		{
			var stringBuilder = new StringBuilder();

			while (exception != null)
			{
				stringBuilder.AppendLine(exception.Message);
				stringBuilder.AppendLine(exception.StackTrace);

				exception = exception.InnerException;
			}

			return stringBuilder.ToString();
		}
	}
}
