﻿using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Response;
using System.Linq;

namespace Orders.Backend.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext _context;
        private readonly DbSet<T> _entity;

        public GenericRepository(DataContext context)
        {
            _context = context;
            _entity = _context.Set<T>();
        }

        public virtual async Task<ActionResponse<T>> GetAsync(int id)
        {
            var row = await _entity.FindAsync(id);
            if (row is null)
            {
                return new ActionResponse<T>
                {
                    WassSuccees = false,
                    Message = "Registro no encontrado."
                };
            }

            return new ActionResponse<T>
            {
                WassSuccees = true,
                Result = row
            };
        }

        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync()
        {
            return new ActionResponse<IEnumerable<T>>
            {
                WassSuccees = true,
                Result = await _entity.ToListAsync()
            };
        }

        public virtual async Task<ActionResponse<IEnumerable<T>>> GetPaginationAsync(PaginationDTO pagination)
        {
            return new ActionResponse<IEnumerable<T>>
            {
                WassSuccees = true,
                Result = await _entity.Skip(pagination.Page).Take(pagination.RecordsNumber).ToListAsync()
            };
        }

        public virtual async Task<ActionResponse<int>> GetTotalRecordAsync(PaginationDTO pagination)
        {
            var queryable = _entity.AsQueryable();
            int count = await queryable.CountAsync();
            return new ActionResponse<int>
            {
                WassSuccees = true,
                Result = count
            };
        }

        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _entity.AsQueryable();
            return new ActionResponse<IEnumerable<T>>
            {
                WassSuccees = true,
                Result = await queryable
                                .Paginate(pagination)
                                .ToListAsync()
            };
        }
        public virtual async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _entity.AsQueryable();
            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                WassSuccees = true,
                Result = totalPages
            };
        }

        public virtual async Task<ActionResponse<T>> AddAsync(T entity)
        {
            _context.Add(entity);
            try
            {
                await _context.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    WassSuccees = true,
                    Result = entity
                };
            }
            catch (DbUpdateException ex)
            {

                if (ex.InnerException != null)
                {
                    if (ex.InnerException!.Message.Contains("duplicate"))
                    {
                        return DbUpdateExceptionActionResponse();
                    }
                }

                return new ActionResponse<T>
                {
                    WassSuccees = false,
                    Message = ex.Message
                };
            }
            catch (Exception exception)
            {
                return ExceptionActionResponse(exception);
            }
        }

        public virtual async Task<ActionResponse<T>> DeleteAsync(int id)
        {
            var row = await _entity.FindAsync(id);
            if (row is null)
            {
                return new ActionResponse<T>
                {
                    WassSuccees = false,
                    Message = "Registro no encontrado."
                };
            }

            try
            {
                _entity.Remove(row);
                await _context.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    WassSuccees = true,
                };
            }
            catch
            {
                return new ActionResponse<T>
                {
                    WassSuccees = false,
                    Message = "No se puede borrar porque existen registros relacionados."
                };
            }
        }


        public virtual async Task<ActionResponse<T>> UpdateAsync(T entity)
        {
            _context.Update(entity);
            try
            {
                await _context.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    WassSuccees = true,
                    Result = entity
                };
            }
            catch (DbUpdateException ex)
            {
                if(ex.InnerException != null)
                {
                    if (ex.InnerException!.Message.Contains("duplicate"))
                    {
                        return DbUpdateExceptionActionResponse();
                    }
                }

                return new ActionResponse<T>
                {
                    WassSuccees = false,
                    Message = ex.Message
                };
            }
            catch (Exception exception)
            {
                return ExceptionActionResponse(exception);
            }
        }

        private ActionResponse<T> DbUpdateExceptionActionResponse()
        {
            return new ActionResponse<T>
            {
                WassSuccees = false,
                Message = "Ya existe el registro que esta intentado crear."
            };
        }

        private ActionResponse<T> ExceptionActionResponse(Exception exception)
        {
            return new ActionResponse<T>
            {
                WassSuccees = false,
                Message = exception.Message
            };
        }
    }
}