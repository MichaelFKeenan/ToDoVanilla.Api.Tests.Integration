using System;

namespace ToDoVanilla.Api.Tests.Integration.Categories
{
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(Object obj) {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            var category = (Category)obj;

            return (Id == category.Id)
                && (Name == category.Name);
        }
    }
}
