using Barcode.Models;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using static QRCoder.PayloadGenerator;

namespace Barcode.Controllers
{
    public class BarcodeController : Controller
    {
        public IActionResult Index()
        {
            QRCodeModel model = new();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(QRCodeModel model)
        {
            Payload? payload = null;
            switch (model.QRCodeType)
            {
                case 1: // compose sms
                    payload = new SMS(model.SMSPhoneNumber, model.SMSBody);
                    break;
                case 2: // compose whatsapp message
                    payload = new WhatsAppMessage(model.WhatsAppNumber, model.WhatsAppMessage);
                    break;
                case 3: //compose email
                    payload = new Mail(model.ReceiverEmailAddress, model.EmailSubject, model.EmailMessage);
                    break;
                case 4: // wifi qr code
                    payload = new WiFi(model.WIFIName, model.WIFIPassword, WiFi.Authentication.WPA);
                    break;
            }

            QRCodeGenerator qrGenerator = new();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload);
            BitmapByteQRCode qrCode = new(qrCodeData);
            string base64String = Convert.ToBase64String(qrCode.GetGraphic(20));
            model.QRImageURL = "data:image/png;base64," + base64String;
            return View("Index", model);
        }
    }
}
