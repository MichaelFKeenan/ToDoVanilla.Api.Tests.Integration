using System;

namespace ToDoVanilla.Api.Tests.Integration.Items
{
    public class DisplayItem : Item
    {
        public string CategoryName { get; set; }
    
        public override bool Equals(Object obj) {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
    
            var item = (DisplayItem)obj;
    
            return (Id == item.Id) 
                && (Name == item.Name)
                && (Complete == item.Complete)
                && (Priority == item.Priority)
                && (Description == item.Description)
                && (Effort == item.Effort)
                && (CompleteBy == item.CompleteBy)
                && (CategoryId == item.CategoryId)
                && (CategoryName == item.CategoryName);
        }
    }
}
