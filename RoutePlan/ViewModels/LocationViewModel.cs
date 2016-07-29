using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace RoutePlan.ViewModels
{
    public class LocationViewModel
    {
        [DisplayName("ID")]
        public string ID { set; get; }

        [DisplayName("經度")]
        public double longitude { set; get; }
        

        [DisplayName("緯度")]
        public double latitude { set; get; }
    }
}