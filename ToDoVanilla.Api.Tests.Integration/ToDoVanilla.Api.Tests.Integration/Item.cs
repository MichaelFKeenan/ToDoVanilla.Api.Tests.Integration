using System;

namespace ToDoVanilla.Api.Tests.Integration
{
    public partial class Tests
    {
        public class Item
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public bool Complete { get; set; }
            public int Priority { get; set; }
            public string Description { get; set; }
            public int Effort { get; set; }
            public string CompleteBy { get; set; }
            public string CategoryId { get; set; }
            public string CategoryName { get; set; }

            public override bool Equals(Object obj) {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }

                var item = (Item)obj;

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
}