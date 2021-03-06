﻿using Microsoft.AspNetCore.Builder;

namespace NewsPaper.GateWay.Configure
{
    public class ConfigureEndpoints
    {
        public static void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseCors("DefaultPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
