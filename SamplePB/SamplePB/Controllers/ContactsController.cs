﻿using System;
using System.Data;
using System.Web.Mvc;
using System.Web.Routing;
using SamplePB.DAL;
using SamplePB.Models;

namespace SamplePB.Controllers
{
    public class ContactsController : Controller
    {
        //
        // GET: /Contacts/
        //Show contact details
        public ActionResult ShowContactDetails(int id)
        {
            var objDb = new DatabaseOperations();

            var ds = objDb.SelectById(id);
            var pModel = new PersonViewModel
            {
                PersonId = id,
                LastName = ds.Tables[0].Rows[0]["LastName"].ToString(),
                FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString(),
                MiddleName = ds.Tables[0].Rows[0]["MiddleName"].ToString(),
                BirthDate = ds.Tables[0].Rows[0]["BirthDate"].ToString(),
                HomeAddress = ds.Tables[0].Rows[0]["HomeAddress"].ToString(),
                Company = ds.Tables[0].Rows[0]["Company"].ToString()

            };

            foreach (DataRow row in ds.Tables[1].Rows)
            {
                pModel.ContactNumbersViewModels.Add(
                    new ContactNumbersViewModel
                    {
                        ContactId = Convert.ToInt32(row["ID"]),
                        PersonId = Convert.ToInt32(row["PersonID"]),
                        SelectedContactType = row["SelectedContactType"].ToString(),
                        ContactNumber = row["ContactNumber"].ToString()
                    });
            }
            foreach (DataRow row in ds.Tables[2].Rows)
            {
                pModel.EmailsViewModels.Add(
                    new EmailsViewModel
                    {
                        EmailId = Convert.ToInt32(row["ID"]),
                        PersonId = Convert.ToInt32(row["PersonID"]),
                        Emails = row["EmailAddress"].ToString()
                    });
            }
        
            return View(pModel);
        }

        public ActionResult InsertContactPerson()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertContactPerson(PersonViewModel model)
        {
            if (ModelState.IsValid)
            {
                var obj = new DatabaseOperations();
                string result = obj.InsertContactPerson(model);
                ViewData["result"] = result;
                ModelState.Clear();
                return RedirectToAction("ShowAllContacts", "Contacts");
            }

            return View();
        }

        public ActionResult ShowAllContacts(PersonViewModel model)
        {
            
            var obj = new DatabaseOperations();
            model.StoreAllData = obj.SelectAllContacts(model);
            return View(model);
        }

