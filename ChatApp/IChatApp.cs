using Rsa;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ChatApp
{
    [ServiceContract(CallbackContract = typeof(IMessageCallback))]
    public interface IChat
    {
        [OperationContract]
        User Add(string name, string hardSerial, PublicKey key);

        [OperationContract]
        void Remove(User user);

        [OperationContract]
        List<User> GetUsers();

        [OperationContract(IsOneWay = true)]
        void SendMessage(byte[] msg, User sender, User receiver);

        [OperationContract]
        void Shutdown();

        [OperationContract]
        bool Ban(string name);

        [OperationContract]
        bool Unban(string name);
    }

    [ServiceContract]
    public interface IMessageCallback
    {
        [OperationContract(IsOneWay = true)]
        void GetMessage(byte[] message, string senderName);
    }

    [Serializable]
    [DataContract]
    public class User
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Serial { get; set; }

        [DataMember]
        public PublicKey Key { get; set; }

        [NonSerialized]
        internal OperationContext OperationContext;

        public void GetMessage(byte[] message, string senderName)
        {
            OperationContext.GetCallbackChannel<IMessageCallback>().GetMessage(message, senderName);
        }

        public User()
        {
            Name = "Server";
            Serial = string.Empty;
            Key = default;
        }

        public User(string name, string serial, PublicKey key)
        {
            Name = name;
            Serial = serial;
            Key = key;
        }
    }
}