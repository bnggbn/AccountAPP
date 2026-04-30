using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AccountAPP.Logging
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }

    /// <summary>
    /// JSON-lines logger. Each entry is one JSON object per line so a message
    /// containing a newline cannot fake a second log entry.
    /// Log files are written to {BaseDir}\logs\yyyy-MM-dd.log and kept for 30 days.
    /// </summary>
    public class AppLogger
    {
        private static readonly string LogDir =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

        private const int MaxMessageLength = 4000;
        private const int RetentionDays = 30;

        // Single lock shared across all instances to serialise file access.
        private static readonly object _fileLock = new object();

        // ------------------------------------------------------------------ //
        //  Public API                                                          //
        // ------------------------------------------------------------------ //

        public void Debug(string message)
        {
            Write(LogLevel.Debug, message);
        }

        public void Info(string message)
        {
            Write(LogLevel.Info, message);
        }

        public void Warning(string message)
        {
            Write(LogLevel.Warning, message);
        }

        public void Error(string message)
        {
            Write(LogLevel.Error, message);
        }

        public void Error(string message, Exception error)
        {
            Write(LogLevel.Error,
                  error == null ? message : message + ": " + error.ToString());
        }

        // ------------------------------------------------------------------ //
        //  Core                                                                //
        // ------------------------------------------------------------------ //

        public void Write(LogLevel level, string message)
        {
            // Fire-and-forget: logging must never crash the caller.
            try
            {
                string entry = Format(level, message);
                AppendLine(entry);
            }
            catch { }
        }

        public string Format(LogLevel level, string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            string label = LevelLabel(level);
            string sanitized = SanitizeLogMessage(message);

            return "{"
                + "\"timestamp\":" + EncodeJsonString(timestamp) + ","
                + "\"level\":"     + EncodeJsonString(label)     + ","
                + "\"message\":"   + EncodeJsonString(sanitized)
                + "}";
        }

        // ------------------------------------------------------------------ //
        //  File I/O                                                            //
        // ------------------------------------------------------------------ //

        private void AppendLine(string line)
        {
            lock (_fileLock)
            {
                Directory.CreateDirectory(LogDir);

                string filePath = Path.Combine(LogDir, DailyFileName(DateTime.Now));
                File.AppendAllText(filePath, line + "\n", Encoding.UTF8);

                CleanupOldLogs();
            }
        }

        private static string DailyFileName(DateTime time)
        {
            return time.ToString("yyyy-MM-dd") + ".log";
        }

        private static void CleanupOldLogs()
        {
            try
            {
                if (!Directory.Exists(LogDir)) return;

                DateTime cutoff = DateTime.Now.AddDays(-RetentionDays);
                foreach (string file in Directory.GetFiles(LogDir, "????-??-??.log"))
                {
                    if (File.GetLastWriteTime(file) < cutoff)
                        File.Delete(file);
                }
            }
            catch { }
        }

        // ------------------------------------------------------------------ //
        //  Message sanitisation                                                //
        // ------------------------------------------------------------------ //

        private static string SanitizeLogMessage(string input)
        {
            // Strip ANSI colour / control sequences.
            string withoutAnsi = Regex.Replace(input,
                @"\x1B\[[0-?]*[ -/]*[@-~]", string.Empty);

            string normalized = SafeUnicode(withoutAnsi);

            if (normalized.Length <= MaxMessageLength)
                return normalized;

            return normalized.Substring(0, MaxMessageLength) + "... [truncated]";
        }

        /// <summary>
        /// Escapes newlines and tabs to keep each log entry on one physical line,
        /// and replaces disallowed code points with Unicode escape sequences.
        /// </summary>
        private static string SafeUnicode(string input)
        {
            var sb = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                int rune;

                // Handle UTF-16 surrogate pairs.
                if (char.IsHighSurrogate(input[i]) && i + 1 < input.Length
                    && char.IsLowSurrogate(input[i + 1]))
                {
                    rune = char.ConvertToUtf32(input[i], input[i + 1]);
                    i++;
                }
                else
                {
                    rune = input[i];
                }

                if (rune == 0x09) { sb.Append(@"\t"); continue; }
                if (rune == 0x0A) { sb.Append(@"\n"); continue; }
                if (rune == 0x0D) { sb.Append(@"\r"); continue; }

                if (IsAllowedRune(rune))
                {
                    if (rune <= 0xFFFF)
                        sb.Append((char)rune);
                    else
                        sb.Append(char.ConvertFromUtf32(rune));
                }
                else
                {
                    sb.Append(EscapeRune(rune));
                }
            }
            return sb.ToString();
        }

        private static bool IsAllowedRune(int r)
        {
            if (r < 0x20 || r == 0x7F)             return false;
            if (r >= 0x80   && r <= 0x9F)           return false;
            if (r >= 0xD800 && r <= 0xDFFF)         return false;
            if (r >= 0xE000 && r <= 0xF8FF)         return false;
            if (r >= 0xF0000 && r <= 0xFFFFD)       return false;
            if (r >= 0x100000 && r <= 0x10FFFD)     return false;

            switch (r)
            {
                case 0x034F: case 0x061C: case 0x115F: case 0x1160:
                case 0x17B4: case 0x17B5: case 0x180E:
                case 0x200B: case 0x200C: case 0x200D:
                case 0x200E: case 0x200F:
                case 0x202A: case 0x202B: case 0x202C: case 0x202D: case 0x202E:
                case 0x2060: case 0x2061: case 0x2062: case 0x2063: case 0x2064:
                case 0x2066: case 0x2067: case 0x2068: case 0x2069:
                case 0x206A: case 0x206B: case 0x206C: case 0x206D: case 0x206E: case 0x206F:
                case 0xFEFF:
                case 0xFFF9: case 0xFFFA: case 0xFFFB:
                    return false;
            }
            return true;
        }

        private static string EscapeRune(int rune)
        {
            if (rune <= 0xFFFF)
                return @"\u" + rune.ToString("x4");

            return @"\u{" + rune.ToString("x") + "}";
        }

        // ------------------------------------------------------------------ //
        //  JSON helpers                                                        //
        // ------------------------------------------------------------------ //

        /// <summary>
        /// Wraps a string in JSON double-quotes and escapes \ and " characters.
        /// The message is already sanitised so no other characters need escaping.
        /// </summary>
        private static string EncodeJsonString(string value)
        {
            var sb = new StringBuilder(value.Length + 2);
            sb.Append('"');
            foreach (char c in value)
            {
                if (c == '\\') { sb.Append(@"\\"); continue; }
                if (c == '"')  { sb.Append("\\\""); continue; }
                sb.Append(c);
            }
            sb.Append('"');
            return sb.ToString();
        }

        // ------------------------------------------------------------------ //
        //  Helpers                                                             //
        // ------------------------------------------------------------------ //

        private static string LevelLabel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:   return "DEBUG";
                case LogLevel.Info:    return "INFO";
                case LogLevel.Warning: return "WARN";
                case LogLevel.Error:   return "ERROR";
                default:               return "INFO";
            }
        }
    }
}
