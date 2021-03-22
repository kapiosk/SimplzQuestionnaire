using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Threading.Tasks;

namespace SimplzQuestionnaire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRController : Controller
    {
        public QRController() { }

        [HttpGet("{data}")]
        public async Task<IActionResult> CreateSVG([FromRoute] string data)
        {
            using QRCodeGenerator qrGenerator = new();
            using QRCodeData qrCodeData = qrGenerator.CreateQrCode(Common.Utilities.Base64Decode(data), QRCodeGenerator.ECCLevel.Q);
            using PngByteQRCode qrCode = new (qrCodeData);
            byte[] qr = await Task.FromResult(qrCode.GetGraphic(20));
            return new FileContentResult(qr, "image/png");
        }
    }
}
