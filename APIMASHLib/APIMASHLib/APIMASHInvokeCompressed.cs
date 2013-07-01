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
    public class APIMASHInvokeCompressed
    {
        public event APIMASHEventHandler OnResponse;

        async public void Invoke<T>(string apiCall)
        {
            var apimashEvent = new APIMASHEvent();

            try
            {
                T response = await APIMASHMap.LoadCompressedObject<T>(apiCall);
                apimashEvent.Object = response;
                apimashEvent.Status = APIMASHStatus.SUCCESS;
                apimashEvent.Message = string.Empty;
            }
            catch (Exception e)
            {
                apimashEvent.Message = e.Message;
                apimashEvent.Object = null;
                apimashEvent.Status = APIMASHStatus.FAILURE;
            }

            OnResponse(this, apimashEvent);
        }
    }
}