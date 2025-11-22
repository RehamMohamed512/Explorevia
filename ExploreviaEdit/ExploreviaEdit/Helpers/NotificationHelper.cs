using Microsoft.AspNetCore.Mvc;

namespace ExploreviaEdit.Helpers
{
    public static class NotificationHelper
    {
        public static void Success(Controller controller, string message)
        {
            controller.TempData["NotificationType"] = "success";
            controller.TempData["NotificationMessage"] = message;
        }

        public static void Error(Controller controller, string message)
        {
            controller.TempData["NotificationType"] = "danger";
            controller.TempData["NotificationMessage"] = message;
        }
    }
}


