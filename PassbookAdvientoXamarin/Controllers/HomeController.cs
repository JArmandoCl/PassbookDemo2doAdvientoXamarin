using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Passbook.Generator;
using Passbook.Generator.Fields;

namespace PassbookAdvientoXamarin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GeneratePass()
        {
            try
            {
                PassGenerator generator = new PassGenerator();
                PassGeneratorRequest request = new PassGeneratorRequest();
                request.PassTypeIdentifier = "pass.passbook.advientoxamarin";
                request.TeamIdentifier = "[AQUÍ VA TU TEAM IDENTIFIER DE TU CUENTA DE APPLE DEVELOPER]";
                request.Description = "Pase para 2do Adviento Xamarin";
                request.OrganizationName = "Un simple developer";
                request.LogoText = "USD";
                request.BackgroundColor = "#FFFFFF";
                request.LabelColor = "#228581";
                request.ForegroundColor = "#228581";

                //Certificados
                request.Certificate =
                    System.IO.File.ReadAllBytes(Server.MapPath("~/Resources/CertificatesPassAdviento.p12"));
                request.CertificatePassword = "[AQUÍ VA EL PASS DE TU CERTIFICADO .p12]";
                request.AppleWWDRCACertificate =
                    System.IO.File.ReadAllBytes(Server.MapPath("~/Resources/AppleWWDRCAG3.cer"));
                request.Style = PassStyle.Generic;

                request.Images.Add(PassbookImage.Icon, System.IO.File.ReadAllBytes(Server.MapPath("~/Resources/icon.png")));
                request.Images.Add(PassbookImage.Icon2X, System.IO.File.ReadAllBytes(Server.MapPath("~/Resources/icon@2x.png")));
                request.Images.Add(PassbookImage.Logo, System.IO.File.ReadAllBytes(Server.MapPath("~/Resources/pc.png")));
                request.Images.Add(PassbookImage.Logo2X, System.IO.File.ReadAllBytes(Server.MapPath("~/Resources/pc@2x.png")));
                request.Images.Add(PassbookImage.Thumbnail, System.IO.File.ReadAllBytes(Server.MapPath("~/Resources/code.png")));
                request.Images.Add(PassbookImage.Thumbnail2X, System.IO.File.ReadAllBytes(Server.MapPath("~/Resources/code@2x.png")));
                request.AddPrimaryField(new StandardField("canal", "Nombre de canal", "Un simple developer"));
                request.AddSecondaryField(new StandardField("plataforma", "Plataforma", "Youtube"));
                request.AddSecondaryField(new StandardField("evento", "Evento", "2do. Calendario de Adviento Xamarin"));
                request.AddSecondaryField(new StandardField("developer", "Developer", "Armando Cárdenas"));
                
                request.SerialNumber =Guid.NewGuid().ToString() ;
                request.TransitType = TransitType.PKTransitTypeAir;
                request.SetBarcode(BarcodeType.PKBarcodeFormatQR,"Bienvenidos al 2do. calendario de adviento de Xamarin","UTF-8","QR de bienvenida");
                request.AddHeaderField(new StandardField("fecha","Fecha",DateTime.Now.ToString("dd/MM/yyyy")));

                byte[] generatedPass = generator.Generate(request);
                FileContentResult result=new FileContentResult(generatedPass,"application/vnd.apple.pkpass")
                {
                    FileDownloadName = "UnSimpleDeveloperPase"
                };

                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Content($"No pudimos generar el pase :(. Error> {e.Message}");
            }
        }
    }
}