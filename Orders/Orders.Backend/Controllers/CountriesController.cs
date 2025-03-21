﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Implementations;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class CountriesController : GenericController<Country>
    {
        private readonly ICountriesUnitsOfWork _countriesUnitsOfWork;

        public CountriesController(IGenericUnitsOfWork<Country> unitsOfWork, ICountriesUnitsOfWork countriesUnitsOfWork) : base(unitsOfWork)
        {
            _countriesUnitsOfWork = countriesUnitsOfWork;
        }

        [AllowAnonymous]
        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _countriesUnitsOfWork.GetComboAsync());
        }


        [HttpGet("full")]
        public override async Task<IActionResult> GetAsync()
        {
            var action = await _countriesUnitsOfWork.GetAsync();
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
        {
            var response = await _countriesUnitsOfWork.GetAsync(pagination);

            if (response.WassSuccees)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        //[HttpGet("totalPages")]
        //public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        //{
        //    var action = await _countriesUnitsOfWork.GetTotalPagesAsync(pagination);
        //    if (action.WassSuccees)
        //    {
        //        return Ok(action.Result);
        //    }
        //    return BadRequest();
        //}

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var action = await _countriesUnitsOfWork.GetAsync(id);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return NotFound();
        }

        [HttpGet("pagination")]
        public override async Task<IActionResult> GetPaginationAsync(PaginationDTO pagination)
        {
            var action = await _countriesUnitsOfWork.GetPaginationAsync(pagination);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }


        [HttpGet("totalRecord")]
        public override async Task<IActionResult> GetTotalRecordAsync(PaginationDTO pagination)
        {
            var action = await _countriesUnitsOfWork.GetTotalRecordAsync(pagination);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }


    }
}