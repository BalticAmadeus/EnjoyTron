﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tron.DebugClient.Demo.ClientService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BaseReq", Namespace="http://schemas.datacontract.org/2004/07/Tron.WebService.TransportClasses")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Tron.DebugClient.Demo.ClientService.CompleteLoginReq))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Tron.DebugClient.Demo.ClientService.CreatePlayerReq))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Tron.DebugClient.Demo.ClientService.WaitGameStartReq))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Tron.DebugClient.Demo.ClientService.LeaveGameReq))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Tron.DebugClient.Demo.ClientService.InitLoginReq))]
    public partial class BaseReq : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Tron.DebugClient.Demo.ClientService.ReqAuth AuthField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Tron.DebugClient.Demo.ClientService.ReqAuth Auth {
            get {
                return this.AuthField;
            }
            set {
                if ((object.ReferenceEquals(this.AuthField, value) != true)) {
                    this.AuthField = value;
                    this.RaisePropertyChanged("Auth");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ReqAuth", Namespace="http://schemas.datacontract.org/2004/07/Tron.WebService.TransportClasses")]
    [System.SerializableAttribute()]
    public partial class ReqAuth : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AuthCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ClientNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int SequenceNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int SessionIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TeamNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AuthCode {
            get {
                return this.AuthCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.AuthCodeField, value) != true)) {
                    this.AuthCodeField = value;
                    this.RaisePropertyChanged("AuthCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ClientName {
            get {
                return this.ClientNameField;
            }
            set {
                if ((object.ReferenceEquals(this.ClientNameField, value) != true)) {
                    this.ClientNameField = value;
                    this.RaisePropertyChanged("ClientName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int SequenceNumber {
            get {
                return this.SequenceNumberField;
            }
            set {
                if ((this.SequenceNumberField.Equals(value) != true)) {
                    this.SequenceNumberField = value;
                    this.RaisePropertyChanged("SequenceNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int SessionId {
            get {
                return this.SessionIdField;
            }
            set {
                if ((this.SessionIdField.Equals(value) != true)) {
                    this.SessionIdField = value;
                    this.RaisePropertyChanged("SessionId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TeamName {
            get {
                return this.TeamNameField;
            }
            set {
                if ((object.ReferenceEquals(this.TeamNameField, value) != true)) {
                    this.TeamNameField = value;
                    this.RaisePropertyChanged("TeamName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CompleteLoginReq", Namespace="http://schemas.datacontract.org/2004/07/Tron.WebService.TransportClasses")]
    [System.SerializableAttribute()]
    public partial class CompleteLoginReq : Tron.DebugClient.Demo.ClientService.BaseReq {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ChallengeResponseField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ChallengeResponse {
            get {
                return this.ChallengeResponseField;
            }
            set {
                if ((object.ReferenceEquals(this.ChallengeResponseField, value) != true)) {
                    this.ChallengeResponseField = value;
                    this.RaisePropertyChanged("ChallengeResponse");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CreatePlayerReq", Namespace="http://schemas.datacontract.org/2004/07/Tron.WebService.TransportClasses")]
    [System.SerializableAttribute()]
    public partial class CreatePlayerReq : Tron.DebugClient.Demo.ClientService.BaseReq {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="WaitGameStartReq", Namespace="http://schemas.datacontract.org/2004/07/Tron.WebService.TransportClasses")]
    [System.SerializableAttribute()]
    public partial class WaitGameStartReq : Tron.DebugClient.Demo.ClientService.BaseReq {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int PlayerIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PlayerId {
            get {
                return this.PlayerIdField;
            }
            set {
                if ((this.PlayerIdField.Equals(value) != true)) {
                    this.PlayerIdField = value;
                    this.RaisePropertyChanged("PlayerId");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LeaveGameReq", Namespace="http://schemas.datacontract.org/2004/07/Tron.WebService.TransportClasses")]
    [System.SerializableAttribute()]
    public partial class LeaveGameReq : Tron.DebugClient.Demo.ClientService.BaseReq {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int PlayerIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PlayerId {
            get {
                return this.PlayerIdField;
            }
            set {
                if ((this.PlayerIdField.Equals(value) != true)) {
                    this.PlayerIdField = value;
                    this.RaisePropertyChanged("PlayerId");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="InitLoginReq", Namespace="http://schemas.datacontract.org/2004/07/Tron.WebService.TransportClasses")]
    [System.SerializableAttribute()]
    public partial class InitLoginReq : Tron.DebugClient.Demo.ClientService.BaseReq {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BaseResp", Namespace="http://schemas.datacontract.org/2004/07/Tron.WebService.TransportClasses")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Tron.DebugClient.Demo.ClientService.CompleteLoginResp))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Tron.DebugClient.Demo.ClientService.CreatePlayerResp))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Tron.DebugClient.Demo.ClientService.WaitGameStartResp))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Tron.DebugClient.Demo.ClientService.LeaveGameResp))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Tron.DebugClient.Demo.ClientService.InitLoginResp))]
    public partial class BaseResp : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StatusField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Status {
            get {
                return this.StatusField;
            }
            set {
                if ((object.ReferenceEquals(this.StatusField, value) != true)) {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CompleteLoginResp", Namespace="http://schemas.datacontract.org/2004/07/Tron.WebService.TransportClasses")]
    [System.SerializableAttribute()]
    public partial class CompleteLoginResp : Tron.DebugClient.Demo.ClientService.BaseResp {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int SessionIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int SessionId {
            get {
                return this.SessionIdField;
            }
            set {
                if ((this.SessionIdField.Equals(value) != true)) {
                    this.SessionIdField = value;
                    this.RaisePropertyChanged("SessionId");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CreatePlayerResp", Namespace="http://schemas.datacontract.org/2004/07/Tron.WebService.TransportClasses")]
    [System.SerializableAttribute()]
    public partial class CreatePlayerResp : Tron.DebugClient.Demo.ClientService.BaseResp {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int PlayerIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PlayerId {
            get {
                return this.PlayerIdField;
            }
            set {
                if ((this.PlayerIdField.Equals(value) != true)) {
                    this.PlayerIdField = value;
                    this.RaisePropertyChanged("PlayerId");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="WaitGameStartResp", Namespace="http://schemas.datacontract.org/2004/07/Tron.WebService.TransportClasses")]
    [System.SerializableAttribute()]
    public partial class WaitGameStartResp : Tron.DebugClient.Demo.ClientService.BaseResp {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int GameIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int GameId {
            get {
                return this.GameIdField;
            }
            set {
                if ((this.GameIdField.Equals(value) != true)) {
                    this.GameIdField = value;
                    this.RaisePropertyChanged("GameId");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LeaveGameResp", Namespace="http://schemas.datacontract.org/2004/07/Tron.WebService.TransportClasses")]
    [System.SerializableAttribute()]
    public partial class LeaveGameResp : Tron.DebugClient.Demo.ClientService.BaseResp {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="InitLoginResp", Namespace="http://schemas.datacontract.org/2004/07/Tron.WebService.TransportClasses")]
    [System.SerializableAttribute()]
    public partial class InitLoginResp : Tron.DebugClient.Demo.ClientService.BaseResp {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ChallengeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Challenge {
            get {
                return this.ChallengeField;
            }
            set {
                if ((object.ReferenceEquals(this.ChallengeField, value) != true)) {
                    this.ChallengeField = value;
                    this.RaisePropertyChanged("Challenge");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ClientService.IClientService")]
    public interface IClientService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientService/InitLogin", ReplyAction="http://tempuri.org/IClientService/InitLoginResponse")]
        Tron.DebugClient.Demo.ClientService.InitLoginResp InitLogin(Tron.DebugClient.Demo.ClientService.InitLoginReq req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientService/InitLogin", ReplyAction="http://tempuri.org/IClientService/InitLoginResponse")]
        System.Threading.Tasks.Task<Tron.DebugClient.Demo.ClientService.InitLoginResp> InitLoginAsync(Tron.DebugClient.Demo.ClientService.InitLoginReq req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientService/CompleteLogin", ReplyAction="http://tempuri.org/IClientService/CompleteLoginResponse")]
        Tron.DebugClient.Demo.ClientService.CompleteLoginResp CompleteLogin(Tron.DebugClient.Demo.ClientService.CompleteLoginReq req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientService/CompleteLogin", ReplyAction="http://tempuri.org/IClientService/CompleteLoginResponse")]
        System.Threading.Tasks.Task<Tron.DebugClient.Demo.ClientService.CompleteLoginResp> CompleteLoginAsync(Tron.DebugClient.Demo.ClientService.CompleteLoginReq req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientService/CreatePlayer", ReplyAction="http://tempuri.org/IClientService/CreatePlayerResponse")]
        Tron.DebugClient.Demo.ClientService.CreatePlayerResp CreatePlayer(Tron.DebugClient.Demo.ClientService.CreatePlayerReq req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientService/CreatePlayer", ReplyAction="http://tempuri.org/IClientService/CreatePlayerResponse")]
        System.Threading.Tasks.Task<Tron.DebugClient.Demo.ClientService.CreatePlayerResp> CreatePlayerAsync(Tron.DebugClient.Demo.ClientService.CreatePlayerReq req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientService/WaitGameStart", ReplyAction="http://tempuri.org/IClientService/WaitGameStartResponse")]
        Tron.DebugClient.Demo.ClientService.WaitGameStartResp WaitGameStart(Tron.DebugClient.Demo.ClientService.WaitGameStartReq req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientService/WaitGameStart", ReplyAction="http://tempuri.org/IClientService/WaitGameStartResponse")]
        System.Threading.Tasks.Task<Tron.DebugClient.Demo.ClientService.WaitGameStartResp> WaitGameStartAsync(Tron.DebugClient.Demo.ClientService.WaitGameStartReq req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientService/LeaveGame", ReplyAction="http://tempuri.org/IClientService/LeaveGameResponse")]
        Tron.DebugClient.Demo.ClientService.LeaveGameResp LeaveGame(Tron.DebugClient.Demo.ClientService.LeaveGameReq req);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientService/LeaveGame", ReplyAction="http://tempuri.org/IClientService/LeaveGameResponse")]
        System.Threading.Tasks.Task<Tron.DebugClient.Demo.ClientService.LeaveGameResp> LeaveGameAsync(Tron.DebugClient.Demo.ClientService.LeaveGameReq req);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IClientServiceChannel : Tron.DebugClient.Demo.ClientService.IClientService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ClientServiceClient : System.ServiceModel.ClientBase<Tron.DebugClient.Demo.ClientService.IClientService>, Tron.DebugClient.Demo.ClientService.IClientService {
        
        public ClientServiceClient() {
        }
        
        public ClientServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ClientServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ClientServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ClientServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Tron.DebugClient.Demo.ClientService.InitLoginResp InitLogin(Tron.DebugClient.Demo.ClientService.InitLoginReq req) {
            return base.Channel.InitLogin(req);
        }
        
        public System.Threading.Tasks.Task<Tron.DebugClient.Demo.ClientService.InitLoginResp> InitLoginAsync(Tron.DebugClient.Demo.ClientService.InitLoginReq req) {
            return base.Channel.InitLoginAsync(req);
        }
        
        public Tron.DebugClient.Demo.ClientService.CompleteLoginResp CompleteLogin(Tron.DebugClient.Demo.ClientService.CompleteLoginReq req) {
            return base.Channel.CompleteLogin(req);
        }
        
        public System.Threading.Tasks.Task<Tron.DebugClient.Demo.ClientService.CompleteLoginResp> CompleteLoginAsync(Tron.DebugClient.Demo.ClientService.CompleteLoginReq req) {
            return base.Channel.CompleteLoginAsync(req);
        }
        
        public Tron.DebugClient.Demo.ClientService.CreatePlayerResp CreatePlayer(Tron.DebugClient.Demo.ClientService.CreatePlayerReq req) {
            return base.Channel.CreatePlayer(req);
        }
        
        public System.Threading.Tasks.Task<Tron.DebugClient.Demo.ClientService.CreatePlayerResp> CreatePlayerAsync(Tron.DebugClient.Demo.ClientService.CreatePlayerReq req) {
            return base.Channel.CreatePlayerAsync(req);
        }
        
        public Tron.DebugClient.Demo.ClientService.WaitGameStartResp WaitGameStart(Tron.DebugClient.Demo.ClientService.WaitGameStartReq req) {
            return base.Channel.WaitGameStart(req);
        }
        
        public System.Threading.Tasks.Task<Tron.DebugClient.Demo.ClientService.WaitGameStartResp> WaitGameStartAsync(Tron.DebugClient.Demo.ClientService.WaitGameStartReq req) {
            return base.Channel.WaitGameStartAsync(req);
        }
        
        public Tron.DebugClient.Demo.ClientService.LeaveGameResp LeaveGame(Tron.DebugClient.Demo.ClientService.LeaveGameReq req) {
            return base.Channel.LeaveGame(req);
        }
        
        public System.Threading.Tasks.Task<Tron.DebugClient.Demo.ClientService.LeaveGameResp> LeaveGameAsync(Tron.DebugClient.Demo.ClientService.LeaveGameReq req) {
            return base.Channel.LeaveGameAsync(req);
        }
    }
}