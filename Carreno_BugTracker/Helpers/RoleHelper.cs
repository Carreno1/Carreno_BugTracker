using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carreno_BugTracker.Helpers
{
    public class RoleHelper
    {
        //When I want to use the methods inside this class
        //Do I want to instantiate a variable of type RoleHelper OR
        //Do I just want to call the method
        public string GetRole(string userId)
        {
            return "Admin";
        }

        public static string GetStaticRole(string userId)
        {
            return "Developer";
        }


    }

    public class Test
    {
        //Option 1: Instantiating a variable of type RoleHelper
        public void Foo()
        {
            var roleHelper = new RoleHelper();
            var myRole = roleHelper.GetRole("123");

            //Option 2: Just call the class and method
            var myStaticRole = RoleHelper.GetStaticRole("123");
            
            

        }


    }

}