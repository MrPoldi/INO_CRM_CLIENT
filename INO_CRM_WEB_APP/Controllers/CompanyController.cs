﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using INO_CRM_API.Models;
using INO_CRM_WEB_APP.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace INO_CRM_WEB_APP.Controllers
{
    public class CompanyController : Controller
    {
        public async Task<CompanyModel> GetCompanyAsync(int id)
        {
            CompanyModel company = new CompanyModel();

            HttpResponseMessage responseMessage = await ApiHelper.GetAsync("api/companies/" + id, HttpContext.Session.GetString("token"));

            if (responseMessage.IsSuccessStatusCode)
            {
                string userResponse = responseMessage.Content.ReadAsStringAsync().Result;
                company = JsonConvert.DeserializeObject<CompanyModel>(userResponse);
            }

            return company;
        }

        public async Task<List<CompanyModel>> GetCompaniesAsync(bool paginated, int id = 0)
        {
            List<CompanyModel> companies = new List<CompanyModel>();
            HttpResponseMessage responseMessage;
            if (!paginated)
            {
                responseMessage = await ApiHelper.GetAsync("api/companies", HttpContext.Session.GetString("token"));
            }
            else
            {
                responseMessage = await ApiHelper.GetAsync("api/companies/page/" + id, HttpContext.Session.GetString("token"));
            }

            if (responseMessage.IsSuccessStatusCode)
            {
                string userResponse = responseMessage.Content.ReadAsStringAsync().Result;
                companies = JsonConvert.DeserializeObject<List<CompanyModel>>(userResponse);
            }
            return companies;
        }

        public async Task<List<CompanyModel>> GetCompaniesByBranch(int id, string searchBranch)
        {
            List<CompanyModel> companies = new List<CompanyModel>();
            HttpResponseMessage response;
            response = await ApiHelper.GetAsync("api/companies/page/" + id + "?searchBranch=" + searchBranch, HttpContext.Session.GetString("token"));

            if (response.IsSuccessStatusCode)
            {
                string userResponse = response.Content.ReadAsStringAsync().Result;
                companies = JsonConvert.DeserializeObject<List<CompanyModel>>(userResponse);
            }
            return companies;
        }

        public async Task<int> GetPageCount()
        {
            int pageCount = 0;
            HttpResponseMessage responseMessage = await ApiHelper.GetAsync("api/companies/pages", HttpContext.Session.GetString("token"));
            if (responseMessage.IsSuccessStatusCode)
            {
                string userResponse = responseMessage.Content.ReadAsStringAsync().Result;
                pageCount = int.Parse(userResponse);                
            }
            return pageCount;
        }

        public async Task<int> GetPageCountFiltered(string searchBranch)
        {
            int pageCount = 0;
            HttpResponseMessage responseMessage = await ApiHelper.GetAsync("api/companies/pages?searchBranch=" + searchBranch, HttpContext.Session.GetString("token"));
            if (responseMessage.IsSuccessStatusCode)
            {
                string userResponse = responseMessage.Content.ReadAsStringAsync().Result;
                pageCount = int.Parse(userResponse);
            }
            return pageCount;
        }

        // GET: Company
        public async  Task<ActionResult> Index()
        {
            List<CompanyModel> companies = await GetCompaniesAsync(paginated: false);
            return View(companies);
        }

        public async Task<ActionResult> Page(int id, string searchBranch)
        {
            List<CompanyModel> companies;
            int pageCount;

            if(searchBranch == null)
            {
                companies = await GetCompaniesAsync(true, id);
                pageCount = await GetPageCount();                
            }
            else
            {
                companies = await GetCompaniesByBranch(id, searchBranch);
                pageCount = await GetPageCountFiltered(searchBranch);
            }

            if(pageCount < id)
            {
                ViewBag.currentPage = pageCount;
            }
            else
            {
                ViewBag.currentPage = id;
            }

            ViewBag.searchBranch = searchBranch;
            ViewBag.pageCount = pageCount;
            
            return View(companies);
        }

        // GET: Company/Details/5
        public async Task<ActionResult> Details(int id)
        {
            CompanyModel company = await GetCompanyAsync(id);
            return View(company);
        }

        // GET: Company/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Company/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCompany(CompanyModel company)
        {
            company.User = new UserModel();
            company.User.Login = HttpContext.Session.GetString("login");

            try
            {
                HttpResponseMessage response = await ApiHelper.PostAsync("api/companies", company);

                return RedirectToAction(nameof(Page), new { id = 1 });
            }
            catch
            {
                return View();
            }
        }

        // GET: Company/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            CompanyModel company = await GetCompanyAsync(id);
            return View(company);
        }

        // POST: Company/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCompany(int id, CompanyModel company)
        {
            company.CompanyId = id;            
            try
            {
                HttpResponseMessage responseMessage = await ApiHelper.PutAsync("api/companies/" + id, company, HttpContext.Session.GetString("token"));

                return RedirectToAction(nameof(Page), new { id = 1 });
            }
            catch
            {
                return View();
            }
        }

        // GET: Company/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            CompanyModel company = await GetCompanyAsync(id);
            return View(company);
        }

        // POST: Company/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCompany(int id)
        {
            try
            {
                HttpResponseMessage responseMessage = await ApiHelper.DeleteAsync("api/companies/" + id, HttpContext.Session.GetString("token"));

                return RedirectToAction(nameof(Page), new { id = 1 });
            }
            catch
            {
                return View();
            }
        }
    }
}