using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrMan.Utility
{

    public class Helper
    {

        public const string Admin = "Admin";
        public static string Regular = "Regular";
        public static string SuperVisor = "SuperVisor";
        public static string Manager = "Manager";
        public static string appointmentAdded = "Appointment added successfully.";
        public static string appointmentUpdated = "Appointment updated successfully.";
        public static string appointmentDeleted = "Appointment deleted successfully.";
        public static string appointmentExists = "Appointment for selected date and time already exists.";
        public static string appointmentNotExists = "Appointment not exists.";
        public static string meetingConfirm = "Meeting confirm successfully.";
        public static string meetingConfirmError = "Error while confirming meeting.";
        public static string appointmentAddError = "Something went wront, Please try again.";
        public static string appointmentUpdatError = "Something went wront, Please try again.";
        public static string somethingWentWrong = "Something went wront, Please try again.";
        public static int success_code = 1;
        public static int failure_code = 0;

        public static List<SelectListItem> GetRolesForDropDown()
        {
           /* if (isAdmin)
            {
                return new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = Helper.Admin,
                        Value = Helper.Admin
                    }
                };
            }
            else
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{Value=Helper.SuperVisor,Text=Helper.SuperVisor},
                    new SelectListItem{Value=Helper.Regular,Text=Helper.Regular},
                    new SelectListItem{Value=Helper.Manager,Text=Helper.Manager},
                };
            }*/
            return new List<SelectListItem>
                {
                    new SelectListItem{Value=Helper.SuperVisor,Text=Helper.SuperVisor},
                    new SelectListItem{Value=Helper.Regular,Text=Helper.Regular},
                    new SelectListItem{Value=Helper.Manager,Text=Helper.Manager},
                    new SelectListItem{Value=Helper.Admin,Text=Helper.Admin},
                };
        }



    }

}
 
