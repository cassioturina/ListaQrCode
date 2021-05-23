using ListaQrCode.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;

namespace ListaQrCode.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var listaProdutos = new List<string>
            {
                "Feijao",
                "Oleo",
                "Batata"
            };

            var listaQrCodeBase64 = new List<string>();

            foreach (var item in listaProdutos)
            {
                listaQrCodeBase64.Add(GerarQRCodeBase64(item));
            }


            ViewBag.Produtos = listaQrCodeBase64;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //https://github.com/codebude/QRCoder
        public static string GerarQRCodeBase64(string url)
        {
            Url generator = new Url(url);
            string payload = generator.ToString();

            QRCodeGenerator _qrCode = new();
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return $"data:image/png;base64,{Convert.ToBase64String(BitmapToBytesCode(qrCodeImage))}";
        }

        public static Bitmap GerarQRCodeImage(string url)
        {
            Url generator = new(url);
            string payload = generator.ToString();
            QRCodeGenerator _qrCode = new();
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new(_qrCodeData);
            return qrCode.GetGraphic(20);
        }

        private static byte[] BitmapToBytesCode(Bitmap image)
        {
            using MemoryStream stream = new();
            image.Save(stream, ImageFormat.Png);
            return stream.ToArray();
        }
    }
}
