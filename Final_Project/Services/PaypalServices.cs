using PayPal.Api;
using System;

namespace Final_Project.Services
{
    public class PaypalServices:IPaypalServices
    {
        private readonly APIContext apicontext;
        private readonly Payment payment;
        private readonly IConfiguration configuration;
        public PaypalServices(IConfiguration configuration)
        {
            this.configuration = configuration;
            var clintid = configuration["AciJN35r05hG5oaPQI19GqPlh4fHd3OX3LPbLjvu8J1n87KDMf21_wp-V_okmA_jSNOJql91U_H06wxD"];
            var clintsecret = configuration["EOmG2pYUhoVg4oQR9U5UVvUAYyr4s2nVsATTBKLLkaKLJhuJVrP2XxWv-HIsIcKK6Swe9I4BzFBsLgY1"];

            var config = new Dictionary<string, string> { { "mode","sandbox" },{ "clintid", clintid },{ "clintsecret", clintsecret } };
            
            var accesstoken = new OAuthTokenCredential(clintid,clintsecret,config).GetAccessToken();

            apicontext = new APIContext(accesstoken);
            payment = new Payment { intent = "sale",payer = new Payer { payment_method = "paypal"} };
        }

        public Task<Payment> createorderasync(decimal amount, string return_url, string cansel_url)
        {
            throw new NotImplementedException();
        }
        //public Task<Payment> createorderasync(decimal amount, string return_url, string cansel_url)
        //{
        //    var apicontext = new APIContext (new OAuthTokenCredential(configuration["PayPal:clintid"], configuration["PayPal:clintsecret"]).GetAccessToken());

        //    var itemlist = new ItemList() { items = new List<Item>() };
        //    return createorderasync();
        //}
    }
}
