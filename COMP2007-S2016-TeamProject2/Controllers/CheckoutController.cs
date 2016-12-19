﻿/* File Name: COMP2007_S2016_TeamProject2
 * Author: Yandong Wang  200277628
 * File Description: Create a website that allow customer to view the cuisines and order them online:
 * View food types, food items, food items details; add items to Cart; remove food items from cart;
 * Fill out shipping information and promotion code; submit order.
 * WebSite Name: Hai Di Lao Hot Pot
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COMP2007_S2016_TeamProject2.Models;

namespace COMP2007_S2016_TeamProject2.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        RestaurantStoreContext storeDB = new RestaurantStoreContext();


        //
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            return View();
        }
        //

        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);

            try
            {


                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;

                    //Save Order
                    storeDB.Orders.Add(order);
                    storeDB.SaveChanges();
                    //Process the order
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);

                    return RedirectToAction("Complete",
                        new { id = order.OrderId });

            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }

        }
        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = storeDB.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }


   }
}
