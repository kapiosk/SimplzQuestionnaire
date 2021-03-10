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
            using QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            using SvgQRCode qrCode = new(qrCodeData);
            string qrCodeAsSvg = await Task.FromResult(qrCode.GetGraphic(20));
            return Content(qrCodeAsSvg, "image/svg+xml; charset=utf-8");
        }
    }
}
