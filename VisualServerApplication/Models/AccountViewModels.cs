using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VisualServerApplication.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "전자 메일")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "코드")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "이 브라우저 기억")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "전자 메일")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "채널")]
        [StringLength(20, MinimumLength = 2/*, ErrorMessage = "아이디 올바르지 않습니다. "*/)]
        public string CHNL_CD { get; set; }

        [Required]
        [Display(Name = "아이디")]
        [StringLength(20, MinimumLength = 1/*, ErrorMessage = "아이디 올바르지 않습니다. "*/)]
        public string USR_ID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 1/*, ErrorMessage = "암호 올바르지 않습니다. "*/)]
        [Display(Name = "암호")]
        public string USR_PWD { get; set; }

        [Display(Name = "사용자 아이디 및 암호 저장")]
        public bool RememberMe { get; set; }
    }


    public class RegisterEqViewModel
    {
        [Required]
        [Display(Name = "LOT NO")]
        [StringLength(20, MinimumLength = 2/*, ErrorMessage = "아이디 올바르지 않습니다. "*/)]
        public string PROC_LOT_NO { get; set; }

       
    }

    //public class RegisterViewModel
    //{
    //    [Required]
    //    [EmailAddress]
    //    [Display(Name = "전자 메일")]
    //    public string Email { get; set; }

    //    [Required]
    //    [StringLength(100, ErrorMessage = "{0}은(는) {2}자 이상이어야 합니다.", MinimumLength = 6)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "암호")]
    //    public string Password { get; set; }

    //    [DataType(DataType.Password)]
    //    [Display(Name = "암호 확인")]
    //    [Compare("Password", ErrorMessage = "암호와 확인 암호가 일치하지 않습니다.")]
    //    public string ConfirmPassword { get; set; }
    //}

    //public class ResetPasswordViewModel
    //{
    //    [Required]
    //    [EmailAddress]
    //    [Display(Name = "전자 메일")]
    //    public string Email { get; set; }

    //    [Required]
    //    [StringLength(100, ErrorMessage = "{0}은(는) {2}자 이상이어야 합니다.", MinimumLength = 6)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "암호")]
    //    public string Password { get; set; }

    //    [DataType(DataType.Password)]
    //    [Display(Name = "암호 확인")]
    //    [Compare("Password", ErrorMessage = "암호와 확인 암호가 일치하지 않습니다.")]
    //    public string ConfirmPassword { get; set; }

    //    public string Code { get; set; }
    //}

    //public class ForgotPasswordViewModel
    //{
    //    [Required]
    //    [EmailAddress]
    //    [Display(Name = "전자 메일")]
    //    public string Email { get; set; }
    //}
}
