using Application.Helpers;
using elraee.IRepository;
using elraee.Models;
using elraee.Repositories;
using elraee.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Pkcs;
using System.Threading.Tasks;

namespace elraee.Controllers
{
    public class ContactController : Controller
    {
        IContactInfoRepo contactRepo;
        IphoneNumsRepo phoneRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ContactController (IContactInfoRepo contact , IphoneNumsRepo phone, IWebHostEnvironment webHostEnvironment)
        {
            this.contactRepo = contact;
            this.phoneRepo = phone;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult Edit()
        {
            ContactInfo contactInfo = contactRepo.GetContact();
            EditContactViewModel contact = new EditContactViewModel();
            contact.phoneNums = new List<string>();


            contact.name = contactInfo.name;
            contact.address = contactInfo.address;
            contact.facebookLink = contactInfo.facebookLink;
            contact.email = contactInfo.email;
            contact.gpsLocation = contactInfo.gpsLocation;
            contact.gpsEmbedLocation = contactInfo.gpsEmbedLocation;
            contact.whatsAppNum = contactInfo.whatsAppNum;

            foreach (phoneNums num in contactInfo.phoneNums)

                if (num != null)
                {
                    contact.phoneNums.Add(num.number);
                }

            return View("EditContact", contact);
        }

        [Authorize(Roles = "admin")]
        public IActionResult saveContact(EditContactViewModel contactInfo)
        {
            ContactInfo contact = contactRepo.GetContact();

            if(ModelState.IsValid)
            {
                contact.name = contactInfo.name;
                contact.address = contactInfo.address;
                contact.facebookLink = contactInfo.facebookLink;
                contact.email = contactInfo.email;
                contact.gpsLocation = contactInfo.gpsLocation;
                contact.gpsEmbedLocation = contactInfo.gpsEmbedLocation;
                contact.whatsAppNum = contactInfo.whatsAppNum;

                //remove old numbers list
                phoneRepo.removeOldNumbers(contact.id);
                phoneRepo.Save();

                
                    foreach(string num in contactInfo.phoneNums)

                        if(num != null)
                        {
                            phoneRepo.Add(new phoneNums { contactId = contact.id, number = num });
                        }


                contactRepo.Update(contact);
                 phoneRepo.Save();
                    
                

                ViewBag.msg = "تم التعديل بنجاح";

            }

            return View("EditContact", contactInfo);
        }

        public IActionResult ContactUS()
        {

            ContactInfo contactInfo = contactRepo.GetContact();
            EditContactViewModel contact = new EditContactViewModel();
            contact.phoneNums = new List<string>();

            contact.name = contactInfo.name;
            contact.address = contactInfo.address;
            contact.facebookLink = contactInfo.facebookLink;
            contact.email = contactInfo.email;
            contact.gpsLocation = contactInfo.gpsLocation;
            contact.gpsEmbedLocation = contactInfo.gpsEmbedLocation;
            contact.whatsAppNum = contactInfo.whatsAppNum;

            foreach (phoneNums num in contactInfo.phoneNums)

                if (num != null)
                {
                    contact.phoneNums.Add(num.number);
                }

            return View("Contact", contact);
        }




        public IActionResult GetContact()
        {

            ContactInfo contactInfo = contactRepo.GetContact();
            EditContactViewModel contact = new EditContactViewModel();
            contact.phoneNums = new List<string>();

            contact.name = contactInfo.name;
            contact.address = contactInfo.address;
            contact.facebookLink = contactInfo.facebookLink;
            contact.email = contactInfo.email;
            contact.gpsLocation = contactInfo.gpsLocation;
            contact.gpsEmbedLocation = contactInfo.gpsEmbedLocation;
            contact.whatsAppNum = contactInfo.whatsAppNum;

            foreach (phoneNums num in contactInfo.phoneNums)

                if (num != null)
                {
                    contact.phoneNums.Add(num.number);
                }

            return Json(contact);
        }

        public IActionResult eventCover()
        {
            ViewBag.img = contactRepo.GetContact().EventCover;
            return View();
        }

        public IActionResult GeteventCover()
        {
            string img = contactRepo.GetContact().EventCover;
            return Json(img);
        }
       
        public async Task<IActionResult> saveCover(EventCoverViewModel cover)
        {
            ContactInfo contact = contactRepo.GetContact();
        
            if (ModelState.IsValid)
            {
                if (contact.EventCover != null)
                {
                    //remove old from root first
                    ImageSavingHelper.RemoveImageFromRoot($"{_webHostEnvironment.WebRootPath}{contact.EventCover}");
                } 

                string img = await ImageSavingHelper.SaveOneImageAsync(cover.image, "event");
                contact.EventCover = img;
                contactRepo.Update(contact);
                contactRepo.Save();
                ViewBag.msg = "تم الحفظ بنجاح";
                ViewBag.img = contactRepo.GetContact().EventCover;
                return View("eventCover");
            }
            else
            {
                ViewBag.img = contactRepo.GetContact().EventCover;
                return View("eventCover",cover);
            }


       
        }


        public async Task<IActionResult> RemoveCover()
        {
            ContactInfo contact = contactRepo.GetContact();

            if (contact.EventCover != null)
            {
                //remove old from root first
                ImageSavingHelper.RemoveImageFromRoot($"{_webHostEnvironment.WebRootPath}{contact.EventCover}");
                contact.EventCover = null;
                contactRepo.Update(contact);
                contactRepo.Save();
                ViewBag.msg = "تم حذف اطار المناسبه";
            }
            else 
            {
                ViewBag.msg = "لا يوجد اطار في الوقت الحالي";
            }
           
            ViewBag.img = contactRepo.GetContact().EventCover;
            return View("eventCover");
        }
        


        }
}
