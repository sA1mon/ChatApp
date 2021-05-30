using Rsa;
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
        internal HashSet<User> Users { get; set; }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Chat : IChat
    {
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

        public User Add(string name, string hardSerial, PublicKey key)
        {
            if (_data.BannedUsers.FirstOrDefault(x => x.Serial == hardSerial) != null ||
                _data.Users.FirstOrDefault(x => x.Name == name) != null)
                return null;

            var user = new User(name, hardSerial, key)
            {
                OperationContext = OperationContext.Current
            };
            _data.Users.Add(user);

            return user;
        }

        public void Remove(User user)
        {
            user = _data.Users.FirstOrDefault(x => x.Name == user.Name);
            if (user == null)
                return;

            _data.Users.Remove(user);
        }

        public List<User> GetUsers()
        {
            return _data.Users.ToList();
        }

        public void SendMessage(byte[] msg, User sender, User receiver)
        {
            if (msg == null ||
                msg.Length == 0)
                return;

            _data.Users.FirstOrDefault(x => x.Name == receiver.Name).GetMessage(msg);
        }

        public void Shutdown()
        {
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