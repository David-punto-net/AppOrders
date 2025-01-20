﻿using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Frontend.Shared;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.States
{
    public partial class StateCreate
    {
        private State state = new();

        private FormWithName<State>? stateForm;
        [Parameter] public int CountryId { get; set; }

        private bool regreso = false;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
     

        private async Task CreateAsync()
        {
            if (!regreso)
            {
                stateForm!.FormPressCreate = true;

                state.CountryId = CountryId;
                var responseHttp = await Repository.PostAsync("/api/states", state);
                if (responseHttp.Error)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    return;
                }

                NavigationManager.NavigateTo($"/countries/details/{CountryId}");

                var toast = SweetAlertService.Mixin(new SweetAlertOptions
                {
                    Toast = true,
                    Position = SweetAlertPosition.BottomEnd,
                    ShowConfirmButton = true,
                    Timer = 3000
                });

                await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");
            }
        }

        private async Task Return()
        {
            regreso = true;
            stateForm!.FormPressCreate = false;
            NavigationManager.NavigateTo($"/countries/details/{CountryId}");
        }
    }
}
