using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HealthCare.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthCare.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public IndexModel(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            //agregado

            [DataType(DataType.Text)]
            [Display(Name = "Nombre")]
            [Required(ErrorMessage = "{0} es requerido.")]
            public string Nombre { get; set; }


            [DataType(DataType.Text)]
            [Display(Name = "Apellido")]
            [Required(ErrorMessage = "{0} es requerido.")]
            public string Apellido { get; set; }

            
            [Phone(ErrorMessage ="El Número Celular no es un número de celular válido.")]            
            [Display(Name = "Número Celular")]
//            [Required(ErrorMessage = "{0} es requerido.")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(Usuario user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                //agregado
                Nombre = user.Nombre,
                Apellido= user.Apellido,
                //----------

                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Incapaz de cargar usuario con ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Incapaz de cargar usuario con ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Error inesperado mientras intentaba guardar número de celular.";
                    return RedirectToPage();
                }
            }

            //agregado
            if (Input.Nombre != user.Nombre)
            {
                user.Nombre = Input.Nombre;
            }

            if (Input.Apellido != user.Apellido)
            {
                user.Apellido = Input.Apellido;
            }
            await _userManager.UpdateAsync(user);
            //-------------se agrego hasta aca----------------

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Su perfil ha sido actualizado";
            return RedirectToPage();
        }
    }
}
