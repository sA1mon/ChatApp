using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace ChatApp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Chat : IChat
    {
        private SortedSet<int> FreeIDs { get; } = new SortedSet<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private HashSet<User> Users { get; } = new HashSet<User>(11);

        public User Add(string name)
        {
            if (FreeIDs.Count == 0)
                throw new InvalidOperationException();

            var user = Users.FirstOrDefault(x => x.Name == name);
            if (user != null)
                throw new ArgumentException();

            var id = FreeIDs.First();
            FreeIDs.Remove(id);
            SendMessage($"{name} connected.", new User());

            user = new User(id, name)
            {
                OperationContext = OperationContext.Current
            };
            Users.Add(user);

            return user;
        }

        public void Remove(User user)
        {
            if (user == null) throw new ArgumentException();

            user = Users.FirstOrDefault(x => x.Id == user.Id && x.Name == user.Name);
            if (user == null) return;

            FreeIDs.Add(user.Id);
            Users.Remove(user);
            SendMessage($"{user.Name} disconnected.", new User());
        }

        public void SendMessage(string msg, User sender)
        {
            if (msg == string.Empty) throw new ArgumentException();
            if (sender == null) return;

            var message = $"[{DateTime.Now.ToShortTimeString()}] {sender.Name}: {msg}\n";
            foreach (var user in Users)
            {
                user.OperationContext.GetCallbackChannel<IMessageCallback>().GetMessage(message);
            }
        }
    }
}