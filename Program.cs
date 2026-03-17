using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
class Employee
{
    public int Id;
    public string Name;
    public string Department;
    public double BaseSalary;
    public virtual double CalculateSalary()
    {
        return BaseSalary;
    }
    public virtual string GetTypeName()
    {
        return "Employee";
    }
}
class FullTimeEmployee : Employee
{
    public double Bonus;

    public override double CalculateSalary()
    {
        return BaseSalary + Bonus;
    }
    public override string GetTypeName()
    {
        return "Full-Time";
    }
}
class ContractEmployee : Employee
{
    public int HoursWorked;
    public double HourlyRate;

    public override double CalculateSalary()
    {
        return HoursWorked * HourlyRate;
    }
    public override string GetTypeName()
    {
        return "Contract";
    }
}
class Program
{
    static List<Employee> employees = new List<Employee>();
    static void Main()
    {
        var actions = new Dictionary<int, Action>
    {
        {1, AddFullTimeEmployee},
        {2, AddContractEmployee},
        {3, ViewAllEmployees},
        {4, SearchByDepartment},
        {5, SortBySalary},
        {6, DeleteEmployee},
        {7, BonusFeatures}
        {9, FetchFromDatabase}
    };

        while (true)
        {
            Console.WriteLine("\n===== Employee Management System =====");
            Console.WriteLine("1. Add Full-Time Employee");
            Console.WriteLine("2. Add Contract Employee");
            Console.WriteLine("3. View All Employees");
            Console.WriteLine("4. Search by Department");
            Console.WriteLine("5. Sort by Salary (High → Low)");
            Console.WriteLine("6. Delete Employee");
            Console.WriteLine("7. Bonus Features");
            Console.WriteLine("8. Exit");
            Console.WriteLine("9. Fetch Employees from Database");

            Console.Write("Enter choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input!");
                continue;
            }

            if (choice == 10)
            break;

            if (actions.ContainsKey(choice))
                actions[choice]();
            else
                Console.WriteLine("Invalid choice!");
        }
    }

    static void AddFullTimeEmployee()
    {
        FullTimeEmployee emp = new FullTimeEmployee();

        emp.Id = GetInt("Enter Id: ");
        Console.Write("Enter Name: ");
        emp.Name = Console.ReadLine();

        Console.Write("Enter Department: ");
        emp.Department = Console.ReadLine();

        emp.BaseSalary = GetDouble("Enter Base Salary: ");
        emp.Bonus = GetDouble("Enter Bonus: ");

        employees.Add(emp);
        Console.WriteLine("Full-Time Employee Added!");
    }
    static void AddContractEmployee()
    {
        ContractEmployee emp = new ContractEmployee();

        emp.Id = GetInt("Enter Id: ");
        Console.Write("Enter Name: ");
        emp.Name = Console.ReadLine();

        Console.Write("Enter Department: ");
        emp.Department = Console.ReadLine();

        emp.HoursWorked = GetInt("Enter Hours Worked: ");
        emp.HourlyRate = GetDouble("Enter Hourly Rate: ");

        employees.Add(emp);
        Console.WriteLine("Contract Employee Added!");
    }
   
    static void ViewAllEmployees()
    {
        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found!");
            return;
        }

        foreach (var emp in employees)
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"ID: {emp.Id}");
            Console.WriteLine($"Name: {emp.Name}");
            Console.WriteLine($"Department: {emp.Department}");
            Console.WriteLine($"Type: {emp.GetTypeName()}");
            Console.WriteLine($"Salary: {emp.CalculateSalary()}");
        }
    }
    static void SearchByDepartment()
    {
        Console.Write("Enter Department: ");
        string dept = Console.ReadLine();

        var result = employees.Where(e => e.Department.Equals(dept, StringComparison.OrdinalIgnoreCase)).ToList();

        if (result.Count == 0)
        {
            Console.WriteLine("No employees found in this department!");
            return;
        }

        foreach (var emp in result)
        {
            Console.WriteLine($"{emp.Name} - {emp.CalculateSalary()}");
        }
    }
    static void SortBySalary()
    {
        if (employees.Count == 0)
        {
            Console.WriteLine("No employees to sort!");
            return;
        }

        var sorted = employees.OrderByDescending(e => e.CalculateSalary());

        foreach (var emp in sorted)
        {
            Console.WriteLine($"{emp.Name} - {emp.CalculateSalary()}");
        }
    }
    static void DeleteEmployee()
    {
        int id = GetInt("Enter Employee ID to delete: ");

        int removed = employees.RemoveAll(e => e.Id == id);

        if (removed == 0)
            Console.WriteLine("Employee not found!");
        else
            Console.WriteLine("Employee deleted successfully!");
    }
    static void BonusFeatures()
    {
        if (!employees.Any())
        {
            Console.WriteLine("No data available!");
            return;
        }

        Console.WriteLine("\n--- Bonus Features ---");

        var highestSalary = employees.Max(e => e.CalculateSalary());
        var highest = employees.First(e => e.CalculateSalary() == highestSalary);

        Console.WriteLine($"Highest Paid: {highest.Name} - {highestSalary}");


        double avg = employees.Average(e => e.CalculateSalary());
        Console.WriteLine($"Average Salary: {avg}");


        var groups = employees.GroupBy(e => e.Department);

        foreach (var group in groups)
        {
            Console.WriteLine($"\nDepartment: {group.Key}");
            foreach (var emp in group)
            {
                Console.WriteLine($"{emp.Name} - {emp.CalculateSalary()}");
            }
        }
    }
    static int GetInt(string message)
    {
        while (true)
        {
            Console.Write(message);
            if (int.TryParse(Console.ReadLine(), out int value))
                return value;

            Console.WriteLine("Invalid input!");
        }
    }

    static double GetDouble(string message)
    {
        while (true)
        {
            Console.Write(message);
            if (double.TryParse(Console.ReadLine(), out double value))
                return value;

            Console.WriteLine("Invalid input!");
        }
    }
    static void FetchFromDatabase()
    {
        string connectionString = "Data Source=.;Initial Catalog=Employee;Integrated Security=True";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM EmployeesDB";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\n--- Data from Database ---");

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["Id"]}, Name: {reader["Name"]}, Dept: {reader["Department"]}, Salary: {reader["Salary"]}");
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}



