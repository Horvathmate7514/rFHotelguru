using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Hotelguru.DataContext.Context;
using Hotelguru.DataContext.Dtos;
using Hotelguru.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hotelguru.Services
{
    public interface IBenefitService
    {
        Task<BenefitDto> BenefitCreateAsync(BenefitCreateDto createDto);
        Task<List<BenefitDto>> BenefitGetAllAsync();
        Task<BenefitDto?> BenefitGetByIdAsync(int id);
        Task<BenefitDto?> BenefitUpdateAsync(int id, BenefitUpdateDto updateDto);
        Task<bool> BenefitDeleteAsync(int id);
    }

    public class BenefitService : IBenefitService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BenefitService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<BenefitDto> BenefitCreateAsync(BenefitCreateDto createDto)
        {
            var benefit = _mapper.Map<DataContext.Entities.Benefit>(createDto);
            _context.Benefits.Add(benefit);
            await _context.SaveChangesAsync();
            return _mapper.Map<BenefitDto>(benefit);
        }
        public async Task<List<BenefitDto>> BenefitGetAllAsync()
        {
            var benefits = await _context.Benefits.ToListAsync();
            return _mapper.Map<List<BenefitDto>>(benefits);
        }
        public async Task<BenefitDto?> BenefitGetByIdAsync(int id)
        {
            var benefit = await _context.Benefits.FindAsync(id);
            if (benefit == null) return null;
            return _mapper.Map<BenefitDto?>(benefit);
        }
        public async Task<BenefitDto?> BenefitUpdateAsync(int id, BenefitUpdateDto updateDto)
        {
            var benefit = await _context.Benefits.FindAsync(id);
            if (benefit == null) return null;
            _mapper.Map(updateDto, benefit);
            await _context.SaveChangesAsync();
            return _mapper.Map<BenefitDto>(benefit);
        }
        public async Task<bool> BenefitDeleteAsync(int id)
        {
            var benefit = await _context.Benefits.FindAsync(id);
            if (benefit == null) return false;
            _context.Benefits.Remove(benefit);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
