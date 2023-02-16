using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Component.Object
{
    public class DetailsViewModel:ProjectViewModel
    {
        public int Price { get; set; }
        public int TransactionType { get; set; }
        public int EstateType { get; set; }
        public int CategoryID { get; set; }
        public string? ProjectName { get; set; }
        public string? Investor { get; set; }
        public int? Elevator { get; set; }
        public int? Floor { get; set; }
        public int? TotalFloor { get; set; }
        public DateTime? TransferDate { get; set; }
        public string Description { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public List<string> Images { get; set; }
        public DetailsViewModel(Project project)
        {
            ID = project.ID;
            Title = project.Title;
            Address = project.Address;
            TotalPrice = project.TotalPrice;
            Bathroom = project.Bathroom;
            MainDirect = project.MainDirection;
            Balcony = project.BalconyDirection;
            Square = project.Square;
            WardID = project.WardID;
            Price = project.Price;
            TransactionType = project.TransactionType;
            CategoryID = project.CategoryID;
            ContactEmail = project.ContactEmail;
            ContactName = project.ContactName;
            ContactPhone = project.ContactPhone;
        }
    }
}
