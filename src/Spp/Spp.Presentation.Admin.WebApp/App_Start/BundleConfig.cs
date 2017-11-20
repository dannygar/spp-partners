/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
// Copyright (c) Microsoft Corporation. All rights reserved. 

using System.Web.Optimization;

namespace WebPortal
{
    public class BundleConfig
    {
        public static void RegisterBundle(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            var styleBundle = new StyleBundle("~/bundles/css");
            styleBundle.Include("~/Client/CSS/fonts.css", new CssRewriteUrlTransform());
            styleBundle.Include("~/Client/CSS/bootstrap.min.css", new CssRewriteUrlTransform());
            styleBundle.Include("~/Client/CSS/jquery-ui.min.css", new CssRewriteUrlTransform());
            styleBundle.Include("~/Client/CSS/cropper.min.css", new CssRewriteUrlTransform());
            styleBundle.Include("~/Client/CSS/app.css", new CssRewriteUrlTransform());

            bundles.Add(styleBundle);

            bundles.Add(new ScriptBundle("~/bundles/js/lib").Include(
                "~/Client/Scripts/Lib/jquery-2.1.4.min.js",
                "~/Client/Scripts/Lib/jquery-ui.min.js",
                "~/Client/Scripts/Lib/globalize.js",
                "~/Client/Scripts/Lib/angular.min.js",
                "~/Client/Scripts/Lib/angular-ui-router.min.js",
                "~/Client/Scripts/Lib/bootstrap.min.js",
                "~/Client/Scripts/Lib/moment.js",
                "~/Client/Scripts/Lib/moment-timezone-with-data.js",
                "~/Client/Scripts/Lib/cropper.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/app").Include(
                "~/Client/Scripts/app.js",
                "~/Client/Scripts/router.js",
                /* Controllers */
                "~/Client/Scripts/Controllers/main.ctrl.js",
                "~/Client/Scripts/Controllers/sessions-day.ctrl.js",
                "~/Client/Scripts/Controllers/sessions-week.ctrl.js",
                "~/Client/Scripts/Controllers/session-detail.ctrl.js",
                "~/Client/Scripts/Controllers/users.ctrl.js",
                "~/Client/Scripts/Controllers/user-detail.ctrl.js",
                "~/Client/Scripts/Controllers/user-new.ctrl.js",
                "~/Client/Scripts/Controllers/login.ctrl.js",
                "~/Client/Scripts/Controllers/settings.ctrl.js",
                /* Services */
                "~/Client/Scripts/Services/main.svc.js",
                "~/Client/Scripts/Services/resource.svc.js",
                "~/Client/Scripts/Services/utils.svc.js",
                "~/Client/Scripts/Services/auth.svc.js",
                "~/Client/Scripts/Services/debounce.svc.js",
                "~/Client/Scripts/Services/validation.svc.js",
                "~/Client/Scripts/Services/settings.svc.js",
                /* Directives */
                "~/Client/Scripts/Directives/main.drv.js",
                "~/Client/Scripts/Directives/nav-tabs.drv.js",
                "~/Client/Scripts/Directives/loading-spinner.drv.js",
                "~/Client/Scripts/Directives/date-picker.drv.js",
                "~/Client/Scripts/Directives/time-picker.drv.js",
                "~/Client/Scripts/Directives/on-off-slider.drv.js",
                "~/Client/Scripts/Directives/timezone-picker.drv.js",
                "~/Client/Scripts/Directives/image-cropper.drv.js",
                "~/Client/Scripts/Directives/modal-view.drv.js",
                "~/Client/Scripts/Directives/switcher.drv.js",
                "~/Client/Scripts/Directives/dynamic-content.drv.js",
                "~/Client/Scripts/Directives/content-for.drv.js",
                "~/Client/Scripts/Directives/dynamic-height.drv.js",
                "~/Client/Scripts/Directives/context-menu.drv.js",
                "~/Client/Scripts/Directives/popover.drv.js",
                "~/Client/Scripts/Directives/player-assignment.drv.js",
                "~/Client/Scripts/Directives/composite-switcher.drv.js",
                "~/Client/Scripts/Directives/wellness-popover.drv.js",
                "~/Client/Scripts/Directives/carousel.drv.js",
                "~/Client/Scripts/Directives/unchanged-option.drv.js",
                "~/Client/Scripts/Directives/bind-scroll-to.drv.js",
                "~/Client/Scripts/Directives/alert-view.drv.js",
                "~/Client/Scripts/Directives/redraw.drv.js",
                "~/Client/Scripts/Directives/personal-image-gallery.drv.js",
                "~/Client/Scripts/Directives/pager.drv.js",
                /* Filters */
                "~/Client/Scripts/Filters/main.flt.js",
                "~/Client/Scripts/Filters/related-by-id.flt.js",
                "~/Client/Scripts/Filters/show-users.flt.js",
                "~/Client/Scripts/Filters/personal-images.flt.js",
                /* Models */
                "~/Client/Scripts/Models/time.mdl.js",
                "~/Client/Scripts/Models/date.mdl.js"
            ));

            BundleTable.EnableOptimizations = false; 
        }
    }
}
