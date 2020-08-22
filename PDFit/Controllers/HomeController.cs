using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iText;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PDFit.Models;
using iText.Layout.Properties;
using iText.Html2pdf;
using System.Net;

namespace PDFit.Controllers
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
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetPdfFromHTML(string htmltext)
        {
            byte[] pdfBytes;
            using (var stream = new MemoryStream())
            using (var wri = new PdfWriter(stream))
            using (var pdf = new PdfDocument(wri))
            //using (var doc = new Document(pdf))
            {
                ConverterProperties converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf(htmltext, pdf, converterProperties);

                //doc.Close();
                //doc.Flush();
                pdfBytes = stream.ToArray();
            }


            return await Task.FromResult(new FileContentResult(pdfBytes, "application/pdf"));

        }

        [HttpPost]
        public async Task<IActionResult> GetPdfFromWebPageURL(string URL)
        {
            byte[] pdfBytes;
            using (var stream = new MemoryStream())
            using (var wri = new PdfWriter(stream))
            using (var pdf = new PdfDocument(wri))
            //using (var doc = new Document(pdf))
            {

                WebClient client = new System.Net.WebClient();
                string htmlText = client.DownloadString(URL);

                ConverterProperties converterProperties = new ConverterProperties();

               // pdf.AddNewPage();
                HtmlConverter.ConvertToPdf(htmlText.Trim(), pdf, converterProperties);

                //doc.Close();
                //doc.Flush();
                pdfBytes = stream.ToArray();
            }


            return await Task.FromResult(new FileContentResult(pdfBytes, "application/pdf"));

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
    }
}
