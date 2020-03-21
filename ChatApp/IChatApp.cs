using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ChatApp
{
    [ServiceContract(CallbackContract = typeof(IMessageCallback))]
    public interface IChat
    {

        [OperationContract]
        User Add(string name);

        [OperationContract]
        void Remove(User user);

        [OperationContract(IsOneWay = true)]
        void SendMessage(string msg, User sender);

        [OperationContract]
        void Shutdown(bool saveHistory);
    }

    [ServiceContract]
    public interface IMessageCallback
    {
        [OperationContract(IsOneWay = true)]
        void GetMessage(string message);

        [OperationContract(IsOneWay = true)]
        void GetHistory(Queue<string> messages);
    }

// Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class User
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }

        internal OperationContext OperationContext;

        public User()
        {
            Id = 0;
            Name = "Server";
        }
        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}