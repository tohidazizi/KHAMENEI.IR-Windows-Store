using System;

/*
 * LICENSE: http://opensource.org/licenses/ms-pl) 
 */
namespace APIMASH_Khamenei.BLL
{
    public delegate void APIMASHEventHandler(object sender, APIMASHEvent e);

    public class APIMASHInvoke
    {
        public event APIMASHEventHandler OnResponse;

        /// <summary>
        /// Reads RSS feed
        /// </summary>
        /// <param name="apiCall"></param>
        public void Invoke(string apiCall)
        {
            var apimashEvent = new APIMASHEvent();

            try
            {
                var response = APIMASHMap.LoadRssFeed(apiCall);

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
