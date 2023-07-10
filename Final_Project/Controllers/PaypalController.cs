using Final_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class PaypalController : Controller
    {
        private readonly PaypalClient _paypalClient;

        public PaypalController(PaypalClient paypalClient)
        {
            this._paypalClient = paypalClient;
        }

        public IActionResult Index()
        {
            // ViewBag.ClientId is used to get the Paypal Checkout javascript SDK
            //ViewBag.ClientId = "AciJN35r05hG5oaPQI19GqPlh4fHd3OX3LPbLjvu8J1n87KDMf21_wp-V_okmA_jSNOJql91U_H06wxD";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Order(CancellationToken cancellationToken)
        {
            try
            {
                // set the transaction price and currency
                //TempData["price"]
                var price = TempData["price"].ToString();
                var currency = "USD";

                // "reference" is the transaction key
                var reference = "INV001";

                var response = await _paypalClient.CreateOrder(price, currency, reference);

                return Ok(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    e.GetBaseException().Message
                };

                return BadRequest(error);
            }
        }

        public async Task<IActionResult> Capture(string orderId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderId);

                var reference = response.purchase_units[0].reference_id;

                // Put your logic to save the transaction here
                // You can use the "reference" variable as a transaction key

                return Ok(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    e.GetBaseException().Message
                };

                return BadRequest(error);
            }
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
