﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace jsonResultsFromWebservice.getResultsFromWebservice {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="getResultsFromWebservice.I_GetResults")]
    public interface I_GetResults {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/I_GetResults/queryToReturnResults", ReplyAction="http://tempuri.org/I_GetResults/queryToReturnResultsResponse")]
        string queryToReturnResults(string value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/I_GetResults/queryToReturnResults", ReplyAction="http://tempuri.org/I_GetResults/queryToReturnResultsResponse")]
        System.Threading.Tasks.Task<string> queryToReturnResultsAsync(string value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/I_GetResults/queryToReturnResultsWithSequence", ReplyAction="http://tempuri.org/I_GetResults/queryToReturnResultsWithSequenceResponse")]
        string queryToReturnResultsWithSequence(string value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/I_GetResults/queryToReturnResultsWithSequence", ReplyAction="http://tempuri.org/I_GetResults/queryToReturnResultsWithSequenceResponse")]
        System.Threading.Tasks.Task<string> queryToReturnResultsWithSequenceAsync(string value);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface I_GetResultsChannel : jsonResultsFromWebservice.getResultsFromWebservice.I_GetResults, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class I_GetResultsClient : System.ServiceModel.ClientBase<jsonResultsFromWebservice.getResultsFromWebservice.I_GetResults>, jsonResultsFromWebservice.getResultsFromWebservice.I_GetResults {
        
        public I_GetResultsClient() {
        }
        
        public I_GetResultsClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public I_GetResultsClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public I_GetResultsClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public I_GetResultsClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string queryToReturnResults(string value) {
            return base.Channel.queryToReturnResults(value);
        }
        
        public System.Threading.Tasks.Task<string> queryToReturnResultsAsync(string value) {
            return base.Channel.queryToReturnResultsAsync(value);
        }
        
        public string queryToReturnResultsWithSequence(string value) {
            return base.Channel.queryToReturnResultsWithSequence(value);
        }
        
        public System.Threading.Tasks.Task<string> queryToReturnResultsWithSequenceAsync(string value) {
            return base.Channel.queryToReturnResultsWithSequenceAsync(value);
        }
    }
}
