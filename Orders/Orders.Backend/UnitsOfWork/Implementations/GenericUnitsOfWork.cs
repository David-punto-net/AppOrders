﻿using Orders.Backend.Repositories.Interfaces;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Response;

namespace Orders.Backend.UnitsOfWork.Implementations
{
    public class GenericUnitsOfWork<T> : IGenericUnitsOfWork<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;

        public GenericUnitsOfWork(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual async Task<ActionResponse<T>> AddAsync(T model) => await _repository.AddAsync(model);

        public virtual async Task<ActionResponse<T>> DeleteAsync(int id) => await _repository.DeleteAsync(id);

        public virtual async Task<ActionResponse<T>> GetAsync(int id) => await _repository.GetAsync(id);

        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync() => await _repository.GetAsync();

        public virtual async Task<ActionResponse<IEnumerable<T>>> GetPaginationAsync(PaginationDTO pagination) => await _repository.GetPaginationAsync(pagination);

        public virtual async Task<ActionResponse<int>> GetTotalRecordAsync(PaginationDTO pagination) => await _repository.GetTotalRecordAsync(pagination);

        public virtual async Task<ActionResponse<T>> UpdateAsync(T model) => await _repository.UpdateAsync(model);

        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination) => await _repository.GetAsync(pagination);
        
        public virtual async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _repository.GetTotalPagesAsync(pagination);
    }
}