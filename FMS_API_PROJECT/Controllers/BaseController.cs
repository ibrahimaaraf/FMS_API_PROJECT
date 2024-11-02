using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMS_API_PROJECT.Controllers
{
    public class BaseController : Controller
    {
        #region [/////////// Cookies ///////////]

        public void SetCookie(string key, string value, int days)
        {
            // Create a new cookie
            HttpCookie cookie = new HttpCookie(key);
            cookie.Value = value;

            // Set the cookie's expiration date
            cookie.Expires = DateTime.Now.AddDays(days);

            // Add the cookie to the response
            Response.Cookies.Add(cookie);

        }
        public string GetCookie(string key)
        {
            string cookieValue = string.Empty;
            // Check if the cookie exists
            if (Request.Cookies[key] != null)
            {
                // Get the cookie's value
                cookieValue = Request.Cookies[key].Value;

            }
            return cookieValue;
        }
        public void DeleteCookie(string key)
        {
            // Check if the cookie exists
            if (Request.Cookies[key] != null)
            {
                // Expire the cookie by setting its expiration date to a date in the past
                Response.Cookies[key].Expires = DateTime.Now.AddDays(-1);
            }
        }

        protected void DeleteAllCookies()
        {
            // Get all existing cookies
            HttpCookieCollection cookies = Request.Cookies;


            foreach (string cookieName in cookies.AllKeys)
            {
                if (cookieName.Contains("_"))
                {
                    HttpCookie cookie = new HttpCookie(cookieName);
                    cookie.Expires = DateTime.Now.AddDays(-1); // Set an expired date
                    HttpContext.Response.Cookies.Add(cookie); // Add the expired cookie to the response
                }
            }
        }


        #endregion

    }
}