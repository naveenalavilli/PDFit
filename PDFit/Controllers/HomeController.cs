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
        public async Task<IActionResult> GetPdfFromHTML(string htmlString)
        {
            byte[] pdfBytes;
            using (var stream = new MemoryStream())
            using (var wri = new PdfWriter(stream))
            using (var pdf = new PdfDocument(wri))
            //using (var doc = new Document(pdf))
            {
                //doc.Add(new Paragraph("Hello World!"));

                //Table table = new Table(UnitValue.CreatePercentArray(8)).UseAllAvailableWidth();

                //for (int i = 0; i < 16; i++)
                //{
                //    table.AddCell("hi");
                //}

                //doc.Add(table);
                ConverterProperties converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf("<html> <body>  <h1>My First Heading</h1>  <p>My first paragraph.</p>  </body> </html> ", pdf, converterProperties);


                //doc.Close();
                //doc.Flush();
                pdfBytes = stream.ToArray();
            }


            return await Task.FromResult( new FileContentResult(pdfBytes, "application/pdf"));

        }

        public IActionResult Privacy()
        {
            byte[] pdfBytes;
            using (var stream = new MemoryStream())
            using (var wri = new PdfWriter(stream))
            using (var pdf = new PdfDocument(wri))
            using (var doc = new Document(pdf))
            {
                //doc.Add(new Paragraph("Hello World!"));

                //Table table = new Table(UnitValue.CreatePercentArray(8)).UseAllAvailableWidth();

                //for (int i = 0; i < 16; i++)
                //{
                //    table.AddCell("hi");
                //}

                //doc.Add(table);
                ConverterProperties converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf("<html> <body>  <h1>My First Heading</h1>  <p>My first paragraph.</p>  </body> </html> ",pdf,converterProperties );


                //doc.Close();
                //doc.Flush();
                pdfBytes = stream.ToArray();
            }


            return new FileContentResult(pdfBytes, "application/pdf");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
