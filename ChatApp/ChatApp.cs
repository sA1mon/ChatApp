using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;

namespace ChatApp
{
	[Serializable]
    internal class ChatData
    {
        internal HashSet<User> BannedUsers { get; set; } = new HashSet<User>();
        internal Queue<string> Messages { get; set; } = new Queue<string>(100);
		internal HashSet<User> Users { get; set; }
    }

	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class Chat : IChat
	{
        private const int MaxMessageCount = 100;
        private delegate void MessageSendHandler(string msg);
		private event MessageSendHandler SendToAll;
        private readonly ChatData _data;

        public Chat()
		{
            try
			{
				var bf = new BinaryFormatter();
                using (var fs = new FileStream("data.bin", FileMode.Open))
                {
                    _data = (ChatData)bf.Deserialize(fs);
                }
            }
			catch
            {
                _data = new ChatData();
            }

			_data.Users = new HashSet<User>();
		}

        public User Add(string name, string serial)
        {
            if (_data.BannedUsers.FirstOrDefault(x => x.Serial == serial) != null || 
                _data.Users.FirstOrDefault(x => x.Name == name) != null)
                return null;

            var user = new User(name, serial)
            {
                OperationContext = OperationContext.Current
            };
            _data.Users.Add(user);

            if (name != "Server")
                SendMessage($"{name} connected.", new User());

            SendToAll += user.GetMessage;
			user.OperationContext.GetCallbackChannel<IMessageCallback>().GetHistory(_data.Messages);

            return user;
		}

        public void Remove(User user)
        {
            user = _data.Users.FirstOrDefault(x => x.Name == user.Name);
            if (user == null) return;

			SendToAll -= user.GetMessage;
            if (_data.Users.Remove(user))
			    SendMessage($"{user.Name} disconnected.", new User());
        }

		private void AddMessageInQueue(string message)
		{
			if (_data.Messages.Count == MaxMessageCount)
			{
				_data.Messages.Dequeue();
			}

			_data.Messages.Enqueue(message);
		}

		public void SendMessage(string msg, User sender)
		{
			if (msg == null || sender == null || _data.Users.FirstOrDefault(x => x.Name == sender.Name) == null)
				return;

			var message = $"[{DateTime.Now.ToLongTimeString()}] {sender.Name}: {msg}\n";
			AddMessageInQueue(message);
			SendToAll?.Invoke(message);
		}

		public void Shutdown(bool saveHistory)
		{
			if (!saveHistory)
			{
				_data.Messages.Clear();
			}

			using (var fs = new FileStream("data.bin", FileMode.OpenOrCreate))
			{
				var bf = new BinaryFormatter();
				bf.Serialize(fs, _data);
			}
        }

        public bool Ban(string name)
        {
            var user = _data.Users.FirstOrDefault(x => x.Name == name);

            if (user == null)
                return false;

            _data.BannedUsers.Add(user);
            Remove(user);
            user.OperationContext.Channel.Abort();
            return true;
        }

        public bool Unban(string name)
        {
            var user = _data.BannedUsers.FirstOrDefault(x => x.Name == name);

            if (user == null)
                return false;

            _data.BannedUsers.Remove(user);
            return true;
        }
    }
}