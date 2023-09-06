using System;
using Microsoft.AspNetCore.Mvc;

namespace WEB_153501_Antilevskaya.ViewComponents
{
    public class Cart:ViewComponent
    {
        public string Invoke()
        {
            return "00,0 руб (0)";
        }
    }
}
