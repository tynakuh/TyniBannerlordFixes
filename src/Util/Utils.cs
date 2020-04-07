using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyniBannerlordFixes
{
    class Utils
    {		
		public static string FlattenException(Exception exception)
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
