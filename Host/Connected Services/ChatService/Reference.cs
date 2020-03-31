﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Host.ChatService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ChatService.IChat", CallbackContract=typeof(Host.ChatService.IChatCallback))]
    public interface IChat {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChat/Add", ReplyAction="http://tempuri.org/IChat/AddResponse")]
        ChatApp.User Add(string name, string serial);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChat/Add", ReplyAction="http://tempuri.org/IChat/AddResponse")]
        System.Threading.Tasks.Task<ChatApp.User> AddAsync(string name, string serial);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChat/Remove", ReplyAction="http://tempuri.org/IChat/RemoveResponse")]
        void Remove(ChatApp.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChat/Remove", ReplyAction="http://tempuri.org/IChat/RemoveResponse")]
        System.Threading.Tasks.Task RemoveAsync(ChatApp.User user);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/SendMessage")]
        void SendMessage(string msg, ChatApp.User sender);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/SendMessage")]
        System.Threading.Tasks.Task SendMessageAsync(string msg, ChatApp.User sender);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChat/Shutdown", ReplyAction="http://tempuri.org/IChat/ShutdownResponse")]
        void Shutdown(bool saveHistory);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChat/Shutdown", ReplyAction="http://tempuri.org/IChat/ShutdownResponse")]
        System.Threading.Tasks.Task ShutdownAsync(bool saveHistory);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChat/Ban", ReplyAction="http://tempuri.org/IChat/BanResponse")]
        bool Ban(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChat/Ban", ReplyAction="http://tempuri.org/IChat/BanResponse")]
        System.Threading.Tasks.Task<bool> BanAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChat/Unban", ReplyAction="http://tempuri.org/IChat/UnbanResponse")]
        bool Unban(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChat/Unban", ReplyAction="http://tempuri.org/IChat/UnbanResponse")]
        System.Threading.Tasks.Task<bool> UnbanAsync(string name);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IChatCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/GetMessage")]
        void GetMessage(string message);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/GetHistory")]
        void GetHistory(System.Collections.Generic.Queue<string> messages);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IChatChannel : Host.ChatService.IChat, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ChatClient : System.ServiceModel.DuplexClientBase<Host.ChatService.IChat>, Host.ChatService.IChat {
        
        public ChatClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ChatClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ChatClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ChatClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ChatClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public ChatApp.User Add(string name, string serial) {
            return base.Channel.Add(name, serial);
        }
        
        public System.Threading.Tasks.Task<ChatApp.User> AddAsync(string name, string serial) {
            return base.Channel.AddAsync(name, serial);
        }
        
        public void Remove(ChatApp.User user) {
            base.Channel.Remove(user);
        }
        
        public System.Threading.Tasks.Task RemoveAsync(ChatApp.User user) {
            return base.Channel.RemoveAsync(user);
        }
        
        public void SendMessage(string msg, ChatApp.User sender) {
            base.Channel.SendMessage(msg, sender);
        }
        
        public System.Threading.Tasks.Task SendMessageAsync(string msg, ChatApp.User sender) {
            return base.Channel.SendMessageAsync(msg, sender);
        }
        
        public void Shutdown(bool saveHistory) {
            base.Channel.Shutdown(saveHistory);
        }
        
        public System.Threading.Tasks.Task ShutdownAsync(bool saveHistory) {
            return base.Channel.ShutdownAsync(saveHistory);
        }
        
        public bool Ban(string name) {
            return base.Channel.Ban(name);
        }
        
        public System.Threading.Tasks.Task<bool> BanAsync(string name) {
            return base.Channel.BanAsync(name);
        }
        
        public bool Unban(string name) {
            return base.Channel.Unban(name);
        }
        
        public System.Threading.Tasks.Task<bool> UnbanAsync(string name) {
            return base.Channel.UnbanAsync(name);
        }
    }
}
