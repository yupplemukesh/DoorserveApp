using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo
{
    public static class CommonModel
    {
        public static List<CheckBox> ServiceType()
        {

            List<CheckBox> checkBoxes = new List<CheckBox>();
            CheckBox chk = new CheckBox { Text = "Installation", Value = "Installation", IsChecked = false };
            checkBoxes.Add(chk);
            chk = new CheckBox { Text = "Repair", Value = "Repair", IsChecked = false };
            checkBoxes.Add(chk);

            return checkBoxes;
        }



        public static List<CheckBox> DeliveryServiceType()
        {

            List<CheckBox> checkBoxes = new List<CheckBox>();
            CheckBox chk = new CheckBox { Text = "Pic & Drop", Value = "Pic & Drop", IsChecked = false };
            checkBoxes.Add(chk);
            chk = new CheckBox { Text = "Carry-in", Value = "Carry-in", IsChecked = false };
            checkBoxes.Add(chk);
            chk = new CheckBox { Text = "Onsite", Value = "Onsite", IsChecked = false };
            checkBoxes.Add(chk);
            return checkBoxes;



        }

    }


    public class CheckBox
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool IsChecked { get; set; }

    }
}