namespace EmployeeApp.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string IDNumber { get; set; }
        public bool IsArchived { get; set; } = false;
    }
}