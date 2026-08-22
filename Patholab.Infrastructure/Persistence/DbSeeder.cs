using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Patholab.Application.Common;
using Patholab.Domain.Entities;
using Patholab.Domain.Enums;

namespace Patholab.Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(PatholabDbContext context)
        {
            context.Database.EnsureCreated();

            // 1. Seed Roles
            if (!await context.Roles.AnyAsync())
            {
                var roles = new List<Role>
                {
                    new Role { RoleName = "Super Admin", Description = "Full system control", IsActive = true, CreatedBy = "System" },
                    new Role { RoleName = "Lab Admin", Description = "Laboratory settings and test configurations", IsActive = true, CreatedBy = "System" },
                    new Role { RoleName = "Receptionist", Description = "Patient registration and billing", IsActive = true, CreatedBy = "System" },
                    new Role { RoleName = "Lab Technician", Description = "Specimen reception and result entry", IsActive = true, CreatedBy = "System" },
                    new Role { RoleName = "Pathologist", Description = "Result validation and report signing", IsActive = true, CreatedBy = "System" },
                    new Role { RoleName = "Accountant", Description = "Financial auditing and payment approvals", IsActive = true, CreatedBy = "System" },
                    new Role { RoleName = "Collection Agent", Description = "Home visit specimen collections", IsActive = true, CreatedBy = "System" }
                };
                context.Roles.AddRange(roles);
                await context.SaveChangesAsync();
            }

            // Get Roles for reference
            var superAdminRole = await context.Roles.FirstAsync(r => r.RoleName == "Super Admin");
            var adminRole = await context.Roles.FirstAsync(r => r.RoleName == "Lab Admin");
            var receptionRole = await context.Roles.FirstAsync(r => r.RoleName == "Receptionist");
            var techRole = await context.Roles.FirstAsync(r => r.RoleName == "Lab Technician");
            var pathRole = await context.Roles.FirstAsync(r => r.RoleName == "Pathologist");
            var accRole = await context.Roles.FirstAsync(r => r.RoleName == "Accountant");
            var agentRole = await context.Roles.FirstAsync(r => r.RoleName == "Collection Agent");

            // 2. Seed Users
            if (!await context.Users.AnyAsync())
            {
                var users = new List<User>
                {
                    new User
                    {
                        Username = "admin",
                        FullName = "System Administrator",
                        Email = "admin@patholab.com",
                        Mobile = "9876543210",
                        PasswordHash = PasswordHasher.HashPassword("Admin@123"),
                        RoleId = superAdminRole.Id,
                        IsActive = true,
                        CreatedBy = "System"
                    },
                    new User
                    {
                        Username = "reception",
                        FullName = "Rita Sharma",
                        Email = "reception@patholab.com",
                        Mobile = "9876543211",
                        PasswordHash = PasswordHasher.HashPassword("Reception@123"),
                        RoleId = receptionRole.Id,
                        IsActive = true,
                        CreatedBy = "System"
                    },
                    new User
                    {
                        Username = "technician",
                        FullName = "Tushar Roy",
                        Email = "technician@patholab.com",
                        Mobile = "9876543212",
                        PasswordHash = PasswordHasher.HashPassword("Technician@123"),
                        RoleId = techRole.Id,
                        IsActive = true,
                        CreatedBy = "System"
                    },
                    new User
                    {
                        Username = "pathologist",
                        FullName = "Dr. Priya Nair",
                        Email = "pathologist@patholab.com",
                        Mobile = "9876543213",
                        PasswordHash = PasswordHasher.HashPassword("Pathologist@123"),
                        RoleId = pathRole.Id,
                        IsActive = true,
                        CreatedBy = "System"
                    },
                    new User
                    {
                        Username = "accountant",
                        FullName = "Anil Mehta",
                        Email = "accountant@patholab.com",
                        Mobile = "9876543214",
                        PasswordHash = PasswordHasher.HashPassword("Accountant@123"),
                        RoleId = accRole.Id,
                        IsActive = true,
                        CreatedBy = "System"
                    },
                    new User
                    {
                        Username = "agent",
                        FullName = "Amit Verma",
                        Email = "agent@patholab.com",
                        Mobile = "9876543215",
                        PasswordHash = PasswordHasher.HashPassword("Agent@123"),
                        RoleId = agentRole.Id,
                        IsActive = true,
                        CreatedBy = "System"
                    }
                };
                context.Users.AddRange(users);
                await context.SaveChangesAsync();
            }

            // 3. Seed Permissions & RolePermissions
            if (!await context.Permissions.AnyAsync())
            {
                var permissions = new List<Permission>
                {
                    new Permission { PermissionCode = "PATIENTS_VIEW", PermissionName = "View Patients", ModuleName = "Patient Management" },
                    new Permission { PermissionCode = "PATIENTS_CREATE", PermissionName = "Create Patient", ModuleName = "Patient Management" },
                    new Permission { PermissionCode = "PATIENTS_EDIT", PermissionName = "Edit Patient", ModuleName = "Patient Management" },
                    new Permission { PermissionCode = "PATIENTS_DELETE", PermissionName = "Delete Patient", ModuleName = "Patient Management" },
                    new Permission { PermissionCode = "ORDERS_VIEW", PermissionName = "View Orders", ModuleName = "Order Management" },
                    new Permission { PermissionCode = "ORDERS_CREATE", PermissionName = "Create Order", ModuleName = "Order Management" },
                    new Permission { PermissionCode = "SAMPLES_COLLECT", PermissionName = "Collect Sample", ModuleName = "Laboratory" },
                    new Permission { PermissionCode = "SAMPLES_RECEIVE", PermissionName = "Receive Sample", ModuleName = "Laboratory" },
                    new Permission { PermissionCode = "RESULTS_ENTER", PermissionName = "Enter Test Results", ModuleName = "Laboratory" },
                    new Permission { PermissionCode = "REPORTS_VERIFY", PermissionName = "Verify Reports", ModuleName = "Pathology Sign-off" },
                    new Permission { PermissionCode = "REPORTS_PUBLISH", PermissionName = "Publish Reports", ModuleName = "Pathology Sign-off" },
                    new Permission { PermissionCode = "BILLING_VIEW", PermissionName = "View Invoices", ModuleName = "Billing" },
                    new Permission { PermissionCode = "BILLING_PAY", PermissionName = "Collect Payment", ModuleName = "Billing" },
                    new Permission { PermissionCode = "ADMIN_USERS", PermissionName = "Manage Users", ModuleName = "Administration" }
                };
                context.Permissions.AddRange(permissions);
                await context.SaveChangesAsync();

                // Grant all to Super Admin & Lab Admin
                var allPermissions = await context.Permissions.ToListAsync();
                foreach (var p in allPermissions)
                {
                    context.RolePermissions.Add(new RolePermission { RoleId = superAdminRole.Id, PermissionId = p.Id });
                    context.RolePermissions.Add(new RolePermission { RoleId = adminRole.Id, PermissionId = p.Id });
                }

                // Grant Receptionist permissions
                var recepCodes = new[] { "PATIENTS_VIEW", "PATIENTS_CREATE", "PATIENTS_EDIT", "ORDERS_VIEW", "ORDERS_CREATE", "BILLING_VIEW", "BILLING_PAY" };
                foreach (var p in allPermissions.Where(x => recepCodes.Contains(x.PermissionCode)))
                {
                    context.RolePermissions.Add(new RolePermission { RoleId = receptionRole.Id, PermissionId = p.Id });
                }

                // Grant Technician permissions
                var techCodes = new[] { "PATIENTS_VIEW", "ORDERS_VIEW", "SAMPLES_COLLECT", "SAMPLES_RECEIVE", "RESULTS_ENTER" };
                foreach (var p in allPermissions.Where(x => techCodes.Contains(x.PermissionCode)))
                {
                    context.RolePermissions.Add(new RolePermission { RoleId = techRole.Id, PermissionId = p.Id });
                }

                // Grant Pathologist permissions
                var pathCodes = new[] { "PATIENTS_VIEW", "ORDERS_VIEW", "RESULTS_ENTER", "REPORTS_VERIFY", "REPORTS_PUBLISH" };
                foreach (var p in allPermissions.Where(x => pathCodes.Contains(x.PermissionCode)))
                {
                    context.RolePermissions.Add(new RolePermission { RoleId = pathRole.Id, PermissionId = p.Id });
                }

                await context.SaveChangesAsync();
            }

            // 4. Seed Departments
            if (!await context.Departments.AnyAsync())
            {
                var depts = new List<Department>
                {
                    new Department { DepartmentCode = "HEM", DepartmentName = "Hematology", Description = "Blood assays", IsActive = true, CreatedBy = "System" },
                    new Department { DepartmentCode = "BIO", DepartmentName = "Biochemistry", Description = "Chemical analysis", IsActive = true, CreatedBy = "System" },
                    new Department { DepartmentCode = "CLP", DepartmentName = "Clinical Pathology", Description = "Fluids & Urine assays", IsActive = true, CreatedBy = "System" },
                    new Department { DepartmentCode = "MIC", DepartmentName = "Microbiology", Description = "Culture assays", IsActive = true, CreatedBy = "System" },
                    new Department { DepartmentCode = "SER", DepartmentName = "Serology", Description = "Antibody tests", IsActive = true, CreatedBy = "System" },
                    new Department { DepartmentCode = "IMM", DepartmentName = "Immunology", Description = "Immune response", IsActive = true, CreatedBy = "System" },
                    new Department { DepartmentCode = "HIS", DepartmentName = "Histopathology", Description = "Tissue biopsies", IsActive = true, CreatedBy = "System" }
                };
                context.Departments.AddRange(depts);
                await context.SaveChangesAsync();
            }

            var hemDept = await context.Departments.FirstAsync(d => d.DepartmentCode == "HEM");
            var bioDept = await context.Departments.FirstAsync(d => d.DepartmentCode == "BIO");
            var clpDept = await context.Departments.FirstAsync(d => d.DepartmentCode == "CLP");

            // 5. Seed SampleTypes
            if (!await context.SampleTypes.AnyAsync())
            {
                var sampleTypes = new List<SampleType>
                {
                    new SampleType { SampleTypeCode = "BLD", SampleTypeName = "Whole Blood", Description = "EDTA / Citrate Vials", IsActive = true, CreatedBy = "System" },
                    new SampleType { SampleTypeCode = "URN", SampleTypeName = "Urine Specimen", Description = "Sterile Container", IsActive = true, CreatedBy = "System" },
                    new SampleType { SampleTypeCode = "SER", SampleTypeName = "Serum", Description = "Red-top Clot Activator Vials", IsActive = true, CreatedBy = "System" },
                    new SampleType { SampleTypeCode = "PLA", SampleTypeName = "Plasma", Description = "Fluoride Vials", IsActive = true, CreatedBy = "System" },
                    new SampleType { SampleTypeCode = "STL", SampleTypeName = "Stool Sample", Description = "Wide container", IsActive = true, CreatedBy = "System" }
                };
                context.SampleTypes.AddRange(sampleTypes);
                await context.SaveChangesAsync();
            }

            var bloodSample = await context.SampleTypes.FirstAsync(s => s.SampleTypeCode == "BLD");
            var urineSample = await context.SampleTypes.FirstAsync(s => s.SampleTypeCode == "URN");
            var serumSample = await context.SampleTypes.FirstAsync(s => s.SampleTypeCode == "SER");

            // 6. Seed Tests & TestParameters
            if (!await context.Tests.AnyAsync())
            {
                // CBC
                var cbc = new Test
                {
                    TestCode = "CBC",
                    TestName = "Complete Blood Count",
                    DepartmentId = hemDept.Id,
                    SampleTypeId = bloodSample.Id,
                    TestType = "Profile",
                    Price = 350.00m,
                    TATMinutes = 120,
                    IsActive = true,
                    CreatedBy = "System"
                };
                cbc.TestParameters.Add(new TestParameter { ParameterCode = "HB", ParameterName = "Hemoglobin", DisplayOrder = 1, ResultType = "Numeric", Unit = "g/dL", MaleMin = 13.0m, MaleMax = 17.0m, FemaleMin = 12.0m, FemaleMax = 15.0m, ChildMin = 11.0m, ChildMax = 14.0m, DefaultReferenceRange = "12 - 17", CriticalLow = 7.0m, CriticalHigh = 20.0m });
                cbc.TestParameters.Add(new TestParameter { ParameterCode = "RBC", ParameterName = "RBC Count", DisplayOrder = 2, ResultType = "Numeric", Unit = "mil/µL", MaleMin = 4.5m, MaleMax = 5.9m, FemaleMin = 4.1m, FemaleMax = 5.1m, ChildMin = 3.8m, ChildMax = 5.2m, DefaultReferenceRange = "4.0 - 6.0" });
                cbc.TestParameters.Add(new TestParameter { ParameterCode = "WBC", ParameterName = "WBC Count", DisplayOrder = 3, ResultType = "Numeric", Unit = "/µL", MaleMin = 4000m, MaleMax = 11000m, FemaleMin = 4000m, FemaleMax = 11000m, ChildMin = 5000m, ChildMax = 13000m, DefaultReferenceRange = "4000 - 11000", CriticalLow = 2000m, CriticalHigh = 30000m });
                cbc.TestParameters.Add(new TestParameter { ParameterCode = "PLT", ParameterName = "Platelet Count", DisplayOrder = 4, ResultType = "Numeric", Unit = "lakh/µL", MaleMin = 1.5m, MaleMax = 4.5m, FemaleMin = 1.5m, FemaleMax = 4.5m, ChildMin = 1.5m, ChildMax = 4.5m, DefaultReferenceRange = "1.5 - 4.5", CriticalLow = 0.5m, CriticalHigh = 10.0m });

                // LFT
                var lft = new Test
                {
                    TestCode = "LFT",
                    TestName = "Liver Function Test",
                    DepartmentId = bioDept.Id,
                    SampleTypeId = serumSample.Id,
                    TestType = "Profile",
                    Price = 750.00m,
                    TATMinutes = 180,
                    IsActive = true,
                    CreatedBy = "System"
                };
                lft.TestParameters.Add(new TestParameter { ParameterCode = "BIL_T", ParameterName = "Bilirubin Total", DisplayOrder = 1, ResultType = "Numeric", Unit = "mg/dL", MaleMin = 0.1m, MaleMax = 1.2m, FemaleMin = 0.1m, FemaleMax = 1.2m, ChildMin = 0.1m, ChildMax = 1.0m, DefaultReferenceRange = "0.1 - 1.2", CriticalHigh = 5.0m });
                lft.TestParameters.Add(new TestParameter { ParameterCode = "SGPT", ParameterName = "SGPT (ALT)", DisplayOrder = 2, ResultType = "Numeric", Unit = "U/L", MaleMin = 5m, MaleMax = 50m, FemaleMin = 5m, FemaleMax = 35m, ChildMin = 5m, ChildMax = 40m, DefaultReferenceRange = "5 - 50" });
                lft.TestParameters.Add(new TestParameter { ParameterCode = "SGOT", ParameterName = "SGOT (AST)", DisplayOrder = 3, ResultType = "Numeric", Unit = "U/L", MaleMin = 5m, MaleMax = 40m, FemaleMin = 5m, FemaleMax = 35m, ChildMin = 5m, ChildMax = 40m, DefaultReferenceRange = "5 - 40" });

                // KFT
                var kft = new Test
                {
                    TestCode = "KFT",
                    TestName = "Kidney Function Test",
                    DepartmentId = bioDept.Id,
                    SampleTypeId = serumSample.Id,
                    TestType = "Profile",
                    Price = 700.00m,
                    TATMinutes = 180,
                    IsActive = true,
                    CreatedBy = "System"
                };
                kft.TestParameters.Add(new TestParameter { ParameterCode = "UREA", ParameterName = "Blood Urea", DisplayOrder = 1, ResultType = "Numeric", Unit = "mg/dL", MaleMin = 15m, MaleMax = 45m, FemaleMin = 15m, FemaleMax = 40m, ChildMin = 10m, ChildMax = 36m, DefaultReferenceRange = "15 - 45", CriticalHigh = 100m });
                kft.TestParameters.Add(new TestParameter { ParameterCode = "CREAT", ParameterName = "Serum Creatinine", DisplayOrder = 2, ResultType = "Numeric", Unit = "mg/dL", MaleMin = 0.6m, MaleMax = 1.2m, FemaleMin = 0.5m, FemaleMax = 1.1m, ChildMin = 0.3m, ChildMax = 0.7m, DefaultReferenceRange = "0.5 - 1.2", CriticalHigh = 3.5m });

                // Sugar Fasting
                var sugar = new Test
                {
                    TestCode = "FBS",
                    TestName = "Blood Sugar Fasting",
                    DepartmentId = bioDept.Id,
                    SampleTypeId = bloodSample.Id,
                    TestType = "Single",
                    Price = 150.00m,
                    TATMinutes = 60,
                    IsActive = true,
                    CreatedBy = "System"
                };
                sugar.TestParameters.Add(new TestParameter { ParameterCode = "SUG_F", ParameterName = "Fasting Sugar", DisplayOrder = 1, ResultType = "Numeric", Unit = "mg/dL", MaleMin = 70m, MaleMax = 100m, FemaleMin = 70m, FemaleMax = 100m, ChildMin = 70m, ChildMax = 100m, DefaultReferenceRange = "70 - 100", CriticalLow = 50m, CriticalHigh = 350m });

                // Standing HbA1c
                var hba1c = new Test
                {
                    TestCode = "HBA1C",
                    TestName = "HbA1c (Glycated Hb)",
                    DepartmentId = bioDept.Id,
                    SampleTypeId = bloodSample.Id,
                    TestType = "Single",
                    Price = 400.00m,
                    TATMinutes = 120,
                    IsActive = true,
                    CreatedBy = "System"
                };
                hba1c.TestParameters.Add(new TestParameter { ParameterCode = "HBA_VAL", ParameterName = "HbA1c Value", DisplayOrder = 1, ResultType = "Numeric", Unit = "%", MaleMin = 4.0m, MaleMax = 5.6m, FemaleMin = 4.0m, FemaleMax = 5.6m, ChildMin = 4.0m, ChildMax = 5.6m, DefaultReferenceRange = "4.0 - 5.6%" });

                // Urine Routine
                var urine = new Test
                {
                    TestCode = "URN",
                    TestName = "Urine Routine",
                    DepartmentId = clpDept.Id,
                    SampleTypeId = urineSample.Id,
                    TestType = "Profile",
                    Price = 200.00m,
                    TATMinutes = 90,
                    IsActive = true,
                    CreatedBy = "System"
                };
                urine.TestParameters.Add(new TestParameter { ParameterCode = "URN_COL", ParameterName = "Color", DisplayOrder = 1, ResultType = "Text", DefaultReferenceRange = "Pale Yellow" });
                urine.TestParameters.Add(new TestParameter { ParameterCode = "URN_SUG", ParameterName = "Sugar", DisplayOrder = 2, ResultType = "Text", DefaultReferenceRange = "Nil" });
                urine.TestParameters.Add(new TestParameter { ParameterCode = "URN_PRO", ParameterName = "Protein", DisplayOrder = 3, ResultType = "Text", DefaultReferenceRange = "Nil" });

                context.Tests.AddRange(cbc, lft, kft, sugar, hba1c, urine);
                await context.SaveChangesAsync();
            }

            // 7. Seed TestPackages
            if (!await context.TestPackages.AnyAsync())
            {
                var cbc = await context.Tests.FirstAsync(t => t.TestCode == "CBC");
                var lft = await context.Tests.FirstAsync(t => t.TestCode == "LFT");
                var kft = await context.Tests.FirstAsync(t => t.TestCode == "KFT");
                var sugar = await context.Tests.FirstAsync(t => t.TestCode == "FBS");

                var package = new TestPackage
                {
                    PackageCode = "FB-CHK",
                    PackageName = "Full Body Wellness Checkup",
                    Description = "Includes CBC, LFT, KFT, and Blood Sugar Fasting.",
                    OriginalPrice = 1950.00m,
                    PackagePrice = 1499.00m,
                    DiscountAmount = 451.00m,
                    IsActive = true,
                    CreatedBy = "System"
                };

                package.PackageTests.Add(new PackageTest { TestId = cbc.Id });
                package.PackageTests.Add(new PackageTest { TestId = lft.Id });
                package.PackageTests.Add(new PackageTest { TestId = kft.Id });
                package.PackageTests.Add(new PackageTest { TestId = sugar.Id });

                context.TestPackages.Add(package);
                await context.SaveChangesAsync();
            }

            // 8. Seed Patients (20 Fictional Records)
            if (!await context.Patients.AnyAsync())
            {
                var firstNames = new[] { "Aarav", "Aditya", "Amit", "Ananya", "Arjun", "Diya", "Isha", "Kabir", "Neha", "Pranav", "Rohan", "Sanjana", "Shreya", "Siddharth", "Tanvi", "Varun", "Vijay", "Yash", "Karan", "Pooja" };
                var lastNames = new[] { "Sharma", "Verma", "Gupta", "Sen", "Roy", "Mehta", "Patel", "Iyer", "Nair", "Verma", "Chawla", "Joshi", "Bose", "Dutta", "Kadam", "Deshmukh", "Singh", "Pillai", "Reddy", "Rao" };
                var bloodGroups = new[] { "O+", "A+", "B+", "AB+", "O-", "A-", "B-", "AB-" };

                for (int i = 0; i < 20; i++)
                {
                    var age = 20 + (i * 3) % 60;
                    var gender = (i % 2 == 0) ? "Male" : "Female";
                    var pCode = $"PAT-{i + 1:D6}";

                    context.Patients.Add(new Patient
                    {
                        PatientCode = pCode,
                        FirstName = firstNames[i],
                        LastName = lastNames[i],
                        Gender = gender,
                        DateOfBirth = DateTime.UtcNow.AddYears(-age).AddDays(-i * 5),
                        Age = age,
                        BloodGroup = bloodGroups[i % bloodGroups.Length],
                        Mobile = $"98123456{i:D2}",
                        Email = $"{firstNames[i].ToLower()}.{lastNames[i].ToLower()}@gmail.com",
                        Address = $"Flat {101 + i}, Building A, Sector {i + 1}",
                        City = "Navi Mumbai",
                        State = "Maharashtra",
                        Pincode = "40070" + (i % 9 + 1),
                        IsActive = true,
                        CreatedBy = "System"
                    });
                }
                await context.SaveChangesAsync();
            }

            // 9. Seed Doctors (10 Fictional Records)
            if (!await context.Doctors.AnyAsync())
            {
                var spec = new[] { "General Medicine", "Cardiology", "Endocrinology", "Pediatrics", "Gynecology", "Oncology", "Orthopedics", "Dermatology", "Nephrology", "Neurology" };
                for (int i = 0; i < 10; i++)
                {
                    var dCode = $"DOC-{i + 1:D6}";
                    context.Doctors.Add(new Doctor
                    {
                        DoctorCode = dCode,
                        DoctorName = $"Dr. Rajesh {spec[i]}",
                        Qualification = "MBBS, MD",
                        Specialization = spec[i],
                        HospitalClinicName = $"Apex {spec[i]} Care Clinic",
                        Mobile = $"99887766{i:D2}",
                        Email = $"dr.rajesh{i}@apexclinic.com",
                        Address = $"Main Hospital Row, Suite {i + 1}",
                        CommissionType = CommissionType.Percentage,
                        CommissionValue = 10.00m,
                        IsActive = true,
                        CreatedBy = "System"
                    });
                }
                await context.SaveChangesAsync();
            }

            // 10. Seed Fictional Completed Transactions (Orders, Invoices, Payments, Samples, Results, Reports)
            if (!await context.Orders.AnyAsync())
            {
                var patients = await context.Patients.Take(15).ToListAsync();
                var doctors = await context.Doctors.Take(5).ToListAsync();
                
                var cbc = await context.Tests.Include(t => t.TestParameters).FirstAsync(t => t.TestCode == "CBC");
                var lft = await context.Tests.Include(t => t.TestParameters).FirstAsync(t => t.TestCode == "LFT");
                var sugar = await context.Tests.Include(t => t.TestParameters).FirstAsync(t => t.TestCode == "FBS");

                var seedUser = await context.Users.FirstAsync(u => u.Username == "technician");
                var pathUser = await context.Users.FirstAsync(u => u.Username == "pathologist");

                for (int i = 0; i < 15; i++)
                {
                    var p = patients[i];
                    var d = doctors[i % doctors.Count];
                    var orderNum = $"ORD-2026-{i + 1:D6}";

                    // Calculate pricing
                    decimal rate = cbc.Price + sugar.Price;
                    decimal discount = i % 3 == 0 ? 50.00m : 0.00m;
                    decimal net = rate - discount;
                    decimal paid = net; // fully paid
                    
                    var order = new Order
                    {
                        OrderNumber = orderNum,
                        PatientId = p.Id,
                        DoctorId = d.Id,
                        OrderDate = DateTime.UtcNow.AddDays(-i - 1),
                        OrderStatus = OrderStatus.Published,
                        TotalAmount = rate,
                        DiscountAmount = discount,
                        NetAmount = net,
                        PaidAmount = paid,
                        DueAmount = 0.00m,
                        PaymentStatus = PaymentStatus.Paid,
                        Remarks = "Demo seeded transaction",
                        CreatedOn = DateTime.UtcNow.AddDays(-i - 1),
                        CreatedBy = "System"
                    };

                    var detail1 = new OrderDetail
                    {
                        TestId = cbc.Id,
                        Rate = cbc.Price,
                        Amount = cbc.Price,
                        Quantity = 1,
                        SampleRequired = true,
                        Status = "Completed",
                        CreatedOn = DateTime.UtcNow.AddDays(-i - 1),
                        CreatedBy = "System"
                    };
                    var detail2 = new OrderDetail
                    {
                        TestId = sugar.Id,
                        Rate = sugar.Price,
                        Amount = sugar.Price,
                        Quantity = 1,
                        SampleRequired = true,
                        Status = "Completed",
                        CreatedOn = DateTime.UtcNow.AddDays(-i - 1),
                        CreatedBy = "System"
                    };

                    order.OrderDetails.Add(detail1);
                    order.OrderDetails.Add(detail2);
                    context.Orders.Add(order);
                    await context.SaveChangesAsync();

                    // Create Sample
                    var sampleNum = $"SMP-2026-{i + 1:D6}";
                    var barcode = $"BAR-2026-{i + 1:D6}";

                    var sample = new Sample
                    {
                        SampleNumber = sampleNum,
                        OrderId = order.Id,
                        PatientId = p.Id,
                        SampleTypeId = bloodSample.Id,
                        Barcode = barcode,
                        CollectionDateTime = DateTime.UtcNow.AddDays(-i - 1).AddHours(1),
                        CollectedById = seedUser.Id,
                        ReceivedDateTime = DateTime.UtcNow.AddDays(-i - 1).AddHours(2),
                        ReceivedById = seedUser.Id,
                        SampleStatus = SampleStatus.Completed,
                        CreatedOn = DateTime.UtcNow.AddDays(-i - 1),
                        CreatedBy = "System"
                    };
                    context.Samples.Add(sample);
                    await context.SaveChangesAsync();

                    // Create SampleTests
                    var st1 = new SampleTest
                    {
                        SampleId = sample.Id,
                        OrderDetailId = detail1.Id,
                        TestId = cbc.Id,
                        Status = "Completed",
                        AssignedToId = seedUser.Id,
                        StartedOn = DateTime.UtcNow.AddDays(-i - 1).AddHours(2),
                        CompletedOn = DateTime.UtcNow.AddDays(-i - 1).AddHours(3),
                        CreatedOn = DateTime.UtcNow.AddDays(-i - 1),
                        CreatedBy = "System"
                    };
                    var st2 = new SampleTest
                    {
                        SampleId = sample.Id,
                        OrderDetailId = detail2.Id,
                        TestId = sugar.Id,
                        Status = "Completed",
                        AssignedToId = seedUser.Id,
                        StartedOn = DateTime.UtcNow.AddDays(-i - 1).AddHours(2),
                        CompletedOn = DateTime.UtcNow.AddDays(-i - 1).AddHours(3),
                        CreatedOn = DateTime.UtcNow.AddDays(-i - 1),
                        CreatedBy = "System"
                    };
                    context.SampleTests.AddRange(st1, st2);
                    await context.SaveChangesAsync();

                    // Create TestResults
                    // Hemoglobin
                    var hbParam = cbc.TestParameters.First(x => x.ParameterCode == "HB");
                    context.TestResults.Add(new TestResult
                    {
                        SampleTestId = st1.Id,
                        TestParameterId = hbParam.Id,
                        ResultValue = "14.2",
                        ResultNumericValue = 14.2m,
                        ResultStatus = ResultStatus.Normal,
                        ReferenceRange = hbParam.DefaultReferenceRange,
                        Unit = hbParam.Unit,
                        EnteredById = seedUser.Id,
                        EnteredOn = DateTime.UtcNow.AddDays(-i - 1).AddHours(3),
                        VerifiedById = pathUser.Id,
                        VerifiedOn = DateTime.UtcNow.AddDays(-i - 1).AddHours(4),
                        CreatedOn = DateTime.UtcNow.AddDays(-i - 1),
                        CreatedBy = "System",
                        RowVersion = new byte[8]
                    });

                    // Fasting Sugar
                    var sugarParam = sugar.TestParameters.First(x => x.ParameterCode == "SUG_F");
                    var sugarVal = 92m + (i % 2 == 0 ? 5m : 32m); // some diabetic, some normal!
                    context.TestResults.Add(new TestResult
                    {
                        SampleTestId = st2.Id,
                        TestParameterId = sugarParam.Id,
                        ResultValue = sugarVal.ToString(),
                        ResultNumericValue = sugarVal,
                        ResultStatus = sugarVal > 100m ? ResultStatus.High : ResultStatus.Normal,
                        ReferenceRange = sugarParam.DefaultReferenceRange,
                        Unit = sugarParam.Unit,
                        EnteredById = seedUser.Id,
                        EnteredOn = DateTime.UtcNow.AddDays(-i - 1).AddHours(3),
                        VerifiedById = pathUser.Id,
                        VerifiedOn = DateTime.UtcNow.AddDays(-i - 1).AddHours(4),
                        CreatedOn = DateTime.UtcNow.AddDays(-i - 1),
                        CreatedBy = "System",
                        RowVersion = new byte[8]
                    });
                    await context.SaveChangesAsync();

                    // Create Report
                    var reportNum = $"REP-2026-{i + 1:D6}";
                    var report = new Report
                    {
                        ReportNumber = reportNum,
                        OrderId = order.Id,
                        PatientId = p.Id,
                        ReportStatus = ReportStatus.Published,
                        ReportDate = DateTime.UtcNow.AddDays(-i - 1),
                        VerifiedById = pathUser.Id,
                        VerifiedOn = DateTime.UtcNow.AddDays(-i - 1).AddHours(4),
                        PublishedOn = DateTime.UtcNow.AddDays(-i - 1).AddHours(4),
                        PdfFilePath = $"/uploads/Report_{reportNum}_V1.pdf",
                        VersionNumber = 1,
                        CreatedOn = DateTime.UtcNow.AddDays(-i - 1),
                        CreatedBy = "System"
                    };

                    report.ReportDetails.Add(new ReportDetail
                    {
                        TestId = cbc.Id,
                        TestName = cbc.TestName,
                        Interpretation = "Complete Blood Count parameters are within normal limits.",
                        CreatedOn = DateTime.UtcNow.AddDays(-i - 1),
                        CreatedBy = "System"
                    });
                    report.ReportDetails.Add(new ReportDetail
                    {
                        TestId = sugar.Id,
                        TestName = sugar.TestName,
                        Interpretation = sugarVal > 100m ? "Elevated fasting blood glucose levels. Clinical correlation recommended." : "Fasting blood sugar is normal.",
                        CreatedOn = DateTime.UtcNow.AddDays(-i - 1),
                        CreatedBy = "System"
                    });

                    context.Reports.Add(report);

                    // Create Invoice
                    var invoiceNum = $"INV-2026-{i + 1:D6}";
                    var invoice = new Invoice
                    {
                        InvoiceNumber = invoiceNum,
                        OrderId = order.Id,
                        PatientId = p.Id,
                        InvoiceDate = DateTime.UtcNow.AddDays(-i - 1),
                        GrossAmount = rate,
                        DiscountAmount = discount,
                        TaxAmount = 0.00m,
                        NetAmount = net,
                        PaidAmount = paid,
                        DueAmount = 0.00m,
                        InvoiceStatus = InvoiceStatus.Paid,
                        CreatedOn = DateTime.UtcNow.AddDays(-i - 1),
                        CreatedBy = "System"
                    };
                    context.Invoices.Add(invoice);
                    await context.SaveChangesAsync();

                    // Create Payment
                    var paymentNum = $"PAY-2026-{i + 1:D6}";
                    var payment = new Payment
                    {
                        InvoiceId = invoice.Id,
                        PaymentNumber = paymentNum,
                        PaymentDate = DateTime.UtcNow.AddDays(-i - 1),
                        Amount = paid,
                        PaymentMode = i % 2 == 0 ? "UPI" : "Cash",
                        TransactionReference = i % 2 == 0 ? "TXN123456" + i : null,
                        ReceivedBy = "System",
                        CreatedOn = DateTime.UtcNow.AddDays(-i - 1),
                        CreatedBy = "System"
                    };
                    context.Payments.Add(payment);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
