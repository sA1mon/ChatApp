using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;

namespace ChatApp
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class Chat : IChat
	{
		private SortedSet<int> FreeIDs { get; } = new SortedSet<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
		private HashSet<User> Users { get; } = new HashSet<User>(11);
		private Queue<string> Messages { get; }

        public Chat()
        {
            try
            {
                using (var fs = new FileStream("dialog.bin", FileMode.Open))
                {
                    var bf = new BinaryFormatter();
                    Messages = (Queue<string>) bf.Deserialize(fs);
                }
            }
            catch
            {
				Messages = new Queue<string>(100);
            }
        }

		public User Add(string name)
		{
			if (FreeIDs.Count == 0)
				throw new FaultException("No free IDs");

			var user = Users.FirstOrDefault(x => x.Name == name);
			if (user != null)
				throw new FaultException("User name is busy.");

			var id = FreeIDs.First();
			FreeIDs.Remove(id);
			if (name != "Admin") SendMessage($"{name} connected.", new User());

			user = new User(id, name)
			{
				OperationContext = OperationContext.Current
			};
			Users.Add(user);
            user.OperationContext.GetCallbackChannel<IMessageCallback>().GetHistory(Messages);

			return user;
		}

		public void Remove(User user)
		{
			if (user == null) throw new FaultException("User unknown");

			user = Users.FirstOrDefault(x => x.Name == user.Name);
			if (user == null) throw new FaultException("User unknown");

			FreeIDs.Add(user.Id);
			Users.Remove(user);
			if (user.Id != 0) SendMessage($"{user.Name} disconnected.", new User());
		}

        private void AddMessageInQueue(string message)
        {
            if (Messages.Count == 100)
            {
                Messages.Dequeue();
            }

			Messages.Enqueue(message);
        }

		public void SendMessage(string msg, User sender)
		{
			if (msg == string.Empty) throw new FaultException("Empty message");
            if (sender == null) return;

			if (sender.Id != 0 || sender.Name != "Server")
            {
                sender = Users.FirstOrDefault(x => x.Name == sender.Name && x.Id == sender.Id);
                if (sender == null) throw new FaultException("Sender not found");
            }

			var message = $"[{DateTime.Now.ToLongTimeString()}] {sender.Name}: {msg}\n";
			AddMessageInQueue(message);
			foreach (var user in Users)
			{
				user.OperationContext.GetCallbackChannel<IMessageCallback>().GetMessage(message);
			}
		}

        public void Shutdown(bool saveHistory)
        {
            if (!saveHistory)
            {
				Messages.Clear();
            }

			using (var fs = new FileStream("dialog.bin", FileMode.OpenOrCreate))
			{
				var bf = new BinaryFormatter();
				bf.Serialize(fs, Messages);
			}
        }
    }
}