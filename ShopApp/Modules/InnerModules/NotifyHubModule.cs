using Microsoft.AspNetCore.SignalR;
using ShopApp.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Modules.InnerModules
{
    public class NotifyHubModule
    {
        public IHubContext<NotifyHub> Hub { get; set; }
        public NotifyHubModule(IHubContext<NotifyHub> hubContext)
        {
            Hub = hubContext;
        }


/*        public async void NotifyOrderExecuted()
        {
            
            await Hub.Clients.All.SendAsync("receive", "data for you");
        }*/
    }
}