        public ActionResult EditContact(int id)
        {
            var objDb = new DatabaseOperations();
            var ds = objDb.SelectById(id);
            var model = new PersonViewModel
            {
                PersonId = Convert.ToInt32(ds.Tables[0].Rows[0]["PersonID"].ToString()),
                LastName = ds.Tables[0].Rows[0]["LastName"].ToString(),
                FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString(),
                MiddleName = ds.Tables[0].Rows[0]["MiddleName"].ToString(),
                BirthDate = ds.Tables[0].Rows[0]["BirthDate"].ToString(),
                HomeAddress = ds.Tables[0].Rows[0]["HomeAddress"].ToString(),
                Company = ds.Tables[0].Rows[0]["Company"].ToString()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditContact(PersonViewModel model)
        {
            if (ModelState.IsValid)
            {
                var obj = new DatabaseOperations();
                obj.UpdateContactPerson(model);
                return RedirectToAction("ShowContactDetails", "Contacts",new{id=model.PersonId});
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteContact(int id)
        {
            var objDB = new DatabaseOperations();
            var ds = objDB.SelectById(id);
            var model = new PersonViewModel
            {
                PersonId = Convert.ToInt32(ds.Tables[0].Rows[0]["PersonID"].ToString()),
                LastName = ds.Tables[0].Rows[0]["LastName"].ToString(),
                FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString(),
                MiddleName = ds.Tables[0].Rows[0]["MiddleName"].ToString(),
                BirthDate = ds.Tables[0].Rows[0]["BirthDate"].ToString(),
                HomeAddress = ds.Tables[0].Rows[0]["HomeAddress"].ToString(),
                Company = ds.Tables[0].Rows[0]["Company"].ToString()
            };

            return View(model);
        }

        public ActionResult DeleteContact(PersonViewModel model)
        {
            var obj = new DatabaseOperations();
            obj.DeleteContact(model.PersonId);
            return RedirectToAction("ShowAllContacts", "Contacts");
        }

        [HttpGet]
        public ActionResult InsertPersonEmail(int id)
        {
            var objDb = new DatabaseOperations();
            var ds = objDb.SelectById(id);
            var model = new EmailsViewModel();
            model.PersonId = Convert.ToInt32(ds.Tables[0].Rows[0]["PersonID"].ToString());
            return View(model);
        }

        public ActionResult InsertPersonEmail(EmailsViewModel eModel)
        {
            if (ModelState.IsValid)
            {
                var obj = new DatabaseOperations();
                obj.AddEmails(eModel);
                ModelState.Clear();
                 return RedirectToAction("ShowContactDetails", "Contacts",new {id=eModel.PersonId});
            }
            else
            {
                return View(eModel);
            }
        }

        [HttpGet]
        public ActionResult InsertPersonContactNumber(int id)
        {
            var objDb = new DatabaseOperations();
            var ds = objDb.SelectById(id);
            var model = new ContactNumbersViewModel
            {
                PersonId = Convert.ToInt32(ds.Tables[0].Rows[0]["PersonID"].ToString())
            };


            return View(model);
        }

        public ActionResult InsertPersonContactNumber(ContactNumbersViewModel cModel)
        {
            if (ModelState.IsValid)
            {

                var obj = new DatabaseOperations();
                obj.AddContactNumbers(cModel);
                ModelState.Clear();
                return RedirectToAction("ShowContactDetails", "Contacts",new{id = cModel.PersonId});

            }
            else
            {
                
                return View(cModel);
            }
        }

        #region Contact Actions
        [HttpGet]
        public ActionResult EditContactNumber(int id)
        {
            var objDb = new DatabaseOperations();
            var ds = objDb.SelectByContactId(id);
            var model = new ContactNumbersViewModel
            {
                ContactId = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"].ToString()),
                PersonId = Convert.ToInt32(ds.Tables[0].Rows[0]["PersonID"].ToString()),
                SelectedContactType = ds.Tables[0].Rows[0]["SelectedContactType"].ToString(),
                ContactNumber = ds.Tables[0].Rows[0]["ContactNumber"].ToString()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditContactNumber(ContactNumbersViewModel model)
        {
            if (ModelState.IsValid)
            {
                var obj = new DatabaseOperations();
                obj.UpdateContactNumber(model);
                return RedirectToAction("ShowContactDetails", "Contacts", new {id = model.PersonId});
            }
            else
            {
                
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult DeleteContactNumber(int id)
        {
            var objDb = new DatabaseOperations();
            var ds = objDb.SelectByContactId(id);
            var model = new ContactNumbersViewModel
            {
                ContactId = id,
                SelectedContactType = ds.Tables[0].Rows[0]["SelectedContactType"].ToString(),
                PersonId = Convert.ToInt32(ds.Tables[0].Rows[0]["PersonID"].ToString()),
                ContactNumber = ds.Tables[0].Rows[0]["ContactNumber"].ToString()
            };

            return View(model);
        }

        public ActionResult DeleteContactNumber(ContactNumbersViewModel model)
        {
            var obj = new DatabaseOperations();

            obj.DeleteContactNumber(model.ContactId);
            return RedirectToAction("ShowContactDetails", "Contacts",new {id = model.PersonId});
        }
        #endregion

        #region Email Related Actions
        public ActionResult EditEmail(int id)
        {
            var objDb = new DatabaseOperations();
            var ds = objDb.SelectByEmailId(id);
            var model = new EmailsViewModel();

            model.EmailId = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"].ToString());
            model.PersonId = Convert.ToInt32(ds.Tables[0].Rows[0]["PersonID"].ToString());
            model.Emails = ds.Tables[0].Rows[0]["EmailAddress"].ToString();
            return View(model);
        }

        [HttpPost]
        public ActionResult EditEmail(EmailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var obj = new DatabaseOperations();

                obj.UpdateEmail(model);
                return RedirectToAction("ShowContactDetails", "Contacts", new { id = model.PersonId });
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult DeleteEmail(int id)
        {
            var objDb = new DatabaseOperations();
            var ds = objDb.SelectByEmailId(id);
            var model = new EmailsViewModel
            {
                EmailId = id,
                PersonId = Convert.ToInt32(ds.Tables[0].Rows[0]["PersonID"].ToString()),
                Emails = ds.Tables[0].Rows[0]["EmailAddress"].ToString()
            };

            return View(model);
        }

        public ActionResult DeleteEmail(EmailsViewModel model)
        {
            var obj = new DatabaseOperations();
            obj.DeleteEmail(model);
            return RedirectToAction("ShowContactDetails", "Contacts", new { id = model.PersonId });
        } 
        #endregion
    }
}