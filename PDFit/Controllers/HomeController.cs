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
using PDFit.Middleware;
using Microsoft.AspNetCore.Http;

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


        /// <summary>
        /// Converts HTML String to PDF.
        /// </summary>
        /// <param name="htmltext"></param>
        /// <returns> PDF Document</returns>
        [HttpPost]
        [RequestsLimit(Name = "Limit Calls to Convert HTML.", Seconds = 5)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPDFFromHTML(string htmltext)
        {
            byte[] pdfBytes;
            using (var stream = new MemoryStream())
            using (var wri = new PdfWriter(stream))
            using (var pdf = new PdfDocument(wri))
            {
                ConverterProperties converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf(htmltext, pdf, converterProperties);

                pdfBytes = stream.ToArray();
            }
            return await Task.FromResult(new FileContentResult(pdfBytes, "application/pdf"));
        }

        /// <summary>
        /// Convert a webpage to PDF
        /// </summary>
        /// <param name="URL"></param>
        /// <returns>PDF Document</returns>
        [HttpPost]
        [RequestsLimit(Name = "Limit Calls To Convert URL.", Seconds = 5)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPDFFromWebPageURL(string URL)
        {
            Uri urlCheck = new Uri(URL);
            WebRequest request = WebRequest.Create(urlCheck);
            request.Timeout = 15000;
            WebResponse response;

            //Check if page exists
            try
            {
                response = request.GetResponse();
            }
            catch (Exception)
            {
                return NotFound("The specified page isn't found.");
            }

            //Lets try to covert that page to PDF
            try
            {
                byte[] pdfBytes;
                using (var stream = new MemoryStream())
                using (var wri = new PdfWriter(stream))
                using (var pdf = new PdfDocument(wri))
                {
                    //Get HTML Content from the URL
                    WebClient client = new System.Net.WebClient();
                    string htmlText = client.DownloadString(URL);
                    
                    //Invoke iText PDF writing method
                    ConverterProperties converterProperties = new ConverterProperties();
                    HtmlConverter.ConvertToPdf(htmlText.Trim(), pdf, converterProperties);

                    pdfBytes = stream.ToArray();
                }
                return await Task.FromResult(new FileContentResult(pdfBytes, "application/pdf"));
            }
            catch (Exception ex)
            {
                return NotFound("The specified page isn't found.");
            }

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
