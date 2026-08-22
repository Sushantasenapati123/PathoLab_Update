using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Patholab.Application.Interfaces;
using Patholab.Domain.Entities;
using Patholab.Shared.DTOs;
using Patholab.Shared.Models;

namespace Patholab.Application.Services
{
    public class TestService : ITestService
    {
        private readonly IApplicationDbContext _context;

        public TestService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TestDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var t = await _context.Tests
                .Include(x => x.Department)
                .Include(x => x.SampleType)
                .Include(x => x.TestParameters)
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedFlag, cancellationToken);

            if (t == null) throw new Exception("Test not found.");
            return MapToDto(t);
        }

        public async Task<PagedResult<TestDto>> GetAllPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default)
        {
            var query = _context.Tests
                .Include(x => x.Department)
                .Include(x => x.SampleType)
                .Where(x => !x.DeletedFlag);

            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                query = query.Where(x => x.TestCode.ToLower().Contains(s) || 
                                         x.TestName.ToLower().Contains(s) || 
                                         x.Department.DepartmentName.ToLower().Contains(s));
            }

            var totalRecords = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var items = await query
                .OrderBy(x => x.TestName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<TestDto>
            {
                Items = items.Select(MapToDto).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }

        public async Task<TestDto> CreateAsync(TestDto dto, CancellationToken cancellationToken = default)
        {
            var t = new Test
            {
                TestCode = dto.TestCode,
                TestName = dto.TestName,
                DepartmentId = dto.DepartmentId,
                SampleTypeId = dto.SampleTypeId,
                TestType = dto.TestType,
                Description = dto.Description,
                Price = dto.Price,
                TATMinutes = dto.TATMinutes,
                IsActive = dto.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System"
            };

            foreach (var p in dto.TestParameters)
            {
                t.TestParameters.Add(new TestParameter
                {
                    ParameterCode = p.ParameterCode,
                    ParameterName = p.ParameterName,
                    DisplayOrder = p.DisplayOrder,
                    ResultType = p.ResultType,
                    Unit = p.Unit,
                    MaleMin = p.MaleMin,
                    MaleMax = p.MaleMax,
                    FemaleMin = p.FemaleMin,
                    FemaleMax = p.FemaleMax,
                    ChildMin = p.ChildMin,
                    ChildMax = p.ChildMax,
                    DefaultReferenceRange = p.DefaultReferenceRange,
                    CriticalLow = p.CriticalLow,
                    CriticalHigh = p.CriticalHigh,
                    IsRequired = p.IsRequired,
                    IsActive = p.IsActive,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "System"
                });
            }

            _context.Tests.Add(t);
            await _context.SaveChangesAsync(cancellationToken);

            return await GetByIdAsync(t.Id, cancellationToken);
        }

        public async Task<TestDto> UpdateAsync(int id, TestDto dto, CancellationToken cancellationToken = default)
        {
            var t = await _context.Tests
                .Include(x => x.TestParameters)
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedFlag, cancellationToken);

            if (t == null) throw new Exception("Test not found.");

            t.TestCode = dto.TestCode;
            t.TestName = dto.TestName;
            t.DepartmentId = dto.DepartmentId;
            t.SampleTypeId = dto.SampleTypeId;
            t.TestType = dto.TestType;
            t.Description = dto.Description;
            t.Price = dto.Price;
            t.TATMinutes = dto.TATMinutes;
            t.IsActive = dto.IsActive;
            t.UpdatedOn = DateTime.UtcNow;
            t.UpdatedBy = "System";

            // Remove parameters that are no longer in the request
            var existingParamIds = dto.TestParameters.Select(p => p.Id).ToList();
            var paramsToRemove = t.TestParameters.Where(p => !existingParamIds.Contains(p.Id)).ToList();
            foreach (var p in paramsToRemove)
            {
                _context.TestParameters.Remove(p);
            }

            // Add or update parameters
            foreach (var pDto in dto.TestParameters)
            {
                if (pDto.Id == 0)
                {
                    t.TestParameters.Add(new TestParameter
                    {
                        ParameterCode = pDto.ParameterCode,
                        ParameterName = pDto.ParameterName,
                        DisplayOrder = pDto.DisplayOrder,
                        ResultType = pDto.ResultType,
                        Unit = pDto.Unit,
                        MaleMin = pDto.MaleMin,
                        MaleMax = pDto.MaleMax,
                        FemaleMin = pDto.FemaleMin,
                        FemaleMax = pDto.FemaleMax,
                        ChildMin = pDto.ChildMin,
                        ChildMax = pDto.ChildMax,
                        DefaultReferenceRange = pDto.DefaultReferenceRange,
                        CriticalLow = pDto.CriticalLow,
                        CriticalHigh = pDto.CriticalHigh,
                        IsRequired = pDto.IsRequired,
                        IsActive = pDto.IsActive,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = "System"
                    });
                }
                else
                {
                    var param = t.TestParameters.FirstOrDefault(p => p.Id == pDto.Id);
                    if (param != null)
                    {
                        param.ParameterCode = pDto.ParameterCode;
                        param.ParameterName = pDto.ParameterName;
                        param.DisplayOrder = pDto.DisplayOrder;
                        param.ResultType = pDto.ResultType;
                        param.Unit = pDto.Unit;
                        param.MaleMin = pDto.MaleMin;
                        param.MaleMax = pDto.MaleMax;
                        param.FemaleMin = pDto.FemaleMin;
                        param.FemaleMax = pDto.FemaleMax;
                        param.ChildMin = pDto.ChildMin;
                        param.ChildMax = pDto.ChildMax;
                        param.DefaultReferenceRange = pDto.DefaultReferenceRange;
                        param.CriticalLow = pDto.CriticalLow;
                        param.CriticalHigh = pDto.CriticalHigh;
                        param.IsRequired = pDto.IsRequired;
                        param.IsActive = pDto.IsActive;
                        param.UpdatedOn = DateTime.UtcNow;
                        param.UpdatedBy = "System";
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            return await GetByIdAsync(t.Id, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var t = await _context.Tests
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedFlag, cancellationToken);
            if (t == null) throw new Exception("Test not found.");

            t.DeletedFlag = true;
            t.UpdatedOn = DateTime.UtcNow;
            t.UpdatedBy = "System";

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<DepartmentDto>> GetDepartmentsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .Where(x => x.IsActive && !x.DeletedFlag)
                .Select(x => new DepartmentDto
                {
                    Id = x.Id,
                    DepartmentCode = x.DepartmentCode,
                    DepartmentName = x.DepartmentName,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .OrderBy(x => x.DepartmentName)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SampleTypeDto>> GetSampleTypesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SampleTypes
                .Where(x => x.IsActive && !x.DeletedFlag)
                .Select(x => new SampleTypeDto
                {
                    Id = x.Id,
                    SampleTypeCode = x.SampleTypeCode,
                    SampleTypeName = x.SampleTypeName,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .OrderBy(x => x.SampleTypeName)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TestPackageDto>> GetPackagesAsync(CancellationToken cancellationToken = default)
        {
            var packages = await _context.TestPackages
                .Include(x => x.PackageTests)
                .ThenInclude(pt => pt.Test)
                .Where(x => x.IsActive && !x.DeletedFlag)
                .ToListAsync(cancellationToken);

            return packages.Select(p => new TestPackageDto
            {
                Id = p.Id,
                PackageCode = p.PackageCode,
                PackageName = p.PackageName,
                Description = p.Description,
                OriginalPrice = p.OriginalPrice,
                PackagePrice = p.PackagePrice,
                DiscountAmount = p.DiscountAmount,
                IsActive = p.IsActive,
                TestIds = p.PackageTests.Select(pt => pt.TestId).ToList(),
                Tests = p.PackageTests.Select(pt => MapToDto(pt.Test)).ToList()
            }).ToList();
        }

        private static TestDto MapToDto(Test t)
        {
            return new TestDto
            {
                Id = t.Id,
                TestCode = t.TestCode,
                TestName = t.TestName,
                DepartmentId = t.DepartmentId,
                DepartmentName = t.Department?.DepartmentName ?? string.Empty,
                SampleTypeId = t.SampleTypeId,
                SampleTypeName = t.SampleType?.SampleTypeName ?? string.Empty,
                TestType = t.TestType,
                Description = t.Description,
                Price = t.Price,
                TATMinutes = t.TATMinutes,
                IsActive = t.IsActive,
                TestParameters = t.TestParameters?.Select(p => new TestParameterDto
                {
                    Id = p.Id,
                    TestId = p.TestId,
                    ParameterCode = p.ParameterCode,
                    ParameterName = p.ParameterName,
                    DisplayOrder = p.DisplayOrder,
                    ResultType = p.ResultType,
                    Unit = p.Unit,
                    MaleMin = p.MaleMin,
                    MaleMax = p.MaleMax,
                    FemaleMin = p.FemaleMin,
                    FemaleMax = p.FemaleMax,
                    ChildMin = p.ChildMin,
                    ChildMax = p.ChildMax,
                    DefaultReferenceRange = p.DefaultReferenceRange,
                    CriticalLow = p.CriticalLow,
                    CriticalHigh = p.CriticalHigh,
                    IsRequired = p.IsRequired,
                    IsActive = p.IsActive
                }).OrderBy(p => p.DisplayOrder).ToList() ?? new List<TestParameterDto>()
            };
        }
    }
}
