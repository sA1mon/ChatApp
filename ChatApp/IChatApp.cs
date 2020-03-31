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
        User Add(string name, string serial);

        [OperationContract]
        void Remove(User user);

        [OperationContract(IsOneWay = true)]
        void SendMessage(string msg, User sender);

        [OperationContract]
        void Shutdown(bool saveHistory);

        [OperationContract]
        bool Ban(string name);

        [OperationContract]
        bool Unban(string name);
    }

    [ServiceContract]
    public interface IMessageCallback
    {
        [OperationContract(IsOneWay = true)]
        void GetMessage(string message);

        [OperationContract(IsOneWay = true)]
        void GetHistory(Queue<string> messages);
    }

    [Serializable]
    [DataContract]
    public class User
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Serial { get; set; }

        [NonSerialized]
        internal OperationContext OperationContext;

        public void GetMessage(string message)
        {
            OperationContext.GetCallbackChannel<IMessageCallback>().GetMessage(message);
        }

        public User()
        {
            Name = "Server";
            Serial = "";
        }

        public User(string name, string serial)
        {
            Name = name;
            Serial = serial;
        }
    }
}