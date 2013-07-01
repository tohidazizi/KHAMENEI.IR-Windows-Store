﻿using System;

/*
 * LICENSE: http://opensource.org/licenses/ms-pl) 
 */

/*
 *
 *  A P I   M A S H
 *
 *  Update the classes here to invoke the RESTful API call(s)
 */

namespace APIMASH_Khamenei.BLL
{
    public class APIMASHEvent : EventArgs
    {
        public APIMASHStatus Status { get; set; }

        public string APIName { get; set; }

        public string Message { get; set; }

        public object Object { get; set; }
    }
}