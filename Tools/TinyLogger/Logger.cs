using System;
using System.IO;
using System.Text;

namespace TinyLogger
{
	public class Logger
	{
		private static class SingletonHolder
		{
			internal static readonly Logger _instance = new Logger();
		}

		private bool _mConsoleOutput;
		private bool _mConsoleTimestamp;
		private readonly object _mLock = new object();
		private string _mLogDirectory;
		private LogIntervalType _mLogInterval;
		private LogLevel _mLogLevel;
		private string _mPostfix;
		private string _mPrefix;

		public static Logger GetInstance()
		{
			return SingletonHolder._instance;
		}

		public ErrorCode Initialize(string prefix, string postfix, string logDirectory, LogIntervalType logInterval, LogLevel logLevel, bool consoleOutput, bool consoleTimestamp)
		{
			_mLogDirectory = logDirectory;
			_mLogInterval = logInterval;
			_mPrefix = prefix;
			_mPostfix = postfix;
			_mLogLevel = logLevel;
			_mConsoleOutput = consoleOutput;
			_mConsoleTimestamp = consoleTimestamp;
			if (logInterval == LogIntervalType.IT_ONE_FILE && string.IsNullOrEmpty(prefix) && string.IsNullOrEmpty(postfix))
			{
				return ErrorCode.EC_ONE_FILE_LOG_REQUIRES_PREFIX_OR_POSTFIX;
			}
			try
			{
				Directory.CreateDirectory(logDirectory);
			}
			catch
			{
				return ErrorCode.EC_CANNOT_CREATE_DIRECTORY;
			}
			return ErrorCode.EC_SUCCESS;
		}

		public void Add(LogLevel logLevel, string logMessage)
		{
			PutLog(logLevel, logMessage);
		}

		public void AddDebug(string logMessage)
		{
			PutLog(LogLevel.D, logMessage);
		}

		public void AddNotice(string logMessage)
		{
			PutLog(LogLevel.N, logMessage);
		}

		public void AddWarning(string logMessage)
		{
			PutLog(LogLevel.W, logMessage);
		}

		public void AddInfo(string logMessage)
		{
			PutLog(LogLevel.I, logMessage);
		}

		public void AddError(string logMessage)
		{
			PutLog(LogLevel.E, logMessage);
		}

		public void AddBinary(string prefix, byte[] data)
		{
			PutLog(LogLevel.D, $"{prefix}: \r\n{HexDump(data)}");
		}

		public static string HexDump(byte[] bytes, int bytesPerLine = 16)
		{
			if (bytes == null) return "<null>";
			int bytesLength = bytes.Length;

			char[] HexChars = "0123456789ABCDEF".ToCharArray();

			int firstHexColumn =
				  8                   // 8 characters for the address
				+ 3;                  // 3 spaces

			int firstCharColumn = firstHexColumn
				+ bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
				+ (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
				+ 2;                  // 2 spaces 

			int lineLength = firstCharColumn
				+ bytesPerLine           // - characters to show the ascii value
				+ Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

			char[] line = (new String(' ', lineLength - Environment.NewLine.Length) + Environment.NewLine).ToCharArray();
			int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
			StringBuilder result = new StringBuilder(expectedLines * lineLength);

			for (int i = 0; i < bytesLength; i += bytesPerLine)
			{
				line[0] = HexChars[(i >> 28) & 0xF];
				line[1] = HexChars[(i >> 24) & 0xF];
				line[2] = HexChars[(i >> 20) & 0xF];
				line[3] = HexChars[(i >> 16) & 0xF];
				line[4] = HexChars[(i >> 12) & 0xF];
				line[5] = HexChars[(i >> 8) & 0xF];
				line[6] = HexChars[(i >> 4) & 0xF];
				line[7] = HexChars[(i >> 0) & 0xF];

				int hexColumn = firstHexColumn;
				int charColumn = firstCharColumn;

				for (int j = 0; j < bytesPerLine; j++)
				{
					if (j > 0 && (j & 7) == 0) hexColumn++;
					if (i + j >= bytesLength)
					{
						line[hexColumn] = ' ';
						line[hexColumn + 1] = ' ';
						line[charColumn] = ' ';
					}
					else
					{
						byte b = bytes[i + j];
						line[hexColumn] = HexChars[(b >> 4) & 0xF];
						line[hexColumn + 1] = HexChars[b & 0xF];
						line[charColumn] = (b < 32 ? '·' : (char)b);
					}
					hexColumn += 3;
					charColumn++;
				}
				result.Append(line);
			}
			return result.ToString();
		}

		private void PutLog(LogLevel logLevel, string logMessage)
		{
			if (logLevel > _mLogLevel)
			{
				return;
			}

			DateTime now = DateTime.Now;
			if (_mConsoleOutput)
			{
				if (_mConsoleTimestamp)
				{
					PrintLog($"{now:MM/dd/yy HH:mm} {logLevel} {logMessage}\r\n", logLevel);
				}
				else
				{
					PrintLog($"{logLevel} {logMessage}\r\n", logLevel);
				}
			}
			logMessage = $"{now:MM/dd/yy HH:mm} {logLevel} {logMessage}\r\n";
			string text2 = "";
			switch (_mLogInterval)
			{
				case LogIntervalType.IT_ONE_FILE:
					text2 = _mPrefix + _mPostfix + ".log";
					break;
				case LogIntervalType.IT_PER_DAY:
					text2 = _mPrefix + now.ToString("yyyyMMdd") + _mPostfix + ".log";
					break;
				case LogIntervalType.IT_PER_HOUR:
					text2 = _mPrefix + now.ToString("yyyyMMdd_HH") + _mPostfix + ".log";
					break;
				case LogIntervalType.IT_PER_MIN:
					text2 = _mPrefix + now.ToString("yyyyMMdd_HHmm") + _mPostfix + ".log";
					break;
			}
			string path = (!("\\" == _mLogDirectory[_mLogDirectory.Length - 1].ToString())) ? (_mLogDirectory + "\\" + text2) : (_mLogDirectory + text2);
			lock (_mLock)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(logMessage);
				try
				{
					FileStream fileStream = File.Open(path, FileMode.Append, FileAccess.Write, FileShare.Read);
					fileStream.Write(bytes, 0, bytes.Length);
					fileStream.Close();
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error writing to log file " + text2);
					Console.WriteLine(ex.Message);
					Console.WriteLine(ex.StackTrace);
				}
			}
		}

		private static void PrintLog(string logMessage, LogLevel logLevel)
		{
			switch (logLevel)
			{
				case LogLevel.D:
					Console.ForegroundColor = ConsoleColor.Cyan;
					break;
				case LogLevel.E:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case LogLevel.I:
					Console.ForegroundColor = ConsoleColor.White;
					break;
				case LogLevel.N:
					Console.ForegroundColor = ConsoleColor.Green;
					break;
				case LogLevel.W:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
			}
			Console.Write(logMessage);
			Console.ResetColor();
		}
	}
}
