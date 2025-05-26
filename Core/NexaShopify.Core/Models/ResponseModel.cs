using System.Collections.Generic;

namespace NexaShopify.Core.Models
{
	public class ResponseModel<T>
	{
		public T Body { get; set; }
		public bool Success { get; set; } = false;
		public List<string> Infos { get; set; } = new List<string>();
		public List<string> Warnings { get; set; } = new List<string>();
		public List<string> Errors { get; set; } = new List<string>();

		public ResponseModel()
		{ }
		public ResponseModel(T body)
		{
			this.Body = body;
		}

		public static ResponseModel<T> SuccessResponse()
		{
			return new ResponseModel<T>()
			{
				Success = true
			};
		}

		public static ResponseModel<T> SuccessResponse(T body)
		{
			return new ResponseModel<T>()
			{
				Success = true,
				Body = body
			};
		}
		public static ResponseModel<T> FailureResponse()
		{
			return new ResponseModel<T>()
			{
				Success = false,
			};
		}
		public static ResponseModel<T> FailureResponse(string error)
		{
			return new ResponseModel<T>()
			{
				Success = false,
				Errors = new List<string> { error }
			};
		}
		public static ResponseModel<T> FailureResponse(List<string> error)
		{
			return new ResponseModel<T>()
			{
				Success = false,
				Errors = error
			};
		}

		public static ResponseModel<T> AccessDeniedResponse()
		{
			return new ResponseModel<T>() { Errors = new List<string>() { "Accès réfusé" } };
		}

		public static ResponseModel<T> WrongPasswordResponse()
		{
			return new ResponseModel<T>() { Errors = new List<string>() { "Mot de passe incorrect" } };
		}

		public static ResponseModel<T> UnexpectedErrorResponse()
		{
			return new ResponseModel<T>() { Errors = new List<string>() { "Une erreur inattendue s'est produite" } };
		}


	}
}
