using System.Globalization;
using System.IO;
using System.Linq;

namespace MiniRedis
{
    abstract class MessageHandler
    {
        const char Separator = ' ';

        protected MessageHandler Handler;

        public void SetHandler(MessageHandler successor)
        {
            this.Handler = successor;
        }

        public abstract void HandleRequest(string message, StreamWriter sWriter);

        protected string GeCommand(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return null;
            }

            string[] strings = message.Split(Separator);
            if (!strings.Any())
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(strings[0]))
            {
                return null;
            }

            return strings[0];
        }

        protected string[] GetSplittedParams(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return null;
            }

            string[] @params = message.Split(Separator);
            if (!@params.Any())
            {
                return null;
            }

            return @params;
        }

        protected void SendMessageToClient(string message, StreamWriter sWriter)
        {
            sWriter.WriteLine(message);
        }
    }

    class SetMessage : MessageHandler
    {
        public override void HandleRequest(string message, StreamWriter sWriter)
        {
            if (GeCommand(message) == null)
            {
                return;
            }

            const string commandName = "SET";
            if (GeCommand(message).ToUpperInvariant() == commandName)
            {
                var par = GetSplittedParams(message);
                if (par == null)
                {
                    SendMessageToClient("Found 'SET' command, but params is coruppted", sWriter);
                    return;
                }

                const string exParamName = "EX";
                if (par.Count() == 5 && par[3] == exParamName) // with EX
                {
                    int seconds;
                    int.TryParse(par[4], out seconds);

                    if (seconds < 0)
                    {
                        seconds = 0;
                        SendMessageToClient("Found 'EX' params for 'SET' command, but EX less that 0, setted it to 0", sWriter);
                    }

                    MemoryStorageInstance.Instance.Set(par[1], par[2], seconds);
                }
                else if (par.Count() == 3)
                {
                    MemoryStorageInstance.Instance.Set(par[1], par[2]);
                    SendMessageToClient("OK", sWriter);
                }
                else
                {
                    SendMessageToClient("Found 'SET' command, but commands not in valid format", sWriter);
                }
            }
            else if (Handler != null)
            {
                Handler.HandleRequest(message, sWriter);
            }
        }
    }

    class GetMessage : MessageHandler
    {
        public override void HandleRequest(string message, StreamWriter sWriter)
        {
            if (GeCommand(message) == null)
            {
                return;
            }

            const string commandName = "GET";
            if (GeCommand(message).ToUpperInvariant() == commandName)
            {
                var par = GetSplittedParams(message);
                if (par == null)
                {
                    SendMessageToClient("Found 'GET' command, but params is coruppted", sWriter);
                    return;
                }

                if (par.Count() == 2)
                {
                    object value = MemoryStorageInstance.Instance.Get(par[1]);
                    SendMessageToClient(value == null ? "nil" : value.ToString(), sWriter);
                }
                else
                {
                    SendMessageToClient("Found 'GET' command, but commands not in valid format", sWriter);
                }
            }
            else if (Handler != null)
            {
                Handler.HandleRequest(message, sWriter);
            }
        }
    }

    class DelMessage : MessageHandler
    {
        public override void HandleRequest(string message, StreamWriter sWriter)
        {
            if (GeCommand(message) == null)
            {
                return;
            }

            const string commandName = "DEL";
            if (GeCommand(message).ToUpperInvariant() == commandName)
            {
                var par = GetSplittedParams(message);
                if (par == null)
                {
                    SendMessageToClient("Found 'GET' command, but params is coruppted", sWriter);
                    return;
                }

                if (par.Count() >= 2)
                {
                    MemoryStorageInstance.Instance.Del(par.Skip(1).ToArray());
                    SendMessageToClient("OK", sWriter);
                }
                else
                {
                    SendMessageToClient("Found 'DEL' command, but commands not in valid format", sWriter);
                }
            }
            else if (Handler != null)
            {
                Handler.HandleRequest(message, sWriter);
            }
        }
    }

    class DbsizeMessage : MessageHandler
    {
        public override void HandleRequest(string message, StreamWriter sWriter)
        {
            if (GeCommand(message) == null)
            {
                return;
            }

            const string commandName = "DBSIZE";
            if (GeCommand(message).ToUpperInvariant() == commandName)
            {
                var par = GetSplittedParams(message);
                if (par == null)
                {
                    SendMessageToClient("Found 'DBSIZE' command, but params is coruppted", sWriter);
                    return;
                }

                if (par.Count() == 1)
                {
                    int dbsize = MemoryStorageInstance.Instance.Dbsize();
                    SendMessageToClient(string.Format("DBSIZE is: '{0}'", dbsize.ToString(CultureInfo.InvariantCulture)), sWriter);
                }
                else
                {
                    SendMessageToClient("Found 'DBSIZE' command, but commands not in valid format", sWriter);
                }
            }
            else if (Handler != null)
            {
                Handler.HandleRequest(message, sWriter);
            }
        }
    }

    class IncrMessage : MessageHandler
    {
        public override void HandleRequest(string message, StreamWriter sWriter)
        {
            if (GeCommand(message) == null)
            {
                return;
            }

            const string commandName = "INCR";
            if (GeCommand(message).ToUpperInvariant() == commandName)
            {
                var par = GetSplittedParams(message);
                if (par == null)
                {
                    SendMessageToClient("Found 'INCR' command, but params is coruppted", sWriter);
                    return;
                }

                if (par.Count() == 1)
                {
                    MemoryStorageInstance.Instance.Incr(par[1]);
                    SendMessageToClient("OK", sWriter);
                }
                else
                {
                    SendMessageToClient("Found 'INCR' command, but commands not in valid format", sWriter);
                }
            }
            else if (Handler != null)
            {
                Handler.HandleRequest(message, sWriter);
            }
        }
    }

    class ZaddMessage : MessageHandler
    {
        public override void HandleRequest(string message, StreamWriter sWriter)
        {
            if (GeCommand(message) == null)
            {
                return;
            }

            const string commandName = "ZADD";
            if (GeCommand(message).ToUpperInvariant() == commandName)
            {
                SendMessageToClient("Found 'ZADD' command. COMMAND NOT IMPLIMENTED!!!", sWriter);
            }
            else if (Handler != null)
            {
                Handler.HandleRequest(message, sWriter);
            }
        }
    }

    class ZcardMessage : MessageHandler
    {
        public override void HandleRequest(string message, StreamWriter sWriter)
        {
            if (GeCommand(message) == null)
            {
                return;
            }

            const string commandName = "ZCARD";
            if (GeCommand(message).ToUpperInvariant() == commandName)
            {
                SendMessageToClient("Found 'ZCARD' command. COMMAND NOT IMPLIMENTED!!!", sWriter);
            }
            else if (Handler != null)
            {
                Handler.HandleRequest(message, sWriter);
            }
        }
    }

    class ZrankMessage : MessageHandler
    {
        public override void HandleRequest(string message, StreamWriter sWriter)
        {
            if (GeCommand(message) == null)
            {
                return;
            }

            const string commandName = "ZRANK";
            if (GeCommand(message).ToUpperInvariant() == commandName)
            {
                SendMessageToClient("Found 'ZRANK' command. COMMAND NOT IMPLIMENTED!!!", sWriter);
            }
            else if (Handler != null)
            {
                Handler.HandleRequest(message, sWriter);
            }
        }
    }

    class ZrangeMessage : MessageHandler
    {
        public override void HandleRequest(string message, StreamWriter sWriter)
        {
            if (GeCommand(message) == null)
            {
                return;
            }

            const string commandName = "ZRANGE";
            if (GeCommand(message).ToUpperInvariant() == commandName)
            {
                SendMessageToClient("Found 'ZRANGE' command. COMMAND NOT IMPLIMENTED!!!", sWriter);
            }
            else if (Handler != null)
            {
                Handler.HandleRequest(message, sWriter);
            }
        }
    }
}
