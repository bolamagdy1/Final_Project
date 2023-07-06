using PayPal.Api;
namespace Final_Project.Services
{
    public interface IPaypalServices
    {
        Task<Payment> createorderasync(Decimal amount, string return_url, string cansel_url);
    }
}
