using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Validate.Mvc.IntegrationTests.Models;

namespace Validate.Mvc.IntegrationTests.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        public ActionResult CreateContact_1()
        {
            return View("CreateContact", new Contact_1 { CurrentJob = new Job() });
        }

        [HttpPost]
        public ActionResult SubmitContact_1(Contact_1 contact1)
        {
            if (!ModelState.IsValid)
            {   
                return View("CreateContact", contact1);
            }
            return View("CreateContact", new Contact_1 { CurrentJob = new Job() });
        }

        [HttpGet]
        public ActionResult CreateContact_2()
        {
            return View("CreateContact", new Contact_2 { CurrentJob = new Job() });
        }

        [HttpPost]
        public ActionResult SubmitContact_2(Contact_2 contact1)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateContact", contact1);
            }
            return View("CreateContact", new Contact_2 { CurrentJob = new Job() });
        }

    }
}
