using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Frontend.Services;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Enums;
using System;

namespace Orders.Frontend.Pages.Auth
{
    public partial class Register
    {
        private UserDTO userDTO = new();
        private List<Country>? countries;
        private List<State>? states;
        private List<City>? cities;
        private bool loading;
        private string? imageUrl;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private ILoginService LoginService { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadCountriesAsync();
        }

        private void ImageSelected(string imagenBase64)
        {
            userDTO.Photo = imagenBase64;
            imageUrl = null;
        }

        private async Task LoadCountriesAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Country>>("/api/countries/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            countries = responseHttp.Response;
        }

        private async Task CountryChangedAsync(ChangeEventArgs e)
        {
            var countryId = Convert.ToInt32(e.Value!);
            states = null;
            cities = null;
            userDTO.CityId = 0;
            await LoadStatesAsyn(countryId);
        }

        private async Task StateChangedAsync(ChangeEventArgs e)
        {
            var stateId = Convert.ToInt32(e.Value!);
            cities = null;
            userDTO.CityId = 0;
            await LoadCitiesAsyn(stateId);
        }

        private async Task LoadStatesAsyn(int countryId)
        {
            var responseHttp = await Repository.GetAsync<List<State>>($"/api/states/combo/{countryId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            states = responseHttp.Response;
        }

        private async Task LoadCitiesAsyn(int stateId)
        {
            var responseHttp = await Repository.GetAsync<List<City>>($"/api/cities/combo/{stateId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            cities = responseHttp.Response;
        }

        private async Task CreateUserAsync()
        {
            userDTO.UserName = userDTO.Email;
            userDTO.UserType = UserType.User;
            loading = true;
            var responseHttp = await Repository.PostAsync<UserDTO>("/api/accounts/CreateUser", userDTO);
            loading = false;

            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            await SweetAlertService.FireAsync("Confirmaci�n", "Su cuenta ha sido creada con �xito. Se te ha enviado un correo electr�nico con las instrucciones para activar tu usuario.", SweetAlertIcon.Info);
            NavigationManager.NavigateTo("/");
        }
    }
}