using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TenmoClient.Models;
using RestSharp.Authenticators;

namespace TenmoClient
{
    public class APIService
    {

        private readonly string APIURL = "https://localhost:44315/";
        private readonly RestClient client = new RestClient();
        //private ApiUser user = new ApiUser();

        //public APIService (string api_url)
        //{
        //    APIURL = api_url;
        //}

        public Accounts GetBalance(int id)
        {
            RestRequest request = new RestRequest(APIURL + "accounts/"+ id + "/balance");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<Accounts> response = client.Get<Accounts>(request);
            ProcessErrorResponse(response);
            return response.Data;
        }

        // use API user or transfer user? Maybe don't need to use extra created method for only getting UserID/Username
        //Might be able to only return UserID/Username from already made User method
        public List<ApiUser> ViewUsers()
        {
            RestRequest request = new RestRequest(APIURL +"transfer/viewallusers");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<List<ApiUser>> response = client.Get<List<ApiUser>>(request);
            ProcessErrorResponse(response);
            return response.Data;
        }

        public List<Transfers> ViewTransfers(int id)
        {
            RestRequest request = new RestRequest(APIURL + "transfer/" + id + "/viewalltransfers");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<List<Transfers>> response = client.Get<List<Transfers>>(request);
            ProcessErrorResponse(response);
            return response.Data;
        }

        public Transfers ViewTransferByID(int id)
        {
            RestRequest request = new RestRequest(APIURL + "transfer/" + id + "/viewtransfer");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<Transfers> response = client.Get<Transfers>(request);
            ProcessErrorResponse(response);
            return response.Data;
        }

        public List<Transfers> ViewPendingTransfers(int id)
        {
            RestRequest request = new RestRequest(APIURL + "transfer/" + id + "/viewpendingtransfers");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<List<Transfers>> response = client.Get<List<Transfers>>(request);
            ProcessErrorResponse(response);
            return response.Data;
        }

        public Transfers CreateTransfer(Transfers transfer)
        {
            int id = transfer.AccountFrom;
            RestRequest request = new RestRequest(APIURL + "transfer/" + id + "/transfer");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            request.AddJsonBody(transfer);
            IRestResponse<Transfers> response = client.Post<Transfers>(request);
            ProcessErrorResponse(response);
            return response.Data;
        }

        public Transfers ApproveTransfer(int id)
        {
            RestRequest request = new RestRequest(APIURL + "transfer/" + id + "/approvetransfer");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<Transfers> response = client.Put<Transfers>(request);
            ProcessErrorResponse(response);
            return response.Data;
        }

        public void ProcessErrorResponse(IRestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new HttpRequestException("Error occured can't reach server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new HttpRequestException("Error occured recieved non success response:" + (int)response.StatusCode);
            }
        }
    }
}
